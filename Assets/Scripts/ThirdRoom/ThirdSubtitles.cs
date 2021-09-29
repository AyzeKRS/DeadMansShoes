using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class ThirdSubtitles : MonoBehaviour
{
    #region Variables
    [SerializeField] private FreezePlayerStates     player;
    [SerializeField] private TMP_Text               player_text;
    [SerializeField] private TMP_Text               title_text;
    [SerializeField] private FadeScreen             fade;
    [SerializeField] private Outline                outline1;
    [SerializeField] private Outline                outline2;
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
            if (Manager.Instance.killed[1])
                title_text.text = "1 month after John's 'suicide'...";
            else
                title_text.text = "1 month after John's forced removal...";
        }
        catch (Exception e) // For testing level without singleton
        {
            title_text.text = "1 month later...";
        }
    }

    private IEnumerator StartSubtitles()
    {
        TitleSubtitle();

        title_text.GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(2.5f);

        title_text.GetComponent<Animator>().SetTrigger("Fade");

        yield return new WaitForSeconds(1.5f);

        GetComponent<TypeWriter>().RunText("The server has shut down...", player_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("Again...", player_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        GetComponent<TypeWriter>().RunText("Tim was in charge of server management", player_text, 25.0f);

        yield return new WaitForSeconds(3.5f);

        GetComponent<TypeWriter>().RunText("But he's hidden somewhere in the storage room", player_text, 25.0f);

        yield return new WaitForSeconds(4.0f);

        GetComponent<TypeWriter>().RunText("I could fix the server myself", player_text, 25.0f);

        yield return new WaitForSeconds(3.5f);

        GetComponent<TypeWriter>().RunText("Or 'forcefully' remove Tim...", player_text, 25.0f);

        yield return new WaitForSeconds(3.5f);

        player_text.text = string.Empty;

        player.ChangeState(States.MOVE);
        outline1.enabled = true;
        outline2.enabled = true;
    }

    public IEnumerator MinigameEndSubtitles()
    {
        player.ChangeState(States.FROZEN);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("I don't think this has been updated since 1992", player_text, 20.0f);

        yield return new WaitForSeconds(3.5f);

        player_text.text = string.Empty;

        fade.FadeOut();

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.NextScene(true);
    }

    public IEnumerator PistolSubtitles()
    {
        player.ChangeState(States.FROZEN);

        yield return new WaitForSeconds(1.5f);

        GetComponent<TypeWriter>().RunText("'WARNING: PISTOL HAS INCONVENIENTLY JAMMED'", player_text, 20.0f);

        yield return new WaitForSeconds(3.5f);

        GetComponent<TypeWriter>().RunText("Who said that?", player_text, 20.0f);

        yield return new WaitForSeconds(2.0f);

        player_text.text = string.Empty;

        player.ChangeState(States.MOVE);
    }

    public IEnumerator KillEndSubtitles()
    {
        player.ChangeState(States.FROZEN);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("...", player_text, 20.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("I'm late for my meeting", player_text, 20.0f);

        yield return new WaitForSeconds(3.0f);

        player_text.text = string.Empty;

        fade.FadeOut();

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.NextScene(false);
    }
    #endregion
}
