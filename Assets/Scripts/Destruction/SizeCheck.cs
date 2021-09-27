using UnityEngine;

enum RemoveState
{
    CAN_DIVIDE,
    CANNOT_DIVIDE,
    FADING_OUT
};

public class SizeCheck : MonoBehaviour
{
    [SerializeField] private float size_cubed       = 0.0f;
    [SerializeField] private float stop_float       = 0.25f;
    [SerializeField] private float destroy_float    = 0.05f;
    [SerializeField] private float timer            = 10.0f;
    private RemoveState rs                          = RemoveState.CAN_DIVIDE;

    private void Start()
    {
        size_cubed =    GetComponent<Collider>().bounds.size.x *
                        GetComponent<Collider>().bounds.size.y *
                        GetComponent<Collider>().bounds.size.z;

        if (destroy_float < size_cubed && size_cubed <= stop_float)
        {
            Destroy(GetComponent<MeshDestruction>());
            rs = RemoveState.CANNOT_DIVIDE;
        }
        else if (size_cubed <= destroy_float) rs = RemoveState.FADING_OUT;
    }

    private void Update()
    {
        switch (rs)
        {
            case RemoveState.CANNOT_DIVIDE:
                RemoveTimer();
                break;

            case RemoveState.FADING_OUT:
                FadeOut();
                break;

            default:
                break;
        }
    }

    private void FadeOut()
    {
        gameObject.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
        if (0 <= gameObject.transform.localScale.x && gameObject.transform.localScale.x <= 0.01 ||
            0 <= gameObject.transform.localScale.y && gameObject.transform.localScale.y <= 0.01 ||
            0 <= gameObject.transform.localScale.z && gameObject.transform.localScale.z <= 0.01) Destroy(gameObject);
    }

    private void RemoveTimer()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) rs = RemoveState.FADING_OUT;
    }
}