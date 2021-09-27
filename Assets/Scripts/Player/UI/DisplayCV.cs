using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayCV : MonoBehaviour
{
    #region Variables
    private Animator                        anim;
    private Image                           im;

    private int                             clicks              = 0;
    private float                           timer               = 0;
    private float                           text_y_normal       = -200.0f;
    private float                           text_y_temp         = -450.0f;

    [SerializeField] private GameObject     manager;
    [SerializeField] private TMP_Text       text;
    [SerializeField] private Image          dim;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        anim            = GetComponent<Animator>();
        anim.enabled    = false;
        im              = GetComponent<Image>();
        im.enabled      = false;
    }
    
    private void Update()
    {
        if (im.enabled)
        {
            if (timer <= 1.0f)
                timer += Time.deltaTime;
            else
            {
                if (Input.GetButtonDown("Interact") && clicks < 2)
                {
                    switch (clicks)
                    {
                        case 0:
                            FlipCV();
                            clicks++;
                            break;

                        case 1:
                            StartCoroutine(RemoveCV());
                            clicks++;
                            timer = 0;
                            break;
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (im.enabled && timer >= 1.0f) // I believe the update function in interact is overriding the text, making it disappear. This fixes it
        {
            text.transform.localPosition = new Vector3
                (
                text.transform.localPosition.x,
                text_y_temp,
                text.transform.localPosition.z
                );

            text.text = "Press E to continue";
        }
        else
            text.transform.localPosition = new Vector3
                (
                text.transform.localPosition.x,
                text_y_normal,
                text.transform.localPosition.z
                );
    }
    #endregion

    #region Custom Functions
    public void ShowCV()
    {
        im.enabled      = true;
        anim.enabled    = true;
        dim.GetComponent<FadeScreen>().Dim();
    }

    public void FlipCV()
    {
        anim.SetTrigger("Reverse");
    }

    public IEnumerator RemoveCV()
    {
        anim.SetTrigger("Remove");

        yield return new WaitForSeconds(1.0f);

        im.enabled = false;

        dim.GetComponent<FadeScreen>().Show();

        StartCoroutine(manager.GetComponent<PSubtitles>().ReceptionistSubtitles());
    }
    #endregion
}
