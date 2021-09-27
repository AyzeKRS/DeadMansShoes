using UnityEngine;

public class RandomSpin : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Update()
    {
        Vector3 mouseWorldPosition
            = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);

        float angle
            = Mathf.Atan2
            (
                transform.position.y - mouseWorldPosition.y,
                transform.position.x - mouseWorldPosition.x
            )
            * Mathf.Rad2Deg;

        transform.rotation
            = Quaternion.Euler(new Vector3(0.0f, -angle - 90.0f, 0.0f));
    }
}
