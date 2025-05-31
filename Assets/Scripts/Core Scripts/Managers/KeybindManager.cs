using UnityEngine;
using System;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    public Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

    private string waitingForKeyAction = null;

    private readonly List<string> actions = new List<string>
    {
        "Lane1", "Lane2", "Lane3", "Lane4", "Special", "Menu", "SuperScore"
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadKeybinds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (waitingForKeyAction != null)
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    SetKey(waitingForKeyAction, key);
                    waitingForKeyAction = null;
                    Debug.Log($"Set {waitingForKeyAction} to {key}");

                    KeybindUIManager.Instance.UpdateUI();
                    break;
                }
            }
        }
    }

    public void StartRebind(string action)
    {
        waitingForKeyAction = action;
        Debug.Log($"Press a key to bind {action}");
    }

    public void SetKey(string action, KeyCode key)
    {
        keybinds[action] = key;
        PlayerPrefs.SetString(action, key.ToString());
    }

    public KeyCode GetKey(string action)
    {
        return keybinds.ContainsKey(action) ? keybinds[action] : KeyCode.None;
    }

    public void LoadKeybinds()
    {
        foreach (string action in actions)
        {
            string savedKey = PlayerPrefs.GetString(action, "");
            if (Enum.TryParse(savedKey, out KeyCode key))
                keybinds[action] = key;
            else
                keybinds[action] = KeyCode.None;
        }
    }

    public void ResetToDefault()
    {
        SetKey("Lane1", KeyCode.Alpha3);
        SetKey("Lane2", KeyCode.E);
        SetKey("Lane3", KeyCode.D);
        SetKey("Lane4", KeyCode.C);
        SetKey("Special", KeyCode.R);
        SetKey("Menu", KeyCode.Escape);
        SetKey("SuperScore", KeyCode.T);
    }

    public bool IsRebinding()
    {
        return waitingForKeyAction != null;
    }

    public List<string> GetActions()
    {
        return actions;
    }
}
