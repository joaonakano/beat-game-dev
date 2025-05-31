using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // KEYBINDS
    [Header("Keybinds")]
    public KeyCode specialNoteKeybind = KeyCode.R;
    public KeyCode inGameMenuKeybind = KeyCode.Escape;
    public KeyCode superScoreKeybind = KeyCode.T;

    // LANES
    [Header("Lista dos GameObjects das Lanes")]
    public List<Lane> laneList = new();

    // RELACIONAMENTO ENTRE LANES E TECLAS
    private Dictionary<KeyCode, Lane> KeybindMap = new();

    // Eventos
    public event Action OnSuperScoreKeybindPressed;
    public event Action OnMenuKeybindPressed;

    // Singleton
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

    private void Start()
    {
        UpdateKeybindMap();
    }

    private void Update()
    {
        double currentTime = SongManager.Instance.GetAudioSourceTime();
        double hitWindow = 0.15f;

        // Lógica de lanes
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

        // Special Note
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

        // Abrir Menu
        if (Input.GetKeyDown(inGameMenuKeybind))
            OnMenuKeybindPressed?.Invoke();

        // Super Score
        if (Input.GetKeyDown(superScoreKeybind))
            OnSuperScoreKeybindPressed?.Invoke();
    }

    /// <summary>
    /// Atualiza o mapeamento de teclas usando o KeybindManager.
    /// </summary>
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

        // Pega também as teclas de funções especiais
        specialNoteKeybind = KeybindManager.Instance.GetKey("Special");
        inGameMenuKeybind = KeybindManager.Instance.GetKey("Menu");
        superScoreKeybind = KeybindManager.Instance.GetKey("SuperScore");

        // Fallback se não encontrar
        if (specialNoteKeybind == KeyCode.None) specialNoteKeybind = KeyCode.R;
        if (inGameMenuKeybind == KeyCode.None) inGameMenuKeybind = KeyCode.Escape;
        if (superScoreKeybind == KeyCode.None) superScoreKeybind = KeyCode.T;
    }

    /// <summary>
    /// Valida se a quantidade de lanes e keybinds estão corretas.
    /// </summary>
    [ContextMenu("Validar Keybinds e Lanes")]
    private void ValidateKeybinds()
    {
        if (laneList.Count == 0)
        {
            Debug.LogWarning("Aviso: Lista de lanes está vazia.");
            return;
        }

        if (laneList.Count != 4)
        {
            Debug.LogWarning($"Aviso: Quantidade incorreta de lanes. Esperado: 4, Atual: {laneList.Count}");
        }
        else
        {
            Debug.Log($"Validação OK - {laneList.Count} lanes.");
        }
    }
}
