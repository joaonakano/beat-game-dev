using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    [Header("Elementos de UI")] 
    [SerializeField] private GameObject menuCanvas;

    [Header("Config")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private int rollbackSamples = 100000;

    private bool isMenuOpen = false;

    void Start()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("InGameMenuManager: InputManager.Instance is null!");
            return;
        }

        // Armazena no Evento do InputManager a função que exibe ou esconde o Menu
        InputManager.Instance.OnMenuKeybindPressed += ToggleMenu;
        HideMenu();
    }

    void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnMenuKeybindPressed -= ToggleMenu;
    }

    public void ToggleMenu()
    {
        if (isMenuOpen)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        isMenuOpen = true;
        menuCanvas.SetActive(true);

        if (AudioManager.Instance != null)
            AudioManager.Instance.Pause("song");

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isMenuOpen = false;
        menuCanvas.SetActive(false);

        if (SongManager.Instance?.songAudioSource != null)
        {
            int newSample = Mathf.Max(SongManager.Instance.songAudioSource.timeSamples - rollbackSamples, 0);
            SongManager.Instance.songAudioSource.timeSamples = newSample;
        }

        if (AudioManager.Instance != null)
            AudioManager.Instance.Unpause("song");

        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(mainMenuSceneName);
    }

    public void GoToOptions() { Debug.Log("Configurações não foram configuradas ainda"); }

    private void HideMenu()
    {
        menuCanvas.SetActive(false);
        isMenuOpen = false;
    }
}