using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // KEYBINDS
    public List<KeyCode> laneKeybinds = new();
    public KeyCode specialNoteKeybind = KeyCode.R;

    // LANES
    public List<Lane> laneList = new();

    // RELATIONSHIP BETWEEN LANES AND KEYBINDS
    private Dictionary<KeyCode, Lane> KeybindMap = new();

    // Singleton instance para acesso global ao InputManager
    public static InputManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateKeybindMap();
    }

    void Update()
    {
        double currentTime = SongManager.Instance.GetAudioSourceTime();
        double hitWindow = 0.15f;

        foreach (var kvp in KeybindMap)
        {
            if (Input.GetKeyDown(kvp.Key) && SongManager.Instance.IsGameRunning())
            {
                Lane lane = kvp.Value;
                Queue<Note> queue = lane.GetNoteQueue();

                if (queue.Count == 0)
                {
                    lane.WrongPressMiss();
                    return;
                }

                Note note = queue.Peek();

                if (note.hasBeenProcessed)
                    return;

                double noteTime = note.assignedTime;
                double timeDiff = currentTime - noteTime;

                if (timeDiff >= -hitWindow && timeDiff <= hitWindow && !note.isSpecialNote)
                {
                    lane.Hit();
                    lane.HandleHit(note);
                }
                else if (timeDiff > hitWindow)
                {
                    lane.Miss();
                    lane.HandleMiss(note);
                }
                else if (timeDiff < -hitWindow)
                {
                    lane.WrongPressMiss();
                }
                else if (timeDiff >= -hitWindow && timeDiff <= hitWindow && note.isSpecialNote)
                {
                    lane.SuperMiss();
                }
            }
        }

        if (Input.GetKeyDown(specialNoteKeybind) && SongManager.Instance.IsGameRunning())
        {
            bool foundValidSpecialNote = false;

            foreach (var lane in laneList)
            {
                Queue<Note> queue = lane.GetNoteQueue();

                if (queue.Count == 0)
                    continue;

                Note note = queue.Peek();

                if (note.hasBeenProcessed || !note.isSpecialNote)
                    continue;

                double noteTime = note.assignedTime;
                double timeDiff = currentTime - noteTime;

                if (timeDiff >= -hitWindow && timeDiff <= hitWindow)
                {
                    lane.SuperHit();
                    lane.HandleHit(note);
                    foundValidSpecialNote = true;
                    return;
                }
                else if (timeDiff > hitWindow)
                {
                    lane.SuperMiss();
                    lane.HandleMiss(note);
                    foundValidSpecialNote = true;
                    break;
                }
            }

            if (!foundValidSpecialNote)
            {
                ScoreManager.WrongPressMiss();
            }
        }
    }

    public void UpdateKeybindMap()
    {
        KeybindMap.Clear();

        string[] actions = { "Lane1", "Lane2", "Lane3", "Lane4" };

        if (laneList.Count != actions.Length)
        {
            Debug.LogError("Erro: O número de lanes precisa ser igual ao número de ações definidas!");
            return;
        }

        for (int i = 0; i < laneList.Count; i++)
        {
            KeyCode key = KeybindManager.Instance.GetKey(actions[i]);

            if (!KeybindMap.ContainsKey(key))
                KeybindMap.Add(key, laneList[i]);
            else
                Debug.LogWarning($"Tecla {key} já está sendo usada por outra lane.");
        }

        
        KeyCode specialKey = KeybindManager.Instance.GetKey("Special");
        specialNoteKeybind = specialKey != KeyCode.None ? specialKey : KeyCode.R;
    }


    [ContextMenu("Validar Keybinds e Lanes")]
        private void ValidateKeybinds()
        {
            if (laneList.Count == 0 || laneKeybinds.Count == 0)
            {
                Debug.LogWarning("Aviso: Um dos campos está vazio.");
                return;
            }

            if (laneList.Count != laneKeybinds.Count)
            {
                Debug.LogWarning($"Aviso: Quantidade desigual - Keybinds: {laneKeybinds.Count}, Lanes: {laneList.Count}");
            }
            else
            {
                Debug.Log($"Validação OK - {laneList.Count} lanes e keybinds.");
            }
        }

}
