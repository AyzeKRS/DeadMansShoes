using UnityEngine;

public class LavaBlock : MonoBehaviour
{
    [Header("Move Parameters")]
    [SerializeField] private bool   x_or_z              = false;
    [SerializeField] private float  speed               = 0.0f;
    [SerializeField] private float  amount              = 0.0f;

    private float                   move_x              = 0.0f;
    private float                   move_z              = 0.0f;
    private float                   default_x_pos       = 0.0f;
    private float                   default_z_pos       = 0.0f;
    private float                   timer               = 0.0f;

    private void Start()
    {
        default_x_pos = transform.localPosition.x;
        default_z_pos = transform.localPosition.z;
    }

    private void Update()
    {
        timer   += speed * Time.deltaTime;
        move_x  = default_x_pos + Mathf.Sin(timer) * amount;
        move_z  = default_z_pos + Mathf.Sin(timer) * amount;

        if (x_or_z)
            transform.localPosition = new Vector3
                (
                move_x,
                transform.localPosition.y,
                transform.localPosition.z
                );

        else
            transform.localPosition = new Vector3
                (
                transform.localPosition.x,
                transform.localPosition.y,
                move_z
                );
    }
}
