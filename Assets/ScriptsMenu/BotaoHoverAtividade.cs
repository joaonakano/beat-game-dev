using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotaoHoverAtividade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI texto;

    public void OnPointerEnter(PointerEventData eventData)
    {
        texto.color = Color.cyan;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        texto.color = new Color(209, 209, 209, 255);
    }
}
