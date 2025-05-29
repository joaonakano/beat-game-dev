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

        // Armazena no Evento do InputManager a fun��o que exibe ou esconde o Menu
        InputManager.Instance.OnMenuKeybindPressed += SetMenuState;
    }

    /// <summary>
    /// Fun��o que altera o estado do MENU: exibir ou esconder? Eis a quest�o.
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
    /// M�todo que esconde o Menu para o jogador e retoma a partida.
    /// </summary>
    public void HideMenu()
    {
        backgroundScreen.SetActive(false);
        ResumeMatch();
    }

    /// <summary>
    /// M�todo que exibe o Menu para o jogador e pausa a partida.
    /// </summary>
    public void ShowMenu()
    {
        backgroundScreen.SetActive(true);
        SongManager.Instance.PauseSong();
    }

    /// <summary>
    /// M�todo respons�vel por REINICIAR a partida.
    /// Carrega a cena definida para o jogo.
    /// </summary>
    public void RestartMatch()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// M�todo respons�vel por CONTINUAR a partida.
    /// Define a m�sica alguns TIMESAMPLES antes, assim a m�sica volta alguns segundos e inicia novamente.
    /// </summary>
    public void ResumeMatch()
    {
        int playbackRollbackInSeconds = SongManager.Instance.audioSource.timeSamples - 100000;
        SongManager.Instance.audioSource.timeSamples = playbackRollbackInSeconds;
        SongManager.Instance.UnpauseSong();
    }

    /// <summary>
    /// M�todo respons�vel por voltar ao MENU PRINCIPAL a partir do MENU DO JOGO.
    /// Carrega a Cena definida "MainMenu".
    /// </summary>
    public void SwitchToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    /// <summary>
    /// M�todo respons�vel por entrar na aba CONFIGURA��ES dentro do Menu.
    /// </summary>
    public void GoToOptions()
    {
    }

    /// <summary>
    /// M�todo respons�vel por.
    /// </summary>
    public void GoToAudio()
    {
    }

    /// <summary>
    /// Se esse InGameManager for desativado por algum motivo,
    /// o Evento definido no InputManager � desvinculado deste Manager e fica vago para caso alguma outra fun��o queira utiliz�-lo.
    /// </summary>
    void OnDisable()
    {
        InputManager.Instance.OnMenuKeybindPressed -= SetMenuState;
    }
}
