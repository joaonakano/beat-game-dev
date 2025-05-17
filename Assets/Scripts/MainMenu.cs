using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuGame;
    public GameObject MenuOptions;

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
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


}
