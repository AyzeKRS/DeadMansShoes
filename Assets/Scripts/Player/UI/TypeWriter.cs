using System.Collections;
using UnityEngine;
using TMPro;

public class TypeWriter : MonoBehaviour
{
    public void RunText(string text_to_type, TMP_Text text, float speed)
    {
        StartCoroutine(TypeWrite(text_to_type, text, speed));
    }

    private IEnumerator TypeWrite(string text_to_type, TMP_Text text, float speed)
    {
        float timer     = 0.0f;
        int index       = 0;
        int pre_index   = 0;

        text.text = string.Empty;

        while (index < text_to_type.Length)
        {
            timer += Time.deltaTime * speed;
            index = Mathf.FloorToInt(timer);
            index = Mathf.Clamp(index, 0, text_to_type.Length);

            text.text
                = text_to_type.Substring(0, index);

            if (index == pre_index)
            {
                pre_index = index + 2;
                Audio.Instance.Play2DSound("TypeWriter");
            }

            yield return null;
        }

        text.text = text_to_type;
    }
}
