using UnityEngine;
using TMPro;

public class KeybindRow : MonoBehaviour
{
    /*
    public TMP_Text actionText;
    public TMP_Text keyText;

    [HideInInspector] public string actionName;

    public void Setup(string action)
    {
        actionName = action;
        actionText.text = FormatActionName(action);
        keyText.text = KeybindManager.Instance.GetKey(action).ToString();
    }

    private string FormatActionName(string action)
    {
        switch (action)
        {
            case "Lane1": return "Lane 1";
            case "Lane2": return "Lane 2";
            case "Lane3": return "Lane 3";
            case "Lane4": return "Lane 4";
            case "Special": return "Special";
            case "Menu": return "Menu";
            case "SuperScore": return "Super Score";
            default: return action;
        }
    }

    public void OnClick()
    {
        KeybindUIManager.Instance.StartRebind(this);
    }

    public void UpdateKeyText()
    {
        keyText.text = KeybindManager.Instance.GetKey(actionName).ToString();
    }
    */
}
