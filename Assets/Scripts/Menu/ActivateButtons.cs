using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActivateButtons : MonoBehaviour
{
    [SerializeField] private Image fade;

    public void Activate()
    {
        transform.SetSiblingIndex(3);
    }

    public void NextScene()
    {
        StartCoroutine(ChangeScene());
    }

    public IEnumerator ChangeScene()
    {
        transform.SetSiblingIndex(0);

        fade.GetComponent<Animator>().SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        Manager.Instance.NextScene();
    }
}
