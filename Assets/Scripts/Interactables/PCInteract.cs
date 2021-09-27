using UnityEngine;
using TMPro;

public class PCInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject             component;
    [SerializeField] private GameObject             instructions;
    [SerializeField] private Outline                outline;
    [SerializeField] private ThirdMinigame          tm;
    [SerializeField] private TMP_Text[]             num_text            = { null, null, null, null, null, null };
    [SerializeField] private Color32[]              col                 =
    {
        new Color32(0, 0, 0, 0),
        new Color32(0, 0, 0, 0),
        new Color32(0, 0, 0, 0),
        new Color32(0, 0, 0, 0),
        new Color32(0, 0, 0, 0),
        new Color32(0, 0, 0, 0)
    };

    public string DisplayPrompt()
    {
        return "Press E to see instructions";
    }

    public void Interact()
    {
        tag = "Untagged";
        transform.GetChild(0).GetComponent<Outline>().enabled = false;
        outline.enabled = false;

        component.tag = "Interactable";
        component.transform.GetChild(1).GetComponent<Outline>().enabled = true;

        for (int i = 0; i < num_text.Length; i++)
            for (int j = 0; j < num_text.Length; j++)
                if (tm.code[i] == j)
                    num_text[i].color = col[j];

        instructions.SetActive(true);
    }
}
