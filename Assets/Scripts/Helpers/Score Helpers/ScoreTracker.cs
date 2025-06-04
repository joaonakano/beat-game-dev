using System;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public static ScoreTracker Instance;

    [Header("Configurações de Pontuação")]
    [Tooltip("Passo entre milestones (100, 200...)")]
    public int milestoneStep = 100;

    [Tooltip("Razão de incremento do Super Score (0-100) por acerto comum")]
    public double specialComboRatio = 1.0;

    [Tooltip("Constante de incremento no combo por acerto comum")]
    public int comboIncrement = 1;

    public int Combo { get; private set; } = 0;
    public double Special { get; private set; } = 0.0;
    public int Misses { get; private set; } = 0;
    public int LastMilestone { get; private set; } = 0;
    public int HighestScore { get; private set; } = 0;

    public event Action<int> OnMilestoneReached;

    void Awake()
    {
        Instance = this;
    }

    public void Initialize(int noteCount)
    {
        Combo = 0;
        Misses = 0;
        Special = 0.0;
        LastMilestone = 0;
        HighestScore = 0;

        specialComboRatio = 1 / (noteCount * 0.3) * 100.0;
    }

    public void RegisterHit(bool isSpecial = false)
    {
        Combo += isSpecial ? comboIncrement * 3 : comboIncrement;

        if (Combo >= HighestScore)
        {
            HighestScore = Combo;
        }

        double boost = isSpecial ? specialComboRatio * 1.5 : specialComboRatio;

        Special += boost;
        Special = Math.Clamp(Special, 0.0, 100.0);
        CheckMilestone();
    }

    public void RegisterMiss(bool isSpecial = false)
    {
        Combo = 0;
        //Misses += isSpecial ? 2 : 1;
        Misses += 1;

        Special = 0.0;
    }

    public void RegisterWrongPress()
    {
        Combo = 0;
        Special = 0.0;
    }

    public void SetSpecial(double value)
    {
        Special = Math.Clamp(value, 0.0, 100.0);
    }

    public void DrainSpecialOverTime(double amountPerSecond, double deltaTime)
    {
        Special -= amountPerSecond * deltaTime;
        Special = Math.Clamp(Special, 0.0, 100.0);
    }

    private void CheckMilestone()
    {
        while (Combo >= LastMilestone + milestoneStep)
        {
            LastMilestone += milestoneStep;
            OnMilestoneReached?.Invoke(LastMilestone);
        }
        Debug.Log("Milestone: " + LastMilestone + " - Highest Score: " + HighestScore);
    }


}
