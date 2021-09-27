using UnityEngine;

public class LavaMovement : MonoBehaviour
{
    [Header("Bob Parameters")]
    [SerializeField] private float  bob_speed       = 0.0f;
    [SerializeField] private float  bob_amount      = 0.0f;

    private float                   bob_y           = 0.0f;
    private float                   bob_z           = 0.0f;
    private float                   default_y_pos   = 0.0f;
    private float                   default_z_pos   = 0.0f;
    private float                   timer           = 0.0f;

    private void Start()
    {
        default_y_pos = transform.localPosition.y;
        default_z_pos = transform.localPosition.z;
    }

    private void Update()
    {
        timer += bob_speed * Time.deltaTime;
        bob_y = default_y_pos + Mathf.Sin(timer) * bob_amount;
        bob_z = default_z_pos + Mathf.Cos(timer) * bob_amount * 5.0f;

        transform.localPosition = new Vector3
            (
            transform.localPosition.x,
            bob_y,
            bob_z
            );
    }
}
