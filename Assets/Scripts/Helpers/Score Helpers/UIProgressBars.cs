using UnityEngine;
using UnityEngine.UI;

public class UIProgressBars : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider specialSlider1;
    [SerializeField] private Slider specialSlider2;

    void Update()
    {
        if (ScoreManager.Instance != null)
        {
            float healthValue = (float)HealthManager.Instance.CurrentHealth / 100f;
            float specialValue = (float)ScoreTracker.Instance.Special / 100f;

            healthSlider.value = healthValue;
            specialSlider1.value = specialValue;
            specialSlider2.value = specialValue;
        }
    }
}