using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

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

    private Transform cameraRig;

    void Start()
    {
        cameraRig = Camera.main.transform.parent;

        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.color = new Color(0, 0, 0, 0);
        }
    }

    public void MoveTo(Transform target)
    {
        StartCoroutine(FadeAndMove(target));
    }

    public void MoveToMainMenu() => MoveTo(mainMenuPos);
    public void MoveToSettings() => MoveTo(settingsPos);
    public void MoveToLevels() => MoveTo(levelsPos);
    public void MoveToCredits() => MoveTo(creditsPos);

    IEnumerator FadeAndMove(Transform target)
    {
        yield return Fade(1);

        cameraRig.DOMove(target.position, moveDuration).SetEase(moveEase);
        cameraRig.DORotateQuaternion(target.rotation, moveDuration).SetEase(moveEase);

        yield return new WaitForSeconds(moveDuration);

        yield return Fade(0);
    }

    IEnumerator Fade(float targetAlpha)
    {
        if (fadeImage == null)
            yield break;

        fadeImage.DOFade(targetAlpha, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }
}
