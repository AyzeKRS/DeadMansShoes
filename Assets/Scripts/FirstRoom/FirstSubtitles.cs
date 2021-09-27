using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirstSubtitles : MonoBehaviour
{
    #region Variables
    [SerializeField] private FreezePlayerStates player;
    [SerializeField] private FirstMinigame      pc;
    [SerializeField] private TMP_Text           player_text;
    [SerializeField] private TMP_Text           title_text;
    [SerializeField] private Image              fade;
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
    private IEnumerator StartSubtitles()
    {
        title_text.GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(1.5f);

        title_text.GetComponent<Animator>().SetTrigger("Fade");

        yield return new WaitForSeconds(1.5f);
        
        GetComponent<TypeWriter>().RunText("I've been working really hard", player_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("I think", player_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        GetComponent<TypeWriter>().RunText("I deserve a promotion", player_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        GetComponent<TypeWriter>().RunText("But Dave is next in line", player_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        GetComponent<TypeWriter>().RunText("I could either be a better coder than him", player_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("or somehow get rid him...", player_text, 25.0f);

        yield return new WaitForSeconds(3.0f);
        
        player_text.text = string.Empty;

        pc.waiting = false;

        player.ChangeState(States.ROTATE);
        outline1.enabled = true;
        outline2.enabled = true;
    }

    public IEnumerator MinigameEndSubtitles()
    {
        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("Work of a champion", player_text, 20.0f);

        yield return new WaitForSeconds(3.0f);

        player_text.text = string.Empty;

        fade.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.NextScene(true);
    }

    public IEnumerator KillEndSubtitles()
    {
        player.ChangeState(States.FROZEN);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("Wow...", player_text, 20.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("The glass procedurally breaking was awesome", player_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        player_text.text = string.Empty;

        fade.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.NextScene(false);
    }
    #endregion
}
