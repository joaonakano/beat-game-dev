using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    public static void InstantiateHitParticles(ParticleSystem particleSystem, Vector3 coord, Quaternion rotation, float t)
    {
        ParticleSystem hitInstance = Instantiate(particleSystem, coord, rotation);
        Destroy(hitInstance.gameObject, t);
    }
}
