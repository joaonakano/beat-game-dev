using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject menuLevels;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject menuMusicSFX;
    public GameObject graphicsMenu;
    public GameObject controlsMenu;

    void Start()
    {
        ShowMainMenu();
    }

    // MAIN MENU BUTTONS
    public void OnPlayClicked()
    {
        mainMenu.SetActive(false);
        menuLevels.SetActive(true);
    }

    public void OnSettingsClicked()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnCreditsClicked()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // MENU LEVELS BUTTONS
    public void OnLevelPlayClicked()
    {
        SceneManager.LoadScene("Game"); // Troque "Game" pelo nome da sua cena do jogo
    }

    public void OnLevelBackClicked()
    {
        menuLevels.SetActive(false);
        mainMenu.SetActive(true);
    }

    // SETTINGS MENU BUTTONS
    public void OnGraphicsClicked()
    {
        settingsMenu.SetActive(false);
        graphicsMenu.SetActive(true);
    }

    public void OnGraphicsBackClicked()
    {
        graphicsMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnAudioClicked()
    {
        settingsMenu.SetActive(false);
        menuMusicSFX.SetActive(true);
    }

    public void OnAudioBackClicked()
    {
        menuMusicSFX.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnControlsClicked()
    {
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void OnControlsBackClicked()
    {
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OnSettingsBackClicked()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    // CREDITS MENU BUTTON 
    public void OnCreditsBackClicked()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        menuLevels.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        menuMusicSFX.SetActive(false);
    }
}
