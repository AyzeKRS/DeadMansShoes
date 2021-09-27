using UnityEngine;

public class BudgetIndi : MonoBehaviour
{
    #region Variables
    #region Script movement variables
    [SerializeField] private float                  speed               = 5.0f;
    private float                                   accelerate          = 12.0f;
    private float                                   gravity             = -9.81f;
    #endregion

    #region Movement turning + acceleration
    private float                                   tS                  = 0.0f;
    private float                                   turn_speed          = 0.0f;
    private float                                   turn_speed_min      = 10f;
    private float                                   turn_speed_max      = 20f;

    private Vector3                                 dir                 = Vector3.zero;
    private Vector3                                 intent              = Vector3.zero;
    private Vector3                                 velocity            = Vector3.zero;
    private Vector3                                 velocityXZ          = Vector3.zero;

    private Vector3                                 camF                = Vector3.zero;
    private Vector3                                 camR                = Vector3.zero;
    #endregion

    [HideInInspector] public CharacterController    cc;
    private InputManager                            im;
    [SerializeField] private Transform              cam;
    [SerializeField] private Transform              reset_point;
    [SerializeField] private SecondMinigame         sm;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        im = GetComponent<InputManager>();

        cc.enabled = false;
    }

    private void Update()
    {
        if (cc.enabled && Input.GetButtonDown("Interact"))
            StartCoroutine(sm.Exit());

        GetCamera();
        GetGravity();
    }

    private void FixedUpdate()
    {
        if (cc.enabled)
            Movement();
        cc.Move(velocity * Time.deltaTime);
    }
    #endregion

    #region Custom Functinos
    void GetCamera()
    {
        camF = cam.forward;
        camR = cam.right;

        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;
    }

    void GetGravity()
    {
        if (cc.isGrounded)
            velocity.y = -2.0f;
        else
            velocity.y += (1.5f * gravity) * Time.deltaTime;

        velocity.y = Mathf.Clamp(velocity.y, -50, 50);
    }

    private void Movement()
    {
        intent          = camF * im.MovementDirectionInput().x + camR * im.MovementDirectionInput().y;
        tS              = velocity.magnitude / speed;
        turn_speed      = Mathf.Lerp(turn_speed_max, turn_speed_min, tS);


        if (im.MovementDirectionInput().magnitude > 0)
        {   
            Quaternion rot      = Quaternion.LookRotation(intent);
            transform.rotation  = Quaternion.Lerp(transform.rotation, rot, turn_speed * Time.deltaTime);
        }

        dir             = transform.forward;
        velocityXZ      = velocity;
        velocityXZ.y    = 0;
        velocityXZ      = Vector3.Lerp(velocity, dir * im.MovementDirectionInput().magnitude * speed, accelerate * Time.deltaTime);

        velocity = new Vector3(velocityXZ.x, velocity.y, velocityXZ.z);
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.transform.tag == "Bullet")
        {
            sm.Hit();
            transform.position = reset_point.position;
            Audio.Instance.Play2DSound("Error");
        }

        if (o.transform.tag == "Button")
            sm.Win();
    }
    #endregion
}
