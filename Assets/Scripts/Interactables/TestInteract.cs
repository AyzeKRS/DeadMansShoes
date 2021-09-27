using System.Collections;
using UnityEngine;

public class TestInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Material[] temp;
    [SerializeField] private string[] tag_;

    public string DisplayPrompt()
    {
        return "Press E to do thing.";
    }

    public void Interact()
    {
        StartCoroutine(Change());
    }

    private IEnumerator Change()
    {
        GetComponent<MeshRenderer>().material = temp[1];

        tag = tag_[0];

        yield return new WaitForSeconds(2);

        GetComponent<MeshRenderer>().material = temp[0];

        tag = tag_[1];
    }
}
