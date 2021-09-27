using UnityEngine;
using UnityEngine.UI;

public class HandInInteractable : MonoBehaviour, IInteractable
{
    #region Variables
    [SerializeField] private GameObject         door;
    [SerializeField] private FreezePlayerStates player;
    [SerializeField] private Image              cv;
    #endregion

    #region Interact
    public void Interact()
    {
        door.GetComponent<DoorInteract>().NowInteract();
        player.ChangeState(States.FROZEN);

        gameObject.tag                                                      = "Untagged";
        transform.GetChild(0).GetComponent<Outline>().enabled               = false;

        cv.GetComponent<DisplayCV>().ShowCV();
    }

    public string DisplayPrompt()
    {
        return "Press E to hand in CV";
    }
    #endregion
}
