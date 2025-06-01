using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI tmpText;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    void Start()
    {
        if (tmpText == null)
            tmpText = GetComponentInChildren<TextMeshProUGUI>();

        if (tmpText != null)
            tmpText.color = normalColor;
        else
            Debug.LogWarning("TMP não encontrado no objeto " + gameObject.name);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tmpText != null)
            tmpText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tmpText != null)
            tmpText.color = normalColor;
    }
}
