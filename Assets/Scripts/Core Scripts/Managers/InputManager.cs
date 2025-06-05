using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
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

    [Header("Input System")]
    public PlayerInput _playerInput;

    private InputAction _menuOpenCloseAction;
    private InputAction _specialNoteAction;
    private InputAction _superModeAction;

    private bool isGameOverPanelActive = false;
    private bool isEndMatchPanelActive = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        SetupInputActions();
    }

    public void SetGameOverPanelState(bool active)
    {
        isGameOverPanelActive = active;
    }

    public void SetEndMatchPanelState(bool active)
    {
        isEndMatchPanelActive = active;
    }

    private void SetupInputActions()
    {
        var actions = _playerInput.actions;

        _menuOpenCloseAction = actions["MenuOpenClose"];
        _specialNoteAction = actions["SpecialNote"];
        _superModeAction = actions["SuperMode"];

        _menuOpenCloseAction.performed += OnMenuKeyPressed;
        _superModeAction.performed += OnSuperScorePressed;
        _specialNoteAction.performed += OnSpecialNotePressed;

        actions["Lane1"].performed += OnLane1Pressed;
        actions["Lane2"].performed += OnLane2Pressed;
        actions["Lane3"].performed += OnLane3Pressed;
        actions["Lane4"].performed += OnLane4Pressed;
    }

    private void OnMenuKeyPressed(InputAction.CallbackContext ctx)
    {
        if (isGameOverPanelActive || isEndMatchPanelActive)
            return;

        OnMenuKeybindPressed?.Invoke();
    }

    private void OnSuperScorePressed(InputAction.CallbackContext ctx)
    {
        OnSuperScoreKeybindPressed?.Invoke();
    }

    private void OnSpecialNotePressed(InputAction.CallbackContext ctx)
    {
        HandleSpecialNote();
    }

    private void OnLane1Pressed(InputAction.CallbackContext ctx) => HandleLaneKeyPress(0);
    private void OnLane2Pressed(InputAction.CallbackContext ctx) => HandleLaneKeyPress(1);
    private void OnLane3Pressed(InputAction.CallbackContext ctx) => HandleLaneKeyPress(2);
    private void OnLane4Pressed(InputAction.CallbackContext ctx) => HandleLaneKeyPress(3);

    private void HandleLaneKeyPress(int laneIndex)
    {
        if (SongManager.Instance == null || !SongManager.Instance.IsGameRunning())
            return;

        if (laneIndex < 0 || laneIndex > laneList.Count)
            return;

        Lane lane = laneList[laneIndex];
        double currentTime = SongManager.Instance.GetAudioSourceTime();
        double hitWindow = 0.15f;

        Queue<Note> queue = lane.GetNoteQueue();

        if (queue.Count == 0)
        {
            lane.WrongPressMiss();
            return;
        }

        Note note = queue.Peek();

        if (note.hasBeenProcessed)
            return;

        double timeDiff = currentTime - note.assignedTime;

        if (timeDiff >= -hitWindow && timeDiff <= hitWindow && !note.isSpecialNote)
        {
            bool superActive = ScoreManager.Instance.IsSuperActive();
            ScoreManager.Instance.RegisterHit(false, superActive);
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

    private void HandleSpecialNote()
    {
        if (SongManager.Instance == null || !SongManager.Instance.IsGameRunning())
            return;

        double currentTime = SongManager.Instance.GetAudioSourceTime();
        double hitWindow = 0.15f;

        bool foundValidSpecialNote = false;

        foreach (var lane in laneList)
        {
            Queue<Note> queue = lane.GetNoteQueue();

            if (queue.Count == 0)
                continue;

            Note note = queue.Peek();

            if (note.hasBeenProcessed || !note.isSpecialNote)
                continue;

            double timeDiff = currentTime - note.assignedTime;

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

    private void OnDestroy()
    {
        if (_menuOpenCloseAction != null)
            _menuOpenCloseAction.performed -= OnMenuKeyPressed;

        if (_superModeAction != null)
            _superModeAction.performed -= OnSuperScorePressed;

        if (_specialNoteAction != null)
            _specialNoteAction.performed -= OnSpecialNotePressed;

        var actions = _playerInput.actions;

        if (actions != null)
        {
            actions["Lane1"].performed -= OnLane1Pressed;
            actions["Lane2"].performed -= OnLane2Pressed;
            actions["Lane3"].performed -= OnLane3Pressed;
            actions["Lane4"].performed -= OnLane4Pressed;
        }
    }
}
