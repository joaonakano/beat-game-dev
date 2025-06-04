using UnityEngine;

public class BlackWhiteShaderController : MonoBehaviour
{
    public static BlackWhiteShaderController Instance;
    [Header("Materiais de Shader")]
    [SerializeField] private MeshRenderer grayscaleObjectRenderer;
    [SerializeField] private float grayscaleLevel = 0;

    public float GrayscaleLevel => grayscaleLevel;

    void Awake()
    {
        Instance = this;
        grayscaleLevel = 0;
    }

    private void ChangeGrayscaleLevelTo(float level)
    {
        grayscaleObjectRenderer.material.SetFloat("_alpha", level);
    }

    private float CalculateLevelFromHealth(double currentHealth, double maxHealth)
    {
        float grayscaleFromHealth = 1f - ((float)currentHealth / (float)maxHealth);
        grayscaleFromHealth = Mathf.Clamp01(grayscaleFromHealth);

        return grayscaleFromHealth;
    }

    private void Update()
    {
        if (!SongManager.Instance.IsGameRunning())
            return;

        double currentHealth = HealthManager.Instance.CurrentHealth;
        double maxHealth = HealthManager.Instance.MaxHealth;

        grayscaleLevel = CalculateLevelFromHealth(currentHealth, maxHealth);

        ChangeGrayscaleLevelTo(grayscaleLevel);
    }
}
