using UnityEngine;

public class BreakChains : MonoBehaviour
{
    [SerializeField] private bool           first_chain     = false;
    [SerializeField] private DropShelf      ds;
    [SerializeField] private Rigidbody      rb;
    private Outline                         ol;
    private Collider                        co;

    private void Start()
    {
        ol = GetComponent<Outline>();
        co = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider o)
    {
        if (o.transform.tag == "Bullet")
        {
            ol.enabled          = false;
            rb.isKinematic      = false;
            co.enabled          = false;
            ds.Drop(first_chain);

            Audio.Instance.Play3DAway("Chains", gameObject);
        }
    }
}
