using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ResolutionDropdown : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Text resolutionDisplay;

    Resolution[] availableResolutions;
    int currentResolutionIndex = 0;

    void Start()
    {
        availableResolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + "x" + availableResolutions[i].height;
            if (!options.Contains(option)) // Evita resoluções duplicadas
            {
                options.Add(option);
            }

            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Atualiza o painel amarelo no início
        UpdateResolutionDisplay(resolutionDropdown.value);

        // Adiciona o listener
        resolutionDropdown.onValueChanged.AddListener(UpdateResolutionDisplay);
    }

    public void UpdateResolutionDisplay(int index)
    {
        string[] res = resolutionDropdown.options[index].text.Split('x');
        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);

        // Atualiza o texto no painel amarelo
        resolutionDisplay.text = width + "x" + height;

        // Aplica a resolução
        Screen.SetResolution(width, height, FullScreenMode.Windowed);
    }
}
