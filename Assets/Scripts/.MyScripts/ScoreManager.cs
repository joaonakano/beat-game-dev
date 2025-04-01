using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;        // Padr�o SINGLETON que certifica que essa classe ter� apenas uma inst�ncia e prov� um acesso global. Static significa que pertence apenas � essa classe e permite o acesso sem precisar criar uma instancia da classe ScoreManager
    public AudioSource hitSFX;                  // Vari�vel que armazena o �udio Source do HIT
    public AudioSource missSFX;                 // Vari�vel que armazena o �udio Source do MISS
    public TMP_Text scoreText;                  // Vari�vel que armazena o objeto de TEXT MESH PRO da Pontua��o
    static int comboScore;                      // Vari�vel que armazena o combo

    void Start()
    {
        Instance = this;                        // Indica que a vari�vel est�tica receber� a inst�ncia dessa classe
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
