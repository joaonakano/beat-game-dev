using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;
    public GameObject menuMusicSFX;

    void Start()
    {
        ShowMainMenu();
    }

    // MAIN MENU BUTTONS
    public void OnPlayClicked()
    {
        // Carrega a cena principal (certifique-se de adicioná-la no Build Settings)
        SceneManager.LoadScene("Game");
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

    // SETTINGS MENU BUTTONS
    public void OnGraphicsClicked()
    {
        Debug.Log("Abrir configurações gráficas");
        // Aqui você pode abrir um painel de gráficos, por exemplo.
    }

    public void OnAudioClicked()
    {
        Debug.Log("Abrir configurações de áudio");
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
        Debug.Log("Abrir configurações de controles");
        // Aqui você pode abrir um painel de controles.
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
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }
}
