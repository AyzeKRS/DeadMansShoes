using UnityEngine;

public class DropShelf : MonoBehaviour
{
    private bool                                chain1      = false;
    private bool                                chain2      = false;
    private Rigidbody                           rb;
    [SerializeField] private Collider           col;
    [SerializeField] private Pistol             pistol;
    [SerializeField] private ThirdSubtitles     ts;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Drop(bool first)
    {
        if (first)
            chain1 = true;
        else
            chain2 = true;

        if (chain1 && chain2)
        {
            rb.isKinematic  = false;
            col.enabled     = false;
            pistol.enabled  = false;
            StartCoroutine(ts.PistolSubtitles());
        }
    }
}
