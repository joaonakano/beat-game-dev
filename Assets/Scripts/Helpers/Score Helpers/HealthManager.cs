using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [Header("Valores de Vida")]

    [Tooltip("Vida do Jogador (0-100)")]
    [SerializeField] private double currentHealth = 100.0;

    public double CurrentHealth => currentHealth;

    public event Action OnHealthDepleted;

    private void Awake()
    {
        Instance = this;
    }

    public void Heal(float baseAmount)
    {
        float missingHealthPercentage = (100f - (float)currentHealth) / 100f;
        float healAmount = baseAmount * missingHealthPercentage;

        currentHealth += healAmount;
        currentHealth = Math.Clamp(currentHealth, 0, 100);
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Math.Clamp(currentHealth, 0, 100);

        if (currentHealth == 0)
        {
            OnHealthDepleted?.Invoke();
        }
    }
}
