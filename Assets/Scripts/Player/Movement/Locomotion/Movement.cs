using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables
    private InputManager                            im;
    [HideInInspector] public CharacterController    cc;
    [SerializeField] private Camera                 cam;

    public bool                                     CanMove { get; set; } = true;
    public bool                                     CanJump             => im.Jump && cc.isGrounded;
    public bool                                     CanCrouch           => im.Crouch && !in_crouch_anim && cc.isGrounded;
    private bool                                    IsSprinting         => can_sprint && im.Sprint;

    private bool                                    can_sprint          = true;
    private bool                                    can_jump            = true;
    private bool                                    can_crouch          = true;

    [Header("Speed Parameters")]
    [SerializeField] private float                  walk_speed          = 0.0f;
    [SerializeField] private float                  sprint_speed        = 0.0f;
    [SerializeField] private float                  crouch_speed        = 0.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float                  jump_force          = 0.0f;
    [SerializeField] private float                  gravity             = 0.0f;

    [Header("Crouch Parameters")]
    [SerializeField] private float                  crouch_height       = 0.0f;
    [SerializeField] private float                  standing_height     = 0.0f;
    [SerializeField] private float                  time_to_crouch      = 0.0f;
    [SerializeField] private Vector3                crouch_centre       = Vector3.zero;
    [SerializeField] private Vector3                standing_centre     = Vector3.zero;
    private bool                                    is_crouching        = false;
    private bool                                    in_crouch_anim      = false;

    private Vector2                                 curr_in             = Vector2.zero;
    [HideInInspector] public Vector3                move_dir            = Vector3.zero;

    private float                                   footstep_timer      = 0.0f;
    #endregion

    #region BuiltIn Functions
    private void Awake()
    {
        im = GetComponent<InputManager>();
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (CanMove)
        {
            GetInput();

            if (can_jump)
                GetJump();

            if (can_crouch)
                GetCrouch();
        }
        else
        {
            move_dir.x = Mathf.Lerp(move_dir.x, 0.0f, 10.0f * Time.deltaTime);
            move_dir.z = Mathf.Lerp(move_dir.z, 0.0f, 10.0f * Time.deltaTime);
        }

        Locomotion();
    }
    #endregion

    #region Custom Functions
    private void GetInput()
    {
        curr_in = new Vector2
            (
            im.MovementDirectionInput().x * GetSpeed(),
            im.MovementDirectionInput().y * GetSpeed()
            );

        float move_dir_y = move_dir.y;

        move_dir =
            (transform.TransformDirection(Vector3.forward) * curr_in.x) +
            (transform.TransformDirection(Vector3.right) * curr_in.y);

        if (cc.isGrounded)
        {
            if (IsSprinting)
                Audio.Instance.step_timer = 0.3f;
            else
                Audio.Instance.step_timer = 0.6f;

            Audio.Instance.PlayFootstep(gameObject, move_dir.magnitude, ref footstep_timer);
        }

        move_dir.y = move_dir_y;
    }

    public float GetSpeed()
    {
        return is_crouching ? crouch_speed : IsSprinting ? sprint_speed : walk_speed;
    }

    public int GetState()
    {
        return is_crouching ? 2 : IsSprinting ? 1 : 0;
    }

    private void GetJump()
    {
        if (CanJump)
            move_dir.y = jump_force;
    }

    private void GetCrouch()
    {
        if (CanCrouch)
            StartCoroutine(CrouchStand());
    }

    private void Locomotion()
    {
        if (!cc.isGrounded)
            move_dir.y -= gravity * Time.deltaTime;

        cc.Move(move_dir * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        if (is_crouching && Physics.Raycast(cam.transform.position, Vector3.up, 1.0f))
            yield break;

        in_crouch_anim = true;

        float   timer           = 0.0f;
        float   target_height   = is_crouching ? standing_height : crouch_height;
        float   current_height  = cc.height;
        Vector3 target_centre   = is_crouching ? standing_centre : crouch_centre;
        Vector3 current_centre  = cc.center;

        while (timer < time_to_crouch)
        {
            cc.height   = Mathf.Lerp(current_height, target_height, timer / time_to_crouch);
            cc.center   = Vector3.Lerp(current_centre, target_centre, timer / time_to_crouch);
            timer       += Time.deltaTime;
            yield return null;
        }

        cc.height       = target_height;
        cc.center       = target_centre;
        is_crouching    = !is_crouching;
        in_crouch_anim  = false;
    }

    #endregion
}
