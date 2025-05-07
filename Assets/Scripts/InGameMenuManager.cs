using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{

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
}
