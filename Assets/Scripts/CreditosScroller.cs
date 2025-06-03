using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditsRoll : MonoBehaviour
{
    [Header("Configuração do Texto")]
    public RectTransform creditsText;
    public float scrollSpeed = 50f;

    [Header("Limite de Altura")]
    public float endYPosition = 800f;

    [Header("Menus")]
    public GameObject creditsMenu;
    public GameObject mainMenuGame;

    [Header("Música dos Créditos")]
    public AudioSource creditsMusic;

    [Header("Câmera")]
    public Transform cameraTransform;
    public Transform mainMenuPosition;

    [Header("Tween Config")]
    public float moveDuration = 1f;
    public Ease easeType = Ease.InOutSine;

    private Vector3 startPosition;
    private bool isScrolling = false;

    void OnEnable()
    {
        startPosition = creditsText.anchoredPosition;
        isScrolling = true;

        if (creditsMusic != null)
        {
            creditsMusic.Play();
        }
    }

    void Update()
    {
        if (isScrolling)
        {
            creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

            if (creditsText.anchoredPosition.y >= endYPosition)
            {
                EndCredits();
            }
        }
    }

    public void EndCredits()
    {
        isScrolling = false;

        if (creditsMusic != null)
        {
            creditsMusic.Stop();
        }

        creditsText.anchoredPosition = startPosition;

        creditsMenu.SetActive(false);
        mainMenuGame.SetActive(true);

        // DOTween - Move a câmera de volta ao menu principal
        if (cameraTransform != null && mainMenuPosition != null)
        {
            cameraTransform.DOMove(mainMenuPosition.position, moveDuration).SetEase(easeType);
            cameraTransform.DORotateQuaternion(mainMenuPosition.rotation, moveDuration).SetEase(easeType);
        }
    }

    public void SkipCredits()
    {
        EndCredits();
    }
}
