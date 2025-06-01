using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Painel de Game Over")]
    public GameObject gameOverPanel;

    private void Start()
    {
        // Garante que o painel come�a desativado
        gameOverPanel.SetActive(false);

        // Inscreve o m�todo GameOver no evento de vida zerada
        HealthManager.Instance.OnHealthDepleted += GameOver;
    }

    private void OnDestroy()
    {
        // Remove a inscri��o no evento para evitar erros quando trocar de cena
        if (HealthManager.Instance != null)
            HealthManager.Instance.OnHealthDepleted -= GameOver;
    }

    void GameOver()
    {
        Time.timeScale = 0f; // Pausa o jogo
        gameOverPanel.SetActive(true);
    }

    // Bot�o de Retry
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Bot�o de voltar para o menu
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Coloque o nome correto da sua cena de menu
    }
}
