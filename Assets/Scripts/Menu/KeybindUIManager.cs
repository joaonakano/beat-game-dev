using UnityEngine;
using TMPro;
using System;

public class KeybindUIManager : MonoBehaviour
{
    public static KeybindUIManager Instance;

    [Header("Popup")]
    public GameObject rebindPopup;
    public TMP_Text rebindPopupText;

    [Header("Keybind Rows")]
    public GameObject rowPrefab;
    public Transform rowsParent;

    private KeybindRow currentRow;
    private bool waitingForKey = false;

    void Awake()
    {
        Instance = this;
        rebindPopup.SetActive(false);
    }

    void Start()
    {
        GenerateRows();
    }

    public void GenerateRows()
    {
        string[] actions = { "Lane1", "Lane2", "Lane3", "Lane4", "Special" };

        foreach (string action in actions)
        {
            GameObject obj = Instantiate(rowPrefab, rowsParent);
            KeybindRow row = obj.GetComponent<KeybindRow>();
            row.Setup(action);
        }
    }

    public void StartRebind(KeybindRow row)
    {
        currentRow = row;
        waitingForKey = true;
        rebindPopup.SetActive(true);
        rebindPopupText.text = $"Pressione uma tecla para {row.actionName}";
    }

    void Update()
    {
        if (waitingForKey)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    KeybindManager.Instance.SetKey(currentRow.actionName, key);
                    currentRow.UpdateKeyText();

                    InputManager.Instance.UpdateKeybindMap();

                    waitingForKey = false;
                    rebindPopup.SetActive(false);
                    break;
                }
            }
        }
    }
}
