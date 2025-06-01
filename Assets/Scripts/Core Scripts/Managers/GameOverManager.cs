using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Painel de Game Over")]
    public GameObject gameOverPanel;

    private void Start()
    {
        // Come�a com o painel desativado
        gameOverPanel.SetActive(false);

        // Se inscreve no evento de vida zerada
        HealthManager.Instance.OnHealthDepleted += ShowGameOver;
    }

    private void OnDestroy()
    {
        // Remove a inscri��o no evento quando o objeto for destru�do
        if (HealthManager.Instance != null)
            HealthManager.Instance.OnHealthDepleted -= ShowGameOver;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // Bot�o de Retry
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Bot�o de voltar para o menu
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Substitua pelo nome da sua cena de menu
    }
}
