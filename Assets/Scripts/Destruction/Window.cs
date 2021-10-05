using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField] private Material[] mat;

    private void Start()
    {
        if (Manager.Instance.killed[0])
            GetComponent<Renderer>().material = mat[0];

        else
            GetComponent<Renderer>().material = mat[1];
    }

}
