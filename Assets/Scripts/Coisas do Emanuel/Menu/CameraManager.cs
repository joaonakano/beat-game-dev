using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [Header("Referências")]
    public Transform cameraTransform;

    [Header("Posições dos Menus")]
    public Transform mainMenuPos;
    public Transform levelsPos;
    public Transform settingsPos;
    public Transform creditsPos;

    [Header("Configurações de Tween")]
    public float moveDuration = 1f;
    public Ease easeType = Ease.InOutSine;

    private void Start()
    {
        MoveToMainMenu();
    }

    public void MoveToMainMenu()
    {
        MoveCamera(mainMenuPos);
    }

    public void MoveToLevels()
    {
        MoveCamera(levelsPos);
    }

    public void MoveToSettings()
    {
        MoveCamera(settingsPos);
    }

    public void MoveToCredits()
    {
        MoveCamera(creditsPos);
    }

    private void MoveCamera(Transform targetPos)
    {
        if (targetPos == null || cameraTransform == null)
        {
            Debug.LogError("Referência nula no CameraManager!");
            return;
        }

        cameraTransform.DOMove(targetPos.position, moveDuration).SetEase(easeType);
        cameraTransform.DORotateQuaternion(targetPos.rotation, moveDuration).SetEase(easeType);
    }
}
