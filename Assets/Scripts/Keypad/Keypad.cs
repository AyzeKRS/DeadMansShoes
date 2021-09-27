using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    #region Variables
    [SerializeField] private string         code                    = "";
    [SerializeField] private string         entered_string          = "";
    public bool                             open                    = false;

    #region Interactables
    [SerializeField] private GameObject     door;                           // Door or similar object with open function
    #endregion

    #region Timer
    [SerializeField] private float          reset_timer  = 1.0f;
    private float                           timer                   = 0.0f;
    private bool                            cool_down               = false;
    #endregion

    #region Text
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private TMPro.TMP_Text code_text;
    #endregion

    #endregion

    #region BuiltIn Functions
    public void Start()
    {
        Init();
    }

    public void Update()
    {
        if (cool_down) CoolDown();
    }
    #endregion

    #region Custom Functions

    #region Start
    public void Init()
    {
        for (int i = 0; i < 4; i++)
            code += Random.Range(1, 10);

        code_text.text = code.ToString();
    }
    #endregion

    #region Extras
    public void EnterCode(string key)
    {
        if (!cool_down)
        {
            if (!open)
            {
                entered_string += key;
                UpdateText();
            }
            if (entered_string.Length == 4)
            {
                if (entered_string == code)
                {
                    open = true;
                    text.color = Color.green;
                    door.GetComponent<Animator>().SetTrigger("Open");
                    Audio.Instance.Play2DSound("Complete");
                    Audio.Instance.Play3DLocal("Door", gameObject);
                }
                else
                {
                    entered_string = "";
                    cool_down = true;
                    Audio.Instance.Play2DSound("Error");
                }
            }
        }
    }

    private void UpdateText()
    {
        text.text = entered_string;
    }

    private void CoolDown()
    {
        timer += Time.deltaTime;
        if (timer >= reset_timer)
        {
            cool_down = false;
            timer = 0.0f;
            text.color = Color.white;
            UpdateText();
        }
        else text.color = Color.red;
    }
    #endregion
    #endregion
}
