using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Variables
    private float                           life_time               = 0.0f;
    [SerializeField] private float          range_in_time           = 0.0f;
    [SerializeField] private float          velocity                = 0.0f;

    [SerializeField] private GameObject     sparks;
    #endregion

    #region BuiltIn Functions
    private void Update()
    {
        Velocity();
        CheckLifetime();
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.transform.tag == "Destructable")       { }
        else if (c.transform.tag == "Enemy")         { }
        else
            Instantiate(sparks.transform, transform.position, Quaternion.Inverse(Quaternion.identity));

        Destroy(gameObject);
    }
    #endregion

    #region Custom Functions
    private void Velocity()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * velocity;
    }

    private void CheckLifetime()
    {
        life_time += Time.deltaTime;
        if (life_time >= range_in_time)
            Destroy(gameObject);
    }
    #endregion
}
