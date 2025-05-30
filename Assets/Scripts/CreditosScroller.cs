using UnityEngine;

public class CreditosScroller : MonoBehaviour
{
    public float scrollSpeed = 50f; // Velocidade da rolagem
    public RectTransform creditsText; // Objeto do texto que sobe
    public float endY = 1000f; // Posição Y onde considera que terminou

    public GameObject menuCredits;  // Painel dos créditos
    public GameObject menuMain;      // Painel do menu principal (MainMenuGame)

    private Vector3 startPosition;

    void Start()
    {
        startPosition = creditsText.anchoredPosition;
    }

    void Update()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsText.anchoredPosition.y >= endY)
        {
            ReturnToMenu();
        }
    }

    public void ReturnToMenu()
    {
        // Reseta a posição do texto para caso abra novamente
        creditsText.anchoredPosition = startPosition;

        // Desativa o menu de créditos
        menuCredits.SetActive(false);

        // Ativa o menu principal
        menuMain.SetActive(true);
    }
}
