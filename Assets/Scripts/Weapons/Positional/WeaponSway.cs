using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    #region Variables
    [Header("Sway parameters")]
    [SerializeField] private float      sway_amount             = 0.0f;
    [SerializeField] private float      sway_amount_max         = 0.0f;
    [SerializeField] private float      sway_smoothing          = 0.0f;
    
    [Header("Tilt parameters")]
    [SerializeField] private float      rotation_smoothing      = 0.0f;
    [SerializeField] private float      tilting_angle           = 0.0f;

    private Vector2                     factor                  = Vector2.zero;
    private Vector3                     final                   = Vector3.zero;

    private Vector2                     tilt_around             = Vector2.zero;
    private Quaternion                  target;
    #endregion

    #region BuiltIn Functions
    private void Update()
    {
        SwayWeapon();
        TiltWeapon();
    }
    #endregion

    #region Custom Functions
    private void SwayWeapon()
    {
        factor.x = -Input.GetAxis("Mouse X") * sway_amount;
        factor.y = -Input.GetAxis("Mouse Y") * sway_amount;

        factor.x = Mathf.Clamp(factor.x, -sway_amount_max, sway_amount_max);
        factor.y = Mathf.Clamp(factor.y, -sway_amount_max, sway_amount_max);

        final = new Vector3
            (
            transform.localPosition.x + factor.x,
            transform.localPosition.y + factor.y,
            transform.localPosition.z
            );

        transform.localPosition = Vector3.Lerp(transform.localPosition, final, Time.deltaTime * sway_smoothing);
    }

    private void TiltWeapon()
    {
        tilt_around.x = Input.GetAxis("Mouse X") * tilting_angle; // Z
        tilt_around.y = Input.GetAxis("Mouse Y") * tilting_angle; // X

        target = Quaternion.Euler(tilt_around.y, 0, tilt_around.x);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * rotation_smoothing);
    }
    #endregion
}
