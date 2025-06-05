using MilkShake;
using UnityEngine;

public class GameOverCameraBounce : MonoBehaviour
{
    // Shake Pivot
    public Shaker cameraShaker;

    // Instances
    private ShakeInstance shakeInstance;
    public static ShakeManager instance;

    public ShakePreset longShakePreset;


    private void Start()
    {
        cameraShaker.Shake(longShakePreset);
    }
}
