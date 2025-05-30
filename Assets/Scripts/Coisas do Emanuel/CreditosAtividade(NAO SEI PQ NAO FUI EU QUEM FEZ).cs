using UnityEngine;
using UnityEngine.UI;

public class CreditosAtividade : MonoBehaviour
{
    public float speed = 50f;
    private RectTransform creditsTransform;
    void Start()
    {
        creditsTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        creditsTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;
    }
}