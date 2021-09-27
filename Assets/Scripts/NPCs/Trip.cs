using System.Collections;
using UnityEngine;

public class Trip : MonoBehaviour
{
    #region Variables
    private Rigidbody                           rb;
    private CapsuleCollider                     c;

    [Header("Turn parameters")]
    [SerializeField] private bool               can_turn                = false;
    [SerializeField] private bool               has_tripped             = false;
    private float                               rot_speed               = 0.0f;
    [SerializeField] private float              normal_rot              = 0.0f;
    [SerializeField] private float              fall_rot                = 0.0f;
    [SerializeField] private Transform          player;

    [Header("Trip parameters")]
    [SerializeField] private Vector3            force_direction         = Vector3.zero;
    [SerializeField] private Vector3            torque                  = Vector3.zero;
    private Vector3                             fall_spot               = Vector3.zero;
    [SerializeField] private float              stand_up_timer          = 0.0f;
    [SerializeField] private float              resetRB_timer           = 0.0f;
    [SerializeField] private float              default_radius          = 0.0f;
    [SerializeField] private float              fall_radius             = 0.0f;
    [SerializeField] private string             tag_string              = "Untagged";
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        rb  = GetComponent<Rigidbody>();
        c   = GetComponent<CapsuleCollider>();

        rot_speed   = normal_rot;
        c.radius    = default_radius;
    }

    private void Update()
    {
        FacePlayer();
    }

    private void OnTriggerEnter(Collider o)
    {
        if ((o.transform.tag == tag_string) && !has_tripped)
            TripOver();

        if (o.transform.tag == "Player")
            Audio.Instance.Play3DLocal("Slap", player.gameObject);
    }

    #endregion

    #region Custom Functions
    private void FacePlayer()
    {
        if (can_turn)
        {
            transform.rotation = Quaternion.Slerp
            (
            transform.rotation,
            Quaternion.LookRotation(player.position - transform.position),
            rot_speed * Time.deltaTime
            );
        }
    }

    private void TripOver()
    {
        c.isTrigger     = false;
        c.radius        = fall_radius;
        rb.isKinematic  = false;
        can_turn        = false;
        has_tripped     = true;

        rb.AddForce(force_direction);
        rb.AddTorque(torque);

        fall_spot = transform.position;

        RandomVoice();
        StartCoroutine(WaitToSitUp());
    }

    private void RandomVoice()
    {
        int num = Random.Range(1, 4);

        Audio.Instance.Play3DAway("Oof" + num.ToString(), gameObject);
    }

    private IEnumerator WaitToSitUp()
    {
        yield return new WaitForSeconds(stand_up_timer);
        StartCoroutine(ResetRB());
    }

    private IEnumerator ResetRB()
    {
        can_turn            = true;
        rb.isKinematic      = true;
        rb.velocity         = new Vector3(0f, 0f, 0f);
        rb.angularVelocity  = new Vector3(0f, 0f, 0f);
        rot_speed           = fall_rot;

        Vector3 vel = Vector3.zero;

        Vector3 reset_spot = new Vector3
            (
            transform.position.x,
            fall_spot.y,
            transform.position.z
            );

        transform.position = reset_spot;

        yield return new WaitForSeconds(resetRB_timer);

        rot_speed           = normal_rot;
        has_tripped         = false;
        c.isTrigger         = true;
        c.radius            = default_radius;
    }
    #endregion
}
