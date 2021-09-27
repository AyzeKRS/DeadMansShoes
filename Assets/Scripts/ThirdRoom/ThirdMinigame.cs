using UnityEngine;
using UnityEngine.UI;

public class ThirdMinigame : MonoBehaviour
{
    public int[]                                    code        = { 0, 0, 0, 0, 0, 0 };
    [SerializeField] private GameObject[]           component   = { null, null, null, null, null, null };
    [SerializeField] private ThirdSubtitles         ts;
    [SerializeField] private Image                  fade;

    private void Start()
    {
        GenerateCode();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CheckCode();
    }

    private void GenerateCode()
    {
        for (int i = 0; i < code.Length; i++)
            code[i] = Random.Range(0, 6);

        component[4].SetActive(false);
    }

    public void CheckCode()
    {
        for (int i = 0; i < code.Length; i++)
        {
            if (code[i] != component[i].GetComponent<ServerInteract>().num)
                return;
        }

        for (int i = 0; i < code.Length; i++)
            component[i].tag = "Untagged";

        fade.GetComponent<Animator>().SetTrigger("GoodFade");
        StartCoroutine(ts.MinigameEndSubtitles());
    }
}
