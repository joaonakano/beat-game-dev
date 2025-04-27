using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public AudioSource hitSFX;
    public AudioSource missSFX;

    public TMP_Text scoreText;
    static int comboScore;

    public TMP_Text missedText;
    static int missedNotesScore;

    void Start()
    {
        Instance = this;
        comboScore = 0;
        missedNotesScore = 0;
    }

    void Update()
    {
        scoreText.text = comboScore.ToString();
        missedText.text = missedNotesScore.ToString();
    }

    public static void Hit()
    {
        comboScore += 1;
        Instance.hitSFX.Play();
    }

    public static void Miss()
    {
        comboScore = 0;
        missedNotesScore += 1;
        Instance.missSFX.Play();
    }
}