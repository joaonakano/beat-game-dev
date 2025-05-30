using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class Lane : MonoBehaviour
{
    public LaneSettings settings;

    private Queue<Note> noteQueue = new Queue<Note>();

    public void RegisterNoteToQueue(Note note)
    {
        noteQueue.Enqueue(note);
    }

    void Update()
    {
        if (!SongManager.Instance.IsGameRunning() || SongManager.Instance.HasSongEnded() || SongManager.Instance.HasSongBeenPaused())
            return;

        if (noteQueue.Count == 0) 
                return;

            double currentTime = SongManager.Instance.GetAudioSourceTime();
            double inputDelay = SongManager.Instance.inputDelayInMilliseconds / 1000.0;
            double margin = SongManager.Instance.marginOfError;

            Note nextNote = noteQueue.Peek();

            if (!nextNote.hasBeenProcessed && currentTime > nextNote.assignedTime + margin + inputDelay)
            {
                Miss();
                HandleMiss(nextNote);
            }
        
    }

    public Queue<Note> GetNoteQueue() => noteQueue;

    public void ProccessNoteHit()
    {

    }

    // Utils - Print a Floating Text
    public void ShowFloatingText(string text, GameObject splashPrefab)
    {
        GameObject prefab = Instantiate(splashPrefab, new Vector3(1f, transform.position.y, -3), settings.smallTextPrefab.transform.rotation, transform);
        prefab.GetComponentInChildren<TMPro.TMP_Text>().text = text;
    }

    // Note Interaction Handlers
    public void HandleHit(Note note)
    {
        ParticleSystemManager.InstantiateHitParticles(settings.hitParticleEffects,
            new Vector3(0, note.transform.position.y, note.transform.position.z), transform.rotation, 1);

        noteQueue.Dequeue();
        note.hasBeenProcessed = true;

        Destroy(note.gameObject);
    }

    public void HandleMiss(Note note)
    {
        noteQueue.Dequeue();
        note.hasBeenProcessed = true;

        Destroy(note.gameObject);
    }

    // Score Handlers
    public void SuperHit()
    {
        ScoreManager.SuperHit();
    }

    public void Hit()
    {
        ScoreManager.Hit();
    }

    public void Miss()
    {
        ScoreManager.Miss();
        ShowFloatingText("LOL!", TextManager.instance.missedSuperNoteTextPrefab);
    }

    public void SuperMiss()
    {
        ShowFloatingText("Dallpozzo m0m3ntz!", TextManager.instance.missedSuperNoteTextPrefab);
        ScoreManager.SuperMiss();
    }

    public void WrongPressMiss()
    {
        ShowFloatingText("Muito cedo, rapá!", TextManager.instance.tooEarlyTextPrefab);
        ScoreManager.WrongPressMiss();
    }
}