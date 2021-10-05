using UnityEngine;
using TMPro;

public class Pause : MonoBehaviour
{
    private bool                            pressed             = false;

    private float                           count_timer         = 0.0f;
    [SerializeField] private float          timer               = 0.0f;
    [SerializeField] private TMP_Text       text;

    private void Update()
    {
        Timer();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pressed)
                pressed = true;

            else
                Manager.Instance.QuitGame();
        }
    }

    private void Timer()
    {
        if (pressed)
        {
            text.text = "Press ESC again to quit";
            count_timer += Time.deltaTime;

            if (count_timer >= timer)
            {
                pressed     = false;
                count_timer = 0.0f;
                text.text   = "";
            }
        }
    }
}
