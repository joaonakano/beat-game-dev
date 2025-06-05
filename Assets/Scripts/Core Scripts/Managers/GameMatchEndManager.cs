using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMatchEndManager : MonoBehaviour
{
    [Header("Painel de Match End")]
    public GameObject endMatchPanel;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text missText;
    [SerializeField] private TMP_Text lastMilestoneText;

    private void Start()
    {
        // Começa com o painel desativado
        endMatchPanel.SetActive(false);

        // Se inscreve no evento de fim de musica
        SongManager.Instance.OnSongEnded += ShowEndGameOverlay;
    }

    private void OnDestroy()
    {
        // Remove a inscrição no evento quando o objeto for destruído
        if (SongManager.Instance != null)
            SongManager.Instance.OnSongEnded -= ShowEndGameOverlay;
    }

    void ShowEndGameOverlay()
    {
        scoreText.text = ScoreTracker.Instance.HighestScore.ToString();
        missText.text = ScoreTracker.Instance.Misses.ToString();
        lastMilestoneText.text = ScoreTracker.Instance.LastMilestone.ToString();
        endMatchPanel.SetActive(true);
        InputManager.Instance.SetEndMatchPanelState(true);
    }

    // Botão de Retry
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Botão de voltar para o menu
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Substitua pelo nome da sua cena de menu
    }
}
