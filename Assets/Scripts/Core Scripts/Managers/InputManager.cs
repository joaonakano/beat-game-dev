using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // KEYBINDS
    public List<KeyCode> laneKeybinds = new();
    public KeyCode specialNoteKeybind = KeyCode.R;
<<<<<<< HEAD:Assets/Scripts/Managers/InputManager.cs
=======
    public KeyCode inGameMenuKeybind = KeyCode.Escape;
    public KeyCode superScoreKeybind = KeyCode.T;
>>>>>>> main:Assets/Scripts/Core Scripts/Managers/InputManager.cs

    // LANES
    public List<Lane> laneList = new();

    // RELATIONSHIP BETWEEN LANES AND KEYBINDS
    private Dictionary<KeyCode, Lane> KeybindMap = new();

<<<<<<< HEAD:Assets/Scripts/Managers/InputManager.cs
=======
    // EVENTS
    public event Action OnSuperScoreKeybindPressed;
    public event Action OnMenuKeybindPressed;

    public static InputManager Instance;

>>>>>>> main:Assets/Scripts/Core Scripts/Managers/InputManager.cs
    void Start()
    {
        if (laneList.Count != laneKeybinds.Count)
        {
            Debug.LogError("Erro: O número de keybinds precisa ser igual ao número de lanes!");
            return;
        }

        for (int i = 0; i < laneList.Count; i++)
        {
            if (!KeybindMap.ContainsKey(laneKeybinds[i]))
                KeybindMap.Add(laneKeybinds[i], laneList[i]);
        }
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
                    bool superActive = ScoreManager.Instance.IsSuperActive();
                    ScoreManager.Instance.RegisterHit(isSpecial: false, isSuperScoreActive: superActive);
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
<<<<<<< HEAD:Assets/Scripts/Managers/InputManager.cs
=======

        if (Input.GetKeyDown(inGameMenuKeybind))
            OnMenuKeybindPressed?.Invoke();

        if (Input.GetKeyDown(superScoreKeybind))
            OnSuperScoreKeybindPressed?.Invoke();
            
>>>>>>> main:Assets/Scripts/Core Scripts/Managers/InputManager.cs
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