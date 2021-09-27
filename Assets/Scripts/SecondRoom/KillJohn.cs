using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KillJohn : MonoBehaviour
{
    #region Variables
    [SerializeField] private SecondSubtitles        ss;
    [SerializeField] private Image                  fade;
    [SerializeField] private Pistol                 pistol;
    #endregion

    #region OnCollision
    private void OnTriggerEnter(Collider o)
    {
        if (o.transform.tag == "Bullet")
            StartCoroutine(KilledDave());
    }
    #endregion

    #region End day logic
    public IEnumerator KilledDave()
    {
        pistol.enabled = false;
        fade.GetComponent<Animator>().SetTrigger("BadFade");
        StartCoroutine(ss.KillEndSubtitles());
        yield return new WaitForSeconds(3.0f);

        enabled = false;
    }
    #endregion
}
