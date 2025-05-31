using UnityEngine;
using TMPro;

public class KeybindRow : MonoBehaviour
{
    public TMP_Text actionText;
    public TMP_Text keyText;

    [HideInInspector] public string actionName;

    public void Setup(string action)
    {
        actionName = action;
        actionText.text = action;
        keyText.text = KeybindManager.Instance.GetKey(action).ToString();
    }

    public void OnClick()
    {
        KeybindUIManager.Instance.StartRebind(this);
    }

    public void UpdateKeyText()
    {
        keyText.text = KeybindManager.Instance.GetKey(actionName).ToString();
    }
}
