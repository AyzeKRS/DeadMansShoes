using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondMinigame : MonoBehaviour
{
    [SerializeField] private Camera[]               cam;
    [SerializeField] private FreezePlayerStates     player;
    [SerializeField] private CharacterController    mini;

    [SerializeField] private Outline                outline;

    [SerializeField] private FadeScreen             fade;
    [SerializeField] private Animator               win_fade;
    [SerializeField] private Image                  error;
    [SerializeField] private RawImage               crosshair;


    [SerializeField] private TMP_Text               text;
    [SerializeField] private TMP_Text               press;
    [SerializeField] private string[]               insults         = { "", "", "" };
    [SerializeField] private SecondSubtitles        ss;

    public IEnumerator Interact()
    {
        tag                 = "Untagged";
        outline.enabled     = false;
        crosshair.enabled   = false;

        player.ChangeState(States.FROZEN);

        fade.FadeOut();

        yield return new WaitForSeconds(1.5f);

        cam[0].enabled = false;
        cam[1].enabled = true;

        fade.FadeIn();

        yield return new WaitForSeconds(0.75f);

        press.enabled   = true;
        mini.enabled    = true;
    }

    public IEnumerator Exit()
    {
        mini.enabled    = false;
        press.enabled   = false;

        fade.FadeOut();

        yield return new WaitForSeconds(1.5f);

        cam[1].enabled = false;
        cam[0].enabled = true;

        fade.FadeIn();

        yield return new WaitForSeconds(0.75f);

        tag                 = "Interactable";
        outline.enabled     = true;
        crosshair.enabled   = true;

        player.ChangeState(States.MOVE);
    }

    public void Win()
    {
        win_fade.SetTrigger("GoodFade");
        press.enabled   = false;
        mini.enabled    = false;
        StartCoroutine(ss.MinigameEndSubtitles());
    }

    public void Hit()
    {
        StartCoroutine(ShowError());

        int num = Random.Range(0, 3);
        StartCoroutine(DisplayInsult(num));
    }

    public IEnumerator ShowError()
    {
        mini.enabled    = false;
        error.enabled   = true;

        yield return new WaitForSeconds(1.5f);

        mini.enabled    = true;
        error.enabled   = false;
    }

    private IEnumerator DisplayInsult(int num)
    {
        text.text = insults[num];

        yield return new WaitForSeconds(1.5f);

        text.text = string.Empty;
    }
}
