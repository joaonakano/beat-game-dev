using UnityEngine;

public class DestroyParticleWhenPlaying : MonoBehaviour
{
    public float lifetime = 1f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
