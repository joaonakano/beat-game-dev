using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMatchEndManager : MonoBehaviour
{
    [Header("Painel de Match End")]
    public GameObject endMatchPanel;

    private void Start()
    {
        // Come�a com o painel desativado
        endMatchPanel.SetActive(false);

        // Se inscreve no evento de fim de musica
        SongManager.Instance.OnSongEnded += ShowEndGameOverlay;
    }

    private void OnDestroy()
    {
        // Remove a inscri��o no evento quando o objeto for destru�do
        if (SongManager.Instance != null)
            SongManager.Instance.OnSongEnded -= ShowEndGameOverlay;
    }

    void ShowEndGameOverlay()
    {
        endMatchPanel.SetActive(true);
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
