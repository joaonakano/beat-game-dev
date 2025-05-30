using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggle : MonoBehaviour
{
    public Toggle fullscreenToggle;

    void Start()
    {
        // Inicializa o toggle com o estado atual
        fullscreenToggle.isOn = Screen.fullScreen;

        // Adiciona o listener
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
