using UnityEngine;
using System.Collections;

public class ItemHolder : MonoBehaviour
{
    #region Variables
    [HideInInspector] public bool               item_held => transform.childCount >= 1;
    [HideInInspector] public bool               being_picked_up     = false;
    [HideInInspector] public GameObject         item;

    [SerializeField] private Camera             cam;
    [SerializeField] private Transform          throw_spot;

    [Header("Throw Parameters")]
    [SerializeField] private float              throw_force         = 0.0f;

    [SerializeField] private float              throw_pos           = 0.0f;
    [SerializeField] private float              throw_rot           = 0.0f;
    public float                                throw_timer         = 0.0f;
    private float                               timer               = 0.0f;
    #endregion

    #region BuiltIn Functions
    private void Update()
    {
        if (Input.GetButtonDown("Throw"))
        {
            ThrowItem();
        }
    }
    #endregion

    #region Custom Functions
    private void ThrowItem()
    {
        if (item_held && !being_picked_up)
            StartCoroutine(Throw());
    }

    private IEnumerator Throw()
    {
        item.transform.parent = throw_spot.transform;
        while (timer < throw_timer)
        {
            timer += Time.deltaTime;
            Vector3 vel = Vector3.zero;

            item.transform.localPosition = Vector3.SmoothDamp
                (
                item.transform.localPosition,
                Vector3.zero,
                ref vel,
                1 / throw_pos * Time.deltaTime
                );

            item.transform.localRotation = Quaternion.Slerp
                (
                item.transform.localRotation,
                Quaternion.Euler(0, 0, 0),
                throw_rot * Time.deltaTime
                );
            yield return new WaitForEndOfFrame();
        }

        timer = 0.0f;

        UnChildAndReset();

        item.GetComponent<Rigidbody>().AddForce(cam.transform.forward * throw_force);

        yield return new WaitForSeconds(0.01f);

        item.GetComponent<Collider>().enabled = true;
        item = null;
    }

    private void UnChildAndReset()
    {
        item.transform.parent = null;

        item.transform.GetChild(0).GetComponent<Collider>().enabled = true;

        item.GetComponent<Rigidbody>().isKinematic      = false;
        item.GetComponent<Rigidbody>().useGravity       = true;
    }
    #endregion
}
