using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }

}
