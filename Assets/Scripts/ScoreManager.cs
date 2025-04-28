using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public AudioSource hitSFX;
    public AudioSource missSFX;

    static AudioClip endClip;

    [SerializeField]
    private AudioClip endSFX;

    public TMP_Text scoreText;
    static int comboScore;

    public TMP_Text missedText;
    static int missedNotesScore;

    public TMP_Text healthText;
    static double healthScore;

    public static double HealthScore => healthScore;

    void Start()
    {
        Instance = this;
        comboScore = 0;
        missedNotesScore = 0;
        healthScore = 100.0;
        endClip = endSFX;
    }

    void Update()
    {
        scoreText.text = comboScore.ToString();
        missedText.text = missedNotesScore.ToString();
        healthText.text = $"{healthScore:F2}%";
    }

    public static void Hit()
    {
        comboScore += 1;

        healthScore += 15.52;                                               // Arrumar depois e colocar um valor melhor de pontos de vida
        healthScore = Math.Clamp(healthScore, 0, 100);

        Instance.hitSFX.Play();
    }

    public static void Miss()
    {
        comboScore = 0;
        missedNotesScore += 1;
        healthScore -= 10.52;                                               // Arrumar depois e colocar um valor melhor de pontos de dano
        healthScore = Math.Clamp(healthScore,0, 100);

        Instance.missSFX.Play();
    }

    public static void End()
    {
        Instance.missSFX.clip = endClip;
        Instance.missSFX.Play();
    }
}