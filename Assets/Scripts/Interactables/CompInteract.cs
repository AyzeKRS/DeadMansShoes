using UnityEngine;

public class CompInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private ActivatePC act;

    public string DisplayPrompt()
    {
        return "Press E to pick up component";
    }

    public void Interact()
    {
        act.has_component = true;
        Destroy(gameObject);
    }
}
