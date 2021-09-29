using System.Collections;
using UnityEngine;
using TMPro;

public class PSubtitles : MonoBehaviour
{
    #region Variables
    [SerializeField] private FreezePlayerStates player;
    [SerializeField] private Outline            first_obj;
    [SerializeField] private Outline            sec_obj;
    [SerializeField] private TMP_Text           player_text;
    [SerializeField] private TMP_Text           receptionist_text;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        player.ChangeState(States.FROZEN);
        StartCoroutine(StartSubtitles());
    }
    #endregion

    #region Subtitles
    private IEnumerator StartSubtitles()
    {
        yield return new WaitForSeconds(0.5f);

        Audio.Instance.Play3DLocal("LiftBing", player.gameObject);

        yield return new WaitForSeconds(0.75f);

        Audio.Instance.Play3DLocal("LiftOpen", player.gameObject);

        yield return new WaitForSeconds(1.75f);

        GetComponent<TypeWriter>().RunText("Oh no", player_text, 25.0f);

        yield return new WaitForSeconds(1.5f);

        GetComponent<TypeWriter>().RunText("I'm running late, there is already a long queue", player_text, 25.0f);

        yield return new WaitForSeconds(3.5f);

        GetComponent<TypeWriter>().RunText("I need to get to the front. Quick.", player_text, 25.0f);

        yield return new WaitForSeconds(3.5f);

        player_text.text = string.Empty;

        player.ChangeState(States.MOVE);
        first_obj.enabled = true;
    }

    public IEnumerator ReceptionistSubtitles()
    {
        yield return new WaitForSeconds(0.5f);
        
        GetComponent<TypeWriter>().RunText("We have never been this impressed by a CV before", receptionist_text, 25.0f);

        yield return new WaitForSeconds(4.0f);

        GetComponent<TypeWriter>().RunText("Congratulations! Walk through the door to start your new job", receptionist_text, 25.0f);

        yield return new WaitForSeconds(4.0f);

        receptionist_text.text = string.Empty;

        GetComponent<TypeWriter>().RunText("Thanks", player_text, 25.0f);

        yield return new WaitForSeconds(2.0f);

        player_text.text = string.Empty;

        player.ChangeState(States.MOVE);
        sec_obj.enabled = true;
    }
    #endregion
}
