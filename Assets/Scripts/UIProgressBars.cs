using UnityEngine;
using UnityEngine.UI;

public class UIProgressBars : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider specialSlider;

    void Update()
    {
        // Atualiza as barras com os valores do ScoreManager
        if (ScoreManager.Instance != null)
        {
            // Atualiza valores entre 0 e 1 (ou 0 e 100 se preferir, depende da config do slider)
            healthSlider.value = (float)ScoreManager.healthScore / 100f;
            specialSlider.value = (float)ScoreManager.specialPercentageScore / 100f;
        }
    }
}
