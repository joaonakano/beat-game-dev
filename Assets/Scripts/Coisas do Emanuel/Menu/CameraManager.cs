using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 0.5f;
    public bool fadeOnStart = true; // ✅ Controla se terá fade ao iniciar

    private void Awake()
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = fadeOnStart ? 1 : 0; // Se for fade inicial, começa preto
            fadeImage.color = color;
            fadeImage.raycastTarget = fadeOnStart;
        }
    }

    private void Start()
    {
        MoveToMainMenu(); // Move a câmera direto para MainMenu

        if (fadeOnStart && fadeImage != null)
        {
            // Faz o fade OUT se estiver ativado
            fadeImage.DOFade(0, fadeDuration).OnComplete(() => fadeImage.raycastTarget = false);
        }
    }

    public void MoveToMainMenu() => StartCoroutine(FadeAndMove(mainMenuPos));
    public void MoveToLevels() => StartCoroutine(FadeAndMove(levelsPos));
    public void MoveToSettings() => StartCoroutine(FadeAndMove(settingsPos));
    public void MoveToCredits() => StartCoroutine(FadeAndMove(creditsPos));

    private IEnumerator FadeAndMove(Transform targetPos)
    {
        if (targetPos == null)
        {
            Debug.LogError("O targetPos não está atribuído!");
            yield break;
        }

        // Fade In (para preto)
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = true;
            yield return fadeImage.DOFade(1, fadeDuration).WaitForCompletion();
        }

        // Move a câmera
        cameraTransform.DOMove(targetPos.position, moveDuration).SetEase(easeType);
        cameraTransform.DORotateQuaternion(targetPos.rotation, moveDuration).SetEase(easeType);
        yield return new WaitForSeconds(moveDuration);

        // Fade Out (para transparente)
        if (fadeImage != null)
        {
            yield return fadeImage.DOFade(0, fadeDuration).WaitForCompletion();
            fadeImage.raycastTarget = false;
        }
    }
}
