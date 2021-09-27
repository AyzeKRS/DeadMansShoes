using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyNumber : MonoBehaviour
{
    #region Variables
    public string key = "";
    public TMPro.TMP_Text text;
    #endregion

    #region Custom Functions
    public void SendKey()
    {
        Audio.Instance.Play3DLocal("Button", gameObject);
        transform.GetComponentInParent<Keypad>().EnterCode(key);
        text.color = Color.black;
        StartCoroutine(reset());
    }

    private IEnumerator reset()
    {
        yield return new WaitForSeconds(0.1f);
        text.color = Color.white;
    }
    #endregion
}
