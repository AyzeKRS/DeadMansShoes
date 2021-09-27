using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    #region Variables

    #region Sensitivity
    InputManager im;

    public bool                         CanLook { get; set; }   = true;

    [Header("Sensitivity")]
    [SerializeField] private bool       lock_XY                         = false;
    [SerializeField] private float      sens                            = 0.0f;
    [SerializeField] private Vector2    sens_sep                        = Vector2.zero;
    [SerializeField] private Vector2    smoothing                       = Vector2.zero;
    [SerializeField] private Vector2    degree_clamp                    = new Vector2(360, 180);
    #endregion

    #region Player
    [Header("Player")]
    [SerializeField] private GameObject player_body = null;
    private Vector2                     intended_direction              = Vector2.zero;
    private Vector2                     intended_player_direction       = Vector2.zero;
    #endregion

    #region Mouse Outputs
    private Vector2                     delta                           = Vector2.zero;
    private Vector2                     mouse_smoothing                 = Vector2.zero;
    private Vector2                     mouse_position                  = Vector2.zero;
    private Vector2                     mouse_position_hold             = Vector2.zero;
    #endregion

    #endregion

    /************************************************************************************************************************************************************************************************/

    #region BuiltIn Functions

    private void Start()
    {
        im = GetComponent<InputManager>();
        GetPlayerDirections();
    }

    private void Update()
    {
        GetInput();

        SmoothInput();
        ClampInput();

        if (CanLook)
            RotateCamera();
        else
            HoldInput();

    }
    #endregion

    /************************************************************************************************************************************************************************************************/

    #region Custom Functions

    #region Start()

    private void GetPlayerDirections()
    {
        intended_direction = transform.localRotation.eulerAngles;

        if (player_body)
            intended_player_direction = player_body.transform.localRotation.eulerAngles;
    }

    #endregion

    #region Update()

    private void GetInput()
    {
        delta = im.MouseAxisRaw();
    }

    private void SmoothInput()
    {
        if (lock_XY)
            delta = Vector2.Scale(delta, new Vector2(sens * smoothing.x, sens * smoothing.y));
        else
            delta = Vector2.Scale(delta, new Vector2(sens_sep.x * smoothing.x, sens_sep.y * smoothing.y));

        mouse_smoothing.x = Mathf.Lerp(mouse_smoothing.x, delta.x, 1f / smoothing.x);
        mouse_smoothing.y = Mathf.Lerp(mouse_smoothing.y, delta.y, 1f / smoothing.y);

        mouse_position += mouse_smoothing;
    }

    private void ClampInput()
    {
        if (degree_clamp.x < 360)
            mouse_position.x = Mathf.Clamp(mouse_position.x, -degree_clamp.x * 0.5f, degree_clamp.x * 0.5f);

        if (degree_clamp.y < 360)
            mouse_position.y = Mathf.Clamp(mouse_position.y, -degree_clamp.y * 0.5f, degree_clamp.y * 0.5f);
    }

    private void RotateCamera()
    {
        Quaternion camera_orientation = Quaternion.Euler(intended_direction);
        Quaternion player_orientation = Quaternion.Euler(intended_player_direction);

        transform.localRotation = Quaternion.AngleAxis(-mouse_position.y, camera_orientation * Vector3.right) * camera_orientation;

        if (player_body)
        {
            Quaternion rot_y = Quaternion.AngleAxis(mouse_position.x, Vector3.up);
            player_body.transform.localRotation = rot_y * player_orientation;
        }
        else
        {
            Quaternion rot_y = Quaternion.AngleAxis(mouse_position.x, transform.InverseTransformDirection(Vector3.up));
            transform.localRotation *= rot_y;
        }

        mouse_position_hold = mouse_position;
    }

    private void HoldInput()
    {
        mouse_position = mouse_position_hold;
    }
    #endregion

    #endregion

    /************************************************************************************************************************************************************************************************/
}
