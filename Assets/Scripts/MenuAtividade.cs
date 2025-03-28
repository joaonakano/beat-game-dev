using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuAtividade : MonoBehaviour
{
    [SerializeField] private string SceneName;
    [SerializeField] private GameObject MenuInicial;
    [SerializeField] private GameObject OptionsMenu;
    [SerializeField] private GameObject Credits;

    public void Play()
    {
        MusicManager.PlayBackgroundMusic(true);
        SceneManager.LoadScene(SceneName);
    }

    public void Options()
    {
        MenuInicial.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void ShowCredits()
    {
        MenuInicial.SetActive(false);
        Credits.SetActive(true);
    }

    public void ExitCredits()
    {
        Credits.SetActive(false);
        MenuInicial.SetActive(true);
    }

    public void CloseOptions()
    {
        OptionsMenu.SetActive(false);
        MenuInicial.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
