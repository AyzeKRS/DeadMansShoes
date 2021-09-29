using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecondSubtitles : MonoBehaviour
{
    #region Variables
    [SerializeField] private FreezePlayerStates player;
    [SerializeField] private TMP_Text           player_text;
    [SerializeField] private TMP_Text           title_text;
    [SerializeField] private FadeScreen         fade;
    [SerializeField] private Outline            outline1;
    [SerializeField] private Outline            outline2;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        title_text.GetComponent<Animator>().SetBool("StartClear", true);
        player.ChangeState(States.FROZEN);
        StartCoroutine(StartSubtitles());
    }
    #endregion

    #region Subtitles
    private void TitleSubtitle()
    {
        try
        {
            if (Manager.Instance.killed[0])
                title_text.text = "1 week after Dave's disappearence...";
            else
                title_text.text = "1 week after the job promotion...";
        }
        catch (Exception e) // For testing level without singleton
        {
            title_text.text = "1 week later...";
        }
    }

    private IEnumerator StartSubtitles()
    {
        TitleSubtitle();

        title_text.GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(2.5f);

        title_text.GetComponent<Animator>().SetTrigger("Fade");

        yield return new WaitForSeconds(1.5f);
        
        GetComponent<TypeWriter>().RunText("The boss is about to promote either John or I", player_text, 25.0f);

        yield return new WaitForSeconds(4.0f);

        GetComponent<TypeWriter>().RunText("I HAVE to get that promotion", player_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("Neither of us have yet tested our new minigame", player_text, 25.0f);

        yield return new WaitForSeconds(4.0f);

        GetComponent<TypeWriter>().RunText("I could do it", player_text, 25.0f);

        yield return new WaitForSeconds(2.5f);

        GetComponent<TypeWriter>().RunText("Or find John", player_text, 25.0f);

        yield return new WaitForSeconds(2.5f);

        GetComponent<TypeWriter>().RunText("He usually hides in the server room", player_text, 25.0f);

        yield return new WaitForSeconds(3.5f);

        GetComponent<TypeWriter>().RunText("The door code is written somewhere\nand the animator had a gun on his desk...", player_text, 25.0f);

        yield return new WaitForSeconds(5.0f);
        
        player_text.text = string.Empty;

        player.ChangeState(States.MOVE);
        outline1.enabled = true;
        outline2.enabled = true;
    }

    public IEnumerator MinigameEndSubtitles()
    {
        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("I should play games professionally", player_text, 20.0f);

        yield return new WaitForSeconds(3.0f);

        player_text.text = string.Empty;

        fade.FadeOut();

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.NextScene(true);
    }

    public IEnumerator KillEndSubtitles()
    {
        player.ChangeState(States.FROZEN);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("So thats who stole all the coffee...", player_text, 20.0f);

        yield return new WaitForSeconds(3.0f);

        player_text.text = string.Empty;

        fade.FadeOut();

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.NextScene(false);
    }
    #endregion
}
