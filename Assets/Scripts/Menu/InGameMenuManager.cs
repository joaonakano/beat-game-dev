using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    [Header("Canva com o Menu In-Game")] 
    public GameObject backgroundScreen;

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
        SongManager.Instance.PauseSong();
    }

    /// <summary>
    /// Método responsável por REINICIAR a partida.
    /// Carrega a cena definida para o jogo.
    /// </summary>
    public void RestartMatch()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Método responsável por CONTINUAR a partida.
    /// Define a música alguns TIMESAMPLES antes, assim a música volta alguns segundos e inicia novamente.
    /// </summary>
    public void ResumeMatch()
    {
        int playbackRollbackInSeconds = SongManager.Instance.audioSource.timeSamples - 100000;
        SongManager.Instance.audioSource.timeSamples = playbackRollbackInSeconds;
        SongManager.Instance.UnpauseSong();
    }

    /// <summary>
    /// Método responsável por voltar ao MENU PRINCIPAL a partir do MENU DO JOGO.
    /// Carrega a Cena definida "MainMenu".
    /// </summary>
    public void SwitchToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    /// <summary>
    /// Método responsável por entrar na aba CONFIGURAÇÕES dentro do Menu.
    /// </summary>
    public void GoToOptions()
    {
    }

    /// <summary>
    /// Método responsável por.
    /// </summary>
    public void GoToAudio()
    {
    }

    /// <summary>
    /// Se esse InGameManager for desativado por algum motivo,
    /// o Evento definido no InputManager é desvinculado deste Manager e fica vago para caso alguma outra função queira utilizá-lo.
    /// </summary>
    void OnDisable()
    {
        InputManager.Instance.OnMenuKeybindPressed -= SetMenuState;
    }
}
