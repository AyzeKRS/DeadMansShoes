using UnityEngine;

public class CameraBob : MonoBehaviour
{
    #region Variables
    public bool                         CanBob { get; set; }    = true;

    private Movement                    m;

    [Header("Bob Parameters")]
    [SerializeField] private float      walk_bob_speed          = 0.0f;
    [SerializeField] private float      walk_bob_amount         = 0.0f;
    [SerializeField] private float      sprint_bob_speed        = 0.0f;
    [SerializeField] private float      sprint_bob_amount       = 0.0f;
    [SerializeField] private float      crouch_bob_speed        = 0.0f;
    [SerializeField] private float      crouch_bob_amount       = 0.0f;

    [HideInInspector] public float      head_bob                = 0.0f;
    private float                       default_y_pos           = 0.0f;
    private float                       timer                   = 0.0f;
    private int                         state                   = 0;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        default_y_pos = transform.localPosition.y;
        m = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        if (CanBob)
            HeadBob();
    }
    #endregion

    #region Custom Functions
    private void HeadBob()
    {
        if (!m.cc.isGrounded)
            return;

        state = m.GetState(); 

        if (Mathf.Abs(m.move_dir.x) > 0.1f || Mathf.Abs(m.move_dir.z) > 0.1f)
        {
            timer   += GetSpeed() * Time.deltaTime;
            head_bob = default_y_pos + Mathf.Sin(timer) * GetAmount();

            transform.localPosition = new Vector3
                (
                transform.localPosition.x,
                head_bob,
                transform.localPosition.z
                );
        }
        else
        {
            head_bob = Mathf.Lerp(transform.localPosition.y, default_y_pos, 0.01f);

            transform.localPosition = new Vector3
                (
                transform.localPosition.x,
                head_bob,
                transform.localPosition.z
                );

            timer = 0.0f;
        }
    }

    private float GetSpeed()
    {
        switch (state)
        {
            case 2: // Crouch
                return crouch_bob_speed;

            case 1: // Sprint
                return sprint_bob_speed;

            case 0: // Walk/Not defined
            default:
                return walk_bob_speed;
        }
    }

    private float GetAmount()
    {
        switch(state)
        {
            case 2: // Crouch
                return crouch_bob_amount;

            case 1: // Sprint
                return sprint_bob_amount;

            case 0: // Walk/Not defined
            default:
                return walk_bob_amount;
        }
    }
    #endregion
}
