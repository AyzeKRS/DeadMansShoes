using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    #region Variables
    private Movement m;
    [Header("Bob parameters")]
    private float                           bob_increment       = 0.0f;
    private float                           lock_axes           = 0.0f;

    [SerializeField] private bool           reverse_arch        = false;

    [SerializeField] private float          bob_speed           = 0.0f;
    [SerializeField] private Vector2        bob_amount          = Vector2.zero;

    private Vector2                         bob_graph           = Vector2.zero;
    private Vector2                         bob_final           = Vector2.zero;
    private Vector3                         reset               = Vector3.zero;
    #endregion

    #region BuiltIn Functions
    private void Start()
    {
        reset = transform.localPosition;
        m = transform.parent.GetComponentInParent<Movement>();
    }

    private void FixedUpdate()
    {
        BobWeapon();
    }
    #endregion

    #region Custom Functions
    private void BobWeapon()
    {
        bob_graph = Vector2.zero;
        
        if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0 && Mathf.Abs(Input.GetAxis("Vertical")) == 0)
            bob_increment = Mathf.Lerp(bob_increment, 0.0f, 0.5f);

        else
        {
            bob_graph.x = Mathf.Sin(bob_increment);
            bob_graph.y = Mathf.Cos(bob_increment * 2) * (reverse_arch ? -1 : 1);

            if (m.cc.isGrounded)
                bob_increment += bob_speed;

            if (bob_increment > Mathf.PI * 2)
                bob_increment -= (Mathf.PI * 2);
        }

        if ((bob_graph.x != 0 || bob_graph.y != 0))
        {
            lock_axes = Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical"));
            lock_axes = Mathf.Clamp(lock_axes, 0.0f, 1.0f);

            bob_final.x = bob_graph.x * bob_amount.x * lock_axes;
            bob_final.y = bob_graph.y * bob_amount.y * lock_axes;

            transform.localPosition = new Vector3
                (
                reset.x + bob_final.x,
                reset.y + bob_final.y,
                reset.z
                );
        }
        else
            transform.localPosition = reset;
    }

    #endregion
}
