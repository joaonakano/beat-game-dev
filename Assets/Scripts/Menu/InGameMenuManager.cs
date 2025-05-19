using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public GameObject MainMenuGame;
    public GameObject MenuOptions;
    public GameObject MenuMusicSFX;

    public void RestartMatch()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ResumeMatch()
    {
        int playbackRollbackInSeconds = SongManager.Instance.audioSource.timeSamples - 100000;
        SongManager.Instance.audioSource.timeSamples = playbackRollbackInSeconds;
        SongManager.Instance.UnpauseSong();
        gameObject.SetActive(false);
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
