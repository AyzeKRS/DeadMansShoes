using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class ESubtitles : MonoBehaviour
{
    #region Variables
    [SerializeField] private TMP_Text player_text;
    [SerializeField] private TMP_Text boss_text;
    [SerializeField] private TMP_Text title_text;
    [SerializeField] private FadeScreen fade;
    [SerializeField] private Transform boss;
    [SerializeField] private Transform cam;
    [SerializeField] private float spin_timer = 0.0f;
    [SerializeField] private float rot_speed = 0.0f;
    private float timer = 0.0f;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        TitleSubtitle();
        title_text.GetComponent<Animator>().SetBool("StartClear", true);
        DecideSubtitles();
    }
    #endregion

    #region Rotate
    private IEnumerator RotateBoss()
    {
        Audio.Instance.Play3DLocal("Scrape", gameObject);
        while (timer <= spin_timer)
        {
            timer += Time.deltaTime;

            boss.rotation = Quaternion.Slerp
            (
            boss.rotation,
            Quaternion.LookRotation(cam.position - boss.position),
            rot_speed * Time.deltaTime
            );

            rot_speed += 0.0025f;

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    #region Subtitles
    private void TitleSubtitle()
    {
        try
        {
            if (Manager.Instance.killed[2])
                title_text.text = "5 minutes after Tim was left to nap...";
            else
                title_text.text = "5 minutes later after fixing the server...";
        }
        catch (Exception e) // For testing level without singleton
        {
            title_text.text = "5 minutes later...";
        }
    }

    private void DecideSubtitles()
    {
        switch (Manager.Instance.score)
        {
            case 3:
                StartCoroutine(GoodEnding());
                break;

            case -3:
                StartCoroutine(BadEnding());
                break;

            default:
                StartCoroutine(NeutralEnding());
                break;
        }
    }

    private IEnumerator GoodEnding()
    {
        title_text.GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(2.5f);

        title_text.GetComponent<Animator>().SetTrigger("Fade");

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(RotateBoss());

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("Impressive work", boss_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        GetComponent<TypeWriter>().RunText("Have another promotion", boss_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        boss_text.text = string.Empty;

        GetComponent<TypeWriter>().RunText("Thanks", player_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        player_text.text = string.Empty;

        fade.FadeOut();

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.ReturnToMenu();
    }

    private IEnumerator NeutralEnding()
    {
        title_text.GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(2.5f);

        title_text.GetComponent<Animator>().SetTrigger("Fade");

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(RotateBoss());

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("We're still unsure where our employees went", boss_text, 25.0f);

        yield return new WaitForSeconds(3.5f);

        GetComponent<TypeWriter>().RunText("But you have done well", boss_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("You've earned yourself another coffee", boss_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        boss_text.text = string.Empty;

        GetComponent<TypeWriter>().RunText("Thanks", player_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        player_text.text = string.Empty;

        fade.FadeOut();

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.ReturnToMenu();
    }

    private IEnumerator BadEnding()
    {
        title_text.GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(2.5f);

        title_text.GetComponent<Animator>().SetTrigger("Fade");

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(RotateBoss());

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("What happened to all of our employees?", boss_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("You work 'ruthlessly' and are reliable", boss_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        GetComponent<TypeWriter>().RunText("I can see us working well together", boss_text, 25.0f);

        yield return new WaitForSeconds(3.0f);

        boss_text.text = string.Empty;

        fade.FadeOut();

        player_text.text = string.Empty;

        yield return new WaitForSeconds(1.25f);

        Manager.Instance.ReturnToMenu();
    }
    #endregion
}
