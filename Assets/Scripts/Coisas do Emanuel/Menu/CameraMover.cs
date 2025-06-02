using UnityEngine;
using DG.Tweening;

public class CameraMover : MonoBehaviour
{
    [Header("Posições da Câmera")]
    public Transform mainMenuPosition;
    public Transform settingsPosition;
    public Transform levelsPosition;
    public Transform creditsPosition;

    [Header("Configurações de Animação")]
    public float moveDuration = 1.0f;
    public Ease easeType = Ease.InOutSine;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void MoveToMainMenu()
    {
        mainCamera.transform.DOMove(mainMenuPosition.position, moveDuration).SetEase(easeType);
    }

    public void MoveToSettings()
    {
        mainCamera.transform.DOMove(settingsPosition.position, moveDuration).SetEase(easeType);
    }

    public void MoveToLevels()
    {
        mainCamera.transform.DOMove(levelsPosition.position, moveDuration).SetEase(easeType);
    }

    public void MoveToCredits()
    {
        mainCamera.transform.DOMove(creditsPosition.position, moveDuration).SetEase(easeType);
    }
}
