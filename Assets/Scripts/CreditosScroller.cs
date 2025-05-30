using UnityEngine;

public class CreditosScroller : MonoBehaviour
{
    public float scrollSpeed = 50f;
    public float startY = -600f;
    public float endY = 1000f;

    private Vector3 startPos;

    void OnEnable()
    {
        // Resetar a posição quando o menu de créditos for aberto
        startPos = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);
        transform.localPosition = startPos;
    }

    void Update()
    {
        transform.localPosition += Vector3.up * scrollSpeed * Time.deltaTime;

        if (transform.localPosition.y >= endY)
        {
            transform.localPosition = startPos;
        }
    }
}
