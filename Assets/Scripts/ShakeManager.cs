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
        if (ScoreManager.healthScore > 50f)
        {
            shakeStrength *= 1 + (0.01f * Time.deltaTime);
            shakeStrength = Mathf.Clamp(shakeStrength, 1f, 3f);

            SetStrengthAndRoughnessScale(shakeInstance, shakeStrength);
        }
        else
        {
            float normalizedHealth = Mathf.Clamp01((float)ScoreManager.healthScore / 50f);
            float decayMultiplier = 1f - normalizedHealth;
            float decayRate = decayMultiplier * 2f;

            shakeStrength -= decayRate * Time.deltaTime;
            shakeStrength = Mathf.Clamp(shakeStrength, 0.7f, 3f);

            SetStrengthAndRoughnessScale(shakeInstance, shakeStrength);
        }

        // Debug.Log($"ShakeStrengthConstant: {shakeStrength} - ShakeStrengthScale: {shakeInstance.CurrentStrength} - ShakeRoughnessScale: {shakeInstance.CurrentRoughness}");
    }

    private void SetStrengthAndRoughnessScale(ShakeInstance instance, float strength)
    {
        instance.StrengthScale = strength;
        instance.RoughnessScale = strength;
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
