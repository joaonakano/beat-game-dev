using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;        // Padrão SINGLETON que certifica que essa classe terá apenas uma instância e provê um acesso global. Static significa que pertence apenas à essa classe e permite o acesso sem precisar criar uma instancia da classe ScoreManager
    public AudioSource hitSFX;                  // Variável que armazena o Áudio Source do HIT
    public AudioSource missSFX;                 // Variável que armazena o Áudio Source do MISS
    public TMP_Text scoreText;                  // Variável que armazena o objeto de TEXT MESH PRO da Pontuação
    static int comboScore;                      // Variável que armazena o combo

    void Start()
    {
        Instance = this;                        // Indica que a variável estática receberá a instância dessa classe
        comboScore = 0;                         // Inicia o combo com 0
    }

    public static void Hit()
    {
        comboScore += 1;
        Instance.hitSFX.Play();
    }

    public static void Miss()
    {
        comboScore = 0;
        Instance.missSFX.Play();
    }

    void Update()
    {
        scoreText.text = comboScore.ToString();
    }
}
