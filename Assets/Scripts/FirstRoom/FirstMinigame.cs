using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstMinigame : MonoBehaviour
{
    #region Variables
    private int                                 score           = 0;
    [HideInInspector] public bool               in_minigame     = false;
    [HideInInspector] public bool               error           = false;
    [HideInInspector] public bool               waiting         = true;
    private bool                                cooldown        = false;

    [SerializeField] private FadeScreen         fade;
    [SerializeField] private Image              score_fade;
    [SerializeField] private GameObject         player;
    [SerializeField] private GameObject         pc;
    [SerializeField] private TMP_Text           text;
    [SerializeField] private FirstSubtitles     fs;
    [SerializeField] private Image              screen;
    [SerializeField] private RawImage           crosshair;
    [SerializeField] private Outline            outline;

    [SerializeField] private string[]           award           = { "", "", "" };
    [SerializeField] private string[]           insults         = { "", "", "", "", "" };
    #endregion

    #region BuiltIn Functions
    private void Update() // If I got time, I will sort out this mess, there is a lot of mess here that could easily be cleared up (more functions, avoiding unneccessary booleans etc...)
    {
        if (!waiting)
        {
            if (in_minigame && !error)
            {
                crosshair.enabled   = false;
                outline.enabled     = false;
                pc.tag              = "Untagged";
                pc.transform.GetChild(0).GetComponent<Outline>().enabled = false;

                fade.Dim();

                player.GetComponent<FreezePlayerStates>().ChangeState(States.FROZEN);
                player.transform.GetChild(1).GetComponent<CameraLook>().ShowCursor();

                transform.GetChild(score).gameObject.SetActive(true);

                if (Input.GetButtonDown("Interact") && !cooldown)
                {
                    in_minigame = false;
                }
            }

            else if (in_minigame && error)
            {
                // Lazy fix but everything works as intended
            }

            else if (!in_minigame && error)
            {
                // Bad set up in places has lead to unintended logic, including this to stop the player from looking around
                player.GetComponent<FreezePlayerStates>().ChangeState(States.FROZEN);
                player.transform.GetChild(1).GetComponent<CameraLook>().HideCursor();
            }

            else
            {
                transform.GetChild(score).gameObject.SetActive(false);
                fade.Show();

                player.GetComponent<FreezePlayerStates>().ChangeState(States.ROTATE);
                player.transform.GetChild(1).GetComponent<CameraLook>().HideCursor();

                crosshair.enabled   = true;
                outline.enabled     = true;
                pc.tag              = "Interactable";
                pc.transform.GetChild(0).GetComponent<Outline>().enabled = true;
            }
        }
    }
    #endregion

    #region Buttons Functions
    public void FinalButtonPressed()
    {
        Audio.Instance.Play2DSound("Button");

        transform.GetChild(score).gameObject.SetActive(false);
        StartCoroutine(DisplayAward());
        score++;

        StartCoroutine(Final());
    }

    public void CorrectButtonPressed()
    {
        Audio.Instance.Play2DSound("Button");

        transform.GetChild(score).gameObject.SetActive(false);
        StartCoroutine(DisplayAward());
        score++;
        transform.GetChild(score).gameObject.SetActive(true);
    }

    public void WrongButtonPressed()
    {
        StartCoroutine(Wrong());

        Audio.Instance.Play2DSound("Error");

        int num = Random.Range(0, 5);
        StartCoroutine(DisplayInsult(num));
    }

    private IEnumerator Wrong()
    {
        error = true;

        transform.GetChild(score).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(score).gameObject.SetActive(true);

        error = false;
    }

    private IEnumerator Final()
    {
        error = true;

        score_fade.GetComponent<Animator>().SetTrigger("GoodFade");

        yield return new WaitForSeconds(1.0f);

        fade.FadeDim();

        yield return new WaitForSeconds(1.5f);

        screen.enabled = true;

        fade.FadeIn();
        fade.Show();

        StartCoroutine(fs.MinigameEndSubtitles());
    }

    private IEnumerator DisplayAward()
    {
        text.text = award[score];

        yield return new WaitForSeconds(1.5f);

        text.text = string.Empty;
    }

    private IEnumerator DisplayInsult(int num)
    {
        text.text = insults[num];

        yield return new WaitForSeconds(1.5f);

        text.text = string.Empty;
    }
    #endregion

    #region Budget fix for entering minigame
    public IEnumerator StartCooldown()
    {
        cooldown = true;

        yield return new WaitForSeconds(0.25f);

        cooldown = false;
    }
    #endregion
}
