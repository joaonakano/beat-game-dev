using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public GameObject MainMenuGame;
    public GameObject MenuOptions;
    public GameObject MenuMusicSFX;

<<<<<<< HEAD:Assets/Scripts/Menu/InGameMenuManager.cs
=======
    [SerializeField]
    private bool isMenuBeingShown = false;

    void Start()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("InGameMenuManager: InputManager.Instance is null!");
            return;
        }

        // Armazena no Evento do InputManager a função que exibe ou esconde o Menu
        InputManager.Instance.OnMenuKeybindPressed += SetMenuState;
    }

    /// <summary>
    /// Função que altera o estado do MENU: exibir ou esconder? Eis a questão.
    /// </summary>
    public void SetMenuState()
    {
        if (isMenuBeingShown)
        {
            HideMenu();
            Debug.Log("HIDING THE MENU");
        }
        else
        {
            ShowMenu();
            Debug.Log("SHOWING THE MENU");
        }

        isMenuBeingShown = !isMenuBeingShown;
    }

    /// <summary>
    /// Método que esconde o Menu para o jogador e retoma a partida.
    /// </summary>
    public void HideMenu()
    {
        backgroundScreen.SetActive(false);
        ResumeMatch();
    }

    /// <summary>
    /// Método que exibe o Menu para o jogador e pausa a partida.
    /// </summary>
    public void ShowMenu()
    {
        backgroundScreen.SetActive(true);
        AudioManager.Instance.Pause("song");
    }

    /// <summary>
    /// Método responsável por REINICIAR a partida.
    /// Carrega a cena definida para o jogo.
    /// </summary>
>>>>>>> main:Assets/Scripts/Coisas do Emanuel/Menu/InGameMenuManager.cs
    public void RestartMatch()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ResumeMatch()
    {
<<<<<<< HEAD:Assets/Scripts/Menu/InGameMenuManager.cs
        int playbackRollbackInSeconds = SongManager.Instance.audioSource.timeSamples - 100000;
        SongManager.Instance.audioSource.timeSamples = playbackRollbackInSeconds;
        SongManager.Instance.UnpauseSong();
        gameObject.SetActive(false);
=======
        int playbackRollbackInSeconds = SongManager.Instance.songAudioSource.timeSamples - 100000;
        SongManager.Instance.songAudioSource.timeSamples = playbackRollbackInSeconds;
        AudioManager.Instance.Unpause("song");
>>>>>>> main:Assets/Scripts/Coisas do Emanuel/Menu/InGameMenuManager.cs
    }

    public void SwitchToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void BackToMenu()
    {
        MenuOptions.SetActive(false);
        MainMenuGame.SetActive(true);
    }

    public void GoToOptions()
    {
        MainMenuGame.SetActive(false);
        MenuOptions.SetActive(true);
    }

    public void GoToAudio()
    {
        MenuOptions.SetActive(false);
        MenuMusicSFX.SetActive(true);
    }
}