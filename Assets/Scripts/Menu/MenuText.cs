using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuText : MonoBehaviour
{
    [SerializeField] private Image          fade;
    [SerializeField] private GameObject     buttons;
    [SerializeField] private TMP_Text       title;
    [SerializeField] private TMP_Text       subtitle;

    private void Start()
    {
        Cursor.lockState    = CursorLockMode.Confined; // Ensure cursor is available when game resets (doesn't change back to default state when game resets)
        Cursor.visible      = true;
        StartCoroutine(StartText());
    }

    private IEnumerator StartText()
    {
        yield return new WaitForSeconds(1.0f);

        GetComponent<TypeWriter>().RunText("Dead Man's Shoes", title, 20.0f);

        yield return new WaitForSeconds(1.25f);

        GetComponent<TypeWriter>().RunText("How to get ahead in games development", subtitle, 35.0f);

        yield return new WaitForSeconds(3.0f);

        title.GetComponent<Animator>().SetTrigger("Fade");
        subtitle.GetComponent<Animator>().SetTrigger("Fade");
        fade.GetComponent<Animator>().SetTrigger("FadeIn");

        yield return new WaitForSeconds(1.0f);

        buttons.GetComponent<ActivateButtons>().Activate();
    }
}
