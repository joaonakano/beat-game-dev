using UnityEngine;

public class PauseAmbientSound : MonoBehaviour
{
    public AudioSource ambientSound;
    public KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;

    void Start()
    {
        if (ambientSound != null)
        {
            ambientSound.loop = true;
            ambientSound.Stop();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Pausa o jogo
            if (ambientSound != null) ambientSound.Play();
        }
        else
        {
            Time.timeScale = 1; // Retoma o jogo
            if (ambientSound != null) ambientSound.Stop();
        }
    }
}
