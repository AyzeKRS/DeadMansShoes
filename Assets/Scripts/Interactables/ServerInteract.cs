using UnityEngine;

public class ServerInteract : MonoBehaviour, IInteractable
{
    [HideInInspector] public int                num     = -1;
    [SerializeField] private Material[]         mat     = { null, null, null, null, null, null };
    [SerializeField] private ThirdMinigame      tm;

    public string DisplayPrompt()
    {
        return "Press E to change colour";
    }

    public void Interact()
    {
        num++;
        if (num == 6)
            num = 0;

        GetComponent<Renderer>().material = mat[num];

        tm.CheckCode();

        Audio.Instance.Play2DSound("Button");
    }
}
