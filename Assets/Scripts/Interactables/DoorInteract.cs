using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoorInteract : MonoBehaviour, IInteractable
{
    #region Variables
    [SerializeField] private FreezePlayerStates     player;
    [SerializeField] private Image                  fade;
    private Animator                                anim;
    [HideInInspector] public bool                   can_interact        = false;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    #endregion

    #region Custom Functions
    public void NowInteract()
    {
        gameObject.tag = "Interactable";
    }

    public void Interact()
    {
        gameObject.tag                                                      = "Untagged";
        player.ChangeState(States.FROZEN);

        transform.GetChild(0).GetComponent<Outline>().enabled = false;

        anim.SetTrigger("Open");

        StartCoroutine(Hold());

        Audio.Instance.Play3DAway("Door", gameObject);
    }

    private IEnumerator Hold()
    {
        yield return new WaitForSeconds(1.0f);
        fade.GetComponent<FadeScreen>().FadeOut();

        yield return new WaitForSeconds(1.5f);
        Manager.Instance.NextScene();
    }

    public string DisplayPrompt()
    {
        return "Press E to start the job";
    }
    #endregion
}
