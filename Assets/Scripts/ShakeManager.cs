using MilkShake;
using UnityEngine;

public class ShakeManager : MonoBehaviour
{
    public ShakePreset hitShakePreset;
    public ShakePreset longShakePreset;

    public Shaker cameraShaker;
    private ShakeInstance shakeInstance;

    public static ShakeManager instance;

    private float shakeStrength = 1.0f;

    void Start()
    {
        instance = this;
        shakeInstance = cameraShaker.Shake(longShakePreset);
        Invoke(nameof(LongShake), 3f);
    }

    void Update()
    {
        if (ScoreManager.HealthScore > 50f)
        {
            shakeStrength *= 1 + (0.01f * Time.deltaTime);
            shakeStrength = Mathf.Clamp(shakeStrength, 1f, 3f);

            shakeInstance.StrengthScale = shakeStrength;
            shakeInstance.RoughnessScale = shakeStrength;
        }
        else
        {
            float normalizedHealth = Mathf.Clamp01((float)ScoreManager.HealthScore / 50f);
            float decayMultiplier = 1f - normalizedHealth;
            float decayRate = decayMultiplier * 2f;

            shakeStrength -= decayRate * Time.deltaTime;
            shakeStrength = Mathf.Clamp(shakeStrength, 0.7f, 3f);

            shakeInstance.StrengthScale = shakeStrength;
            shakeInstance.RoughnessScale = shakeStrength;
        }

        // Debug.Log($"ShakeStrengthConstant: {shakeStrength} - ShakeStrengthScale: {shakeInstance.CurrentStrength} - ShakeRoughnessScale: {shakeInstance.CurrentRoughness}");
    }

    public void HitShake()
    {
        CameraBounce.ShakeCamera(hitShakePreset, cameraShaker);
    }

    public void LongShake()
    {
        shakeInstance.Start(1f);
    }
}
