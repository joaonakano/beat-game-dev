using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    [Header("Valores de Vida")]

    [Tooltip("Vida do Jogador (0-x)")]
    [SerializeField] private double currentHealth = 100.0;
    [SerializeField] private double maxHealth = 100.0;

    public double MaxHealth => maxHealth;
    public double CurrentHealth => currentHealth;

    public event Action OnHealthDepleted;

    private void Awake()
    {
        Instance = this;
    }

    public void Heal(double baseAmount)
    {
        double missingHealthPercentage = (maxHealth - (float)currentHealth) / 100f;
        double healAmount = baseAmount * missingHealthPercentage;

        currentHealth += healAmount;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);
    }

    public void Damage(double amount)
    {
        currentHealth -= amount;
        currentHealth = Math.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth == 0)
        {
            OnHealthDepleted?.Invoke();
        }
    }
}
