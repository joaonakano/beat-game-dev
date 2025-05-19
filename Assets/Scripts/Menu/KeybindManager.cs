using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class KeybindManager : MonoBehaviour
{
    public static KeybindManager Instance;

    public Dictionary<string, KeyCode> keybinds = new Dictionary<string, KeyCode>();

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
        string[] actions = { "Lane1", "Lane2", "Lane3", "Lane4", "Special" }; // apenas uma tecla especial

        foreach (string action in actions)
        {
            string savedKey = PlayerPrefs.GetString(action, "");
            if (Enum.TryParse(savedKey, out KeyCode key))
                keybinds[action] = key;
            else
                keybinds[action] = KeyCode.None;
        }
    }

}
