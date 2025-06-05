using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    [Header("Menus Principais")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject settingsMenuInGame;

    [Header("Submenus de Settings")]
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel;

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

        InputManager.Instance.OnMenuKeybindPressed += ToggleMenu;
        HideAll();
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
        settingsMenuInGame.SetActive(false);
        HideSubSettings();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Pause("song");
            AudioManager.Instance.Pause("interaction");
            AudioManager.Instance.Pause("voiceline");
        }

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isMenuOpen = false;

        menuCanvas.SetActive(false);
        settingsMenuInGame.SetActive(false);
        HideSubSettings();

        if (SongManager.Instance?.songAudioSource != null)
        {
            int newSample = Mathf.Max(SongManager.Instance.songAudioSource.timeSamples - rollbackSamples, 0);
            SongManager.Instance.songAudioSource.timeSamples = newSample;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Unpause("song");
            AudioManager.Instance.Unpause("interaction");
            AudioManager.Instance.Unpause("voiceline");
        }

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

    // Menu Settings principal
    public void OpenSettings()
    {
        settingsMenuInGame.SetActive(true);
        menuCanvas.SetActive(false);
        HideSubSettings();
    }

    public void CloseSettings()
    {
        settingsMenuInGame.SetActive(false);
        menuCanvas.SetActive(true);
        HideSubSettings();
    }

    // Submenus dentro do Settings
    public void OpenGraphics()
    {
        HideSubSettings();
        graphicsPanel.SetActive(true);
    }

    public void OpenAudio()
    {
        HideSubSettings();
        audioPanel.SetActive(true);
    }

    public void OpenControls()
    {
        HideSubSettings();
        controlsPanel.SetActive(true);
    }

    public void BackToSettingsMenu()
    {
        HideSubSettings();
    }

    // Esconde todos os submenus de configuração
    private void HideSubSettings()
    {
        graphicsPanel.SetActive(false);
        audioPanel.SetActive(false);
        controlsPanel.SetActive(false);
    }

    // Esconde tudo (chamado no Start)
    private void HideAll()
    {
        menuCanvas.SetActive(false);
        settingsMenuInGame.SetActive(false);
        HideSubSettings();
        isMenuOpen = false;
    }
}
