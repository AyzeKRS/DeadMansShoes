using UnityEngine;

public class GameMinigame : MonoBehaviour, IInteractable
{
    private SecondMinigame sm;

    private void Start()
    {
        sm = GetComponent<SecondMinigame>();
    }

    public string DisplayPrompt()
    {
        return "Press E to play a game";
    }

    public void Interact()
    {
        StartCoroutine(sm.Interact());
    }
}
