using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KillTim : MonoBehaviour
{
    #region Variables
    [SerializeField] private ThirdSubtitles     ts;
    [SerializeField] private Image              fade;
    private bool                                just_to_be_safe; // Lazy method of stopping the gun hitting Tim twice, causing double BadFade animations
    #endregion

    #region OnCollision
    private void OnTriggerEnter(Collider o)
    {
        if (o.transform.tag == "Item" && !just_to_be_safe)
        {
            just_to_be_safe = true;
            StartCoroutine(KilledTim());
        }
            
    }
    #endregion

    #region End day logic
    public IEnumerator KilledTim()
    {
        fade.GetComponent<Animator>().SetTrigger("BadFade");
        StartCoroutine(ts.KillEndSubtitles());
        yield return new WaitForSeconds(3.0f);

        enabled = false;
    }
    #endregion
}
