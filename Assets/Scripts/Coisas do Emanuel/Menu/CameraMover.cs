using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CameraMover : MonoBehaviour
{
    [Header("Referências")]
    public Transform mainMenuPos;
    public Transform settingsPos;
    public Transform levelsPos;
    public Transform creditsPos;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    [Header("Movimento da Câmera")]
    public float moveDuration = 1f;
    public Ease moveEase = Ease.InOutSine;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0); // Começa transparente
        }
    }

    public void MoveToMainMenu()
    {
        StartCoroutine(FadeAndMove(mainMenuPos));
    }

    public void MoveToSettings()
    {
        StartCoroutine(FadeAndMove(settingsPos));
    }

    public void MoveToLevels()
    {
        StartCoroutine(FadeAndMove(levelsPos));
    }

    public void MoveToCredits()
    {
        StartCoroutine(FadeAndMove(creditsPos));
    }

    System.Collections.IEnumerator FadeAndMove(Transform target)
    {
        // Fade Out
        yield return Fade(1);

        // Movimenta a Câmera
        mainCamera.transform.DOMove(target.position, moveDuration).SetEase(moveEase);
        mainCamera.transform.DORotateQuaternion(target.rotation, moveDuration).SetEase(moveEase);

        yield return new WaitForSeconds(moveDuration);

        // Fade In
        yield return Fade(0);
    }

    System.Collections.IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null)
            yield break;

        fadeImage.DOFade(targetAlpha, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }
}
