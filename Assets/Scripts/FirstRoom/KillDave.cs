using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KillDave : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject             throwable;
    [SerializeField] private FirstSubtitles         fs;
    [SerializeField] private FirstMinigame          fm;
    [SerializeField] private Image                  fade;
    private bool                                    just_to_be_safe; // Lazy method of stopping the cup hitting Dave twice, causing double BadFade animations
    #endregion

    #region OnCollision
    private void OnTriggerEnter(Collider o)
    {
        if (o.transform.tag == "Item" && !just_to_be_safe)
        {
            fm.error = true;
            just_to_be_safe = true;
            StartCoroutine(KilledDave());
        }
    }
    #endregion

    #region End day logic
    public IEnumerator KilledDave()
    {
        throwable.GetComponent<Throwable>().can_insult = false;
        fade.GetComponent<Animator>().SetTrigger("BadFade");
        StartCoroutine(fs.KillEndSubtitles());
        yield return new WaitForSeconds(3.0f);

        enabled = false;
    }
    #endregion
}
