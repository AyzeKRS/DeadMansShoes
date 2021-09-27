using UnityEngine;

public class ProgramMinigame : MonoBehaviour, IInteractable
{
    [SerializeField] private FirstMinigame pc;

    public string DisplayPrompt()
    {
        return "Press E to learn programming";
    }

    public void Interact()
    {
        StartCoroutine(pc.StartCooldown());
        pc.in_minigame = true;
    }
}
