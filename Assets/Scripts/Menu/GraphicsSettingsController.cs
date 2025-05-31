using UnityEngine;
using TMPro;

public class GraphicsSettingsController : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown textureDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        SetupResolutionOptions();
        SetupQualityOptions();
        SetupTextureOptions();
    }

    void SetupResolutionOptions()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    void SetupQualityOptions()
    {
        qualityDropdown.ClearOptions();

        string[] qualityNames = QualitySettings.names;
        qualityDropdown.AddOptions(new System.Collections.Generic.List<string>(qualityNames));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    void SetupTextureOptions()
    {
        textureDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string> { "Alta", "Média", "Baixa" };
        textureDropdown.AddOptions(options);

        int textureLimit = QualitySettings.globalTextureMipmapLimit; // 0 = alta, 1 = média, 2 = baixa
        textureDropdown.value = textureLimit;
        textureDropdown.RefreshShownValue();

        textureDropdown.onValueChanged.AddListener(SetTextureQuality);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }

    public void SetTextureQuality(int index)
    {
        QualitySettings.globalTextureMipmapLimit = index;
    }
}
