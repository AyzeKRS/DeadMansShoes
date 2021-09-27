using UnityEngine;
using System.Collections;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float timer = 0.0f;

    private void Start()
    {
        StartCoroutine(TimeToDestroy());
    }

    private IEnumerator TimeToDestroy()
    {
        GetComponent<ParticleSystem>().Emit(1);

        yield return new WaitForSeconds(timer);

        Destroy(gameObject);
    }
}
