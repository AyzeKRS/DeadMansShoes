using UnityEngine;

public class AudioPlaySoundHere : MonoBehaviour
{
    private float       timer           = 0.0f;
    private float       lifetime        = 5.0f;

    void Start()
    {
        Audio.Instance.Play3DLocal(gameObject.name, transform.gameObject);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }
}
