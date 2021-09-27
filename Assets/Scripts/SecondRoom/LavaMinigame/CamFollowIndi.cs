using UnityEngine;

public class CamFollowIndi : MonoBehaviour
{
    [SerializeField] private GameObject     cam_follow_object;
    [SerializeField] private float          smooth_speed            = 0.0f;
    private Vector3                         offset                  = Vector3.zero;
    [SerializeField] private Vector3        offset2                 = Vector3.zero;

    private void FixedUpdate()
    {
        Transform target            = cam_follow_object.transform;
        Vector3 desired_position    = target.position + offset;
        Vector3 smoothed_position   = Vector3.Lerp(transform.position, desired_position, smooth_speed);
        transform.position          = new Vector3
            (
            smoothed_position.x + offset2.x,
            smoothed_position.y + offset2.y,
            smoothed_position.z + offset2.z
            );
    }
}
