using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Painel de Game Over")]
    public GameObject gameOverPanel;

    private void Start()
    {
        // Garante que o painel começa desativado
        gameOverPanel.SetActive(false);

        // Inscreve o método GameOver no evento de vida zerada
        HealthManager.Instance.OnHealthDepleted += GameOver;
    }

    private void OnDestroy()
    {
        // Remove a inscrição no evento para evitar erros quando trocar de cena
        if (HealthManager.Instance != null)
            HealthManager.Instance.OnHealthDepleted -= GameOver;
    }

    void GameOver()
    {
        Time.timeScale = 0f; // Pausa o jogo
        gameOverPanel.SetActive(true);
    }

    // Botão de Retry
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Botão de voltar para o menu
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Coloque o nome correto da sua cena de menu
    }
}
