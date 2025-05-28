using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public NoteName noteRestriction;

    public KeyCode input;
    public KeyCode specialInput;

    public GameObject lightNotePrefab;
    public GameObject darkNotePrefab;

    public double darkNoteChance = 0.15;
    private List<bool> isDarkNoteList = new List<bool>();

    public ParticleSystem burstExplosionParticles;

    // Lista que armazena objetos Note
    List<Note> notes = new List<Note>();

    // Listas que armazenam minutagens
    public List<double> timeStamps = new List<double>();

    // Indices de controle
    int spawnIndex = 0;
    int inputIndex = 0;

    public GameObject textPrefab;

    private void Awake()
    {
        spawnIndex = 0;
        inputIndex = 0;
    }

    // Converte TICKS em SEGUNDOS de um par�metro fornecido (Length, EndTime e Time)
    public double ConvertToMetricStamp(long noteParamToConvert)
    {
        var metricStamp = TimeConverter.ConvertTo<MetricTimeSpan>(noteParamToConvert, SongManager.Instance.midiFile.GetTempoMap());
        var stamp = (double)metricStamp.Minutes * 60f + metricStamp.Seconds + (double)metricStamp.Milliseconds / 1000f;

        return stamp;
    }

    // Define os segundos em que devem surgir as notas
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        Debug.Log("The code of midi file was read, setting timestamps for lane");
        System.Random random = new System.Random();

        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var noteTime = ConvertToMetricStamp(note.Time);

                if (noteTime < SongManager.Instance.noteTime)
                    continue;

                timeStamps.Add(noteTime);

                // Define se a nota ser� preta
                bool isDark = random.NextDouble() < darkNoteChance;
                isDarkNoteList.Add(isDark);
            }
        }

        StartCoroutine(SpawnNotesCoroutine());
    }

    void Update()
    {
        if (!SongManager.Instance.hasEnded && !SongManager.Instance.isPaused)
        {
            // INTERACAO - PLAYER/NOTA
            if (inputIndex < timeStamps.Count && inputIndex < notes.Count)
            {
                double timeStamp = timeStamps[inputIndex];
                double marginOfError = SongManager.Instance.marginOfError;
                double audioTime = SongManager.Instance.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);
                bool isDarkNote = notes[inputIndex].isDarkNote;

                double timeDifference = Math.Abs(audioTime - timeStamp);

                GameObject noteGameObject = notes[inputIndex].gameObject;

                if (ScoreManager.healthScore > 0)
                {
                    bool specialInputPressed = Input.GetKeyDown(specialInput);
                    bool laneKeyPressed = Input.GetKeyDown(input);
                    bool noteProcessed = false;


                    if (timeDifference < marginOfError)
                    {
                        Vector3 notePosition = noteGameObject.transform.position;
                        if (isDarkNote)
                        {
                            if (specialInputPressed)
                            {
                                SuperHit();
                                ParticleSystemManager.InstantiateHitParticles(burstExplosionParticles, new Vector3(0, notePosition.y, notePosition.z), transform.rotation, 1);
                                Destroy(noteGameObject);
                                noteProcessed = true;
                            }
                            else if (laneKeyPressed)
                            {
                                SuperMiss();
                                noteProcessed = true;
                            }
                        }
                        else
                        {
                            if (laneKeyPressed)
                            {
                                Hit();
                                ParticleSystemManager.InstantiateHitParticles(burstExplosionParticles, new Vector3(0, notePosition.y, notePosition.z), transform.rotation, 1);
                                Destroy(noteGameObject);
                                noteProcessed = true;
                            }
                        }

                        if (noteProcessed)
                        {
                            inputIndex++;
                        }
                    }

                    if (laneKeyPressed && timeDifference > marginOfError)
                    {
                        WrongPressMiss();
                        ShowFloatingText("Muito cedo!", TextManager.instance.tooEarlyTextPrefab);
                    }
                }

                if (timeStamp + marginOfError <= audioTime)                 // Se o tempo da m�sica for superior, exemplo 2 segundos, ao timeStamp da nota, exemplo 1.5 segundos, quer dizer que a nota passou do limite aceit�vel de TAPPING (2s > 1.5s)
                {
                    if (!noteGameObject.activeSelf)                         // Se a nota a ser verificada tiver seu Estado de Ativa��o como FALSO, isso quer dizer que o AIMBOT reconheceu a nota e registrou um acerto, portanto, ela deve ser destru�da
                    {
                        Destroy(noteGameObject);
                        inputIndex++;
                    }
                    else
                    {
                        if (ScoreManager.healthScore > 0)
                        {
                            if (notes[inputIndex].isDarkNote)
                            {
                                SuperMiss();
                                ShowFloatingText("LOL!", TextManager.instance.missedSuperNoteTextPrefab);

                            }
                            else
                            {
                                Miss();
                            }
                        }
                        inputIndex++;
                    }
                }
            }
        }
    }

    // SPAWN DE NOTAS
    private IEnumerator SpawnNotesCoroutine()
    {
        for (int i = 0; i < timeStamps.Count; i++)
        {
            double currentTime = timeStamps[i];
            double spawnTime = currentTime - SongManager.Instance.noteTime;     // SpawnTime (qual segundo da musica a nota deve spawnar) = CurrentTime (segundo da nota) - NoteTime (tempo definido no inspector para a nota sair do ponto ORIGEM -> DESTINO)

            while (SongManager.Instance.GetAudioSourceTime() < spawnTime)       // Enquanto o segundo da musica (ex: musica está em 3s e a nota precisa ser spawnada no segundo 4.5) for menor que o segundo definido para a proxima nota, fica inativo.
            {
                yield return null;
            }

            GameObject prefabToUse = isDarkNoteList[i] ? darkNotePrefab : lightNotePrefab;
            var note = Instantiate(prefabToUse, transform);
            Note noteComponent = note.GetComponent<Note>();

            noteComponent.assignedTime = (float)currentTime;
            noteComponent.isDarkNote = isDarkNoteList[i];

            notes.Add(noteComponent);
            note.SetActive(true);

            spawnIndex++;
        }
    }

    private void ShowFloatingText(string text, GameObject splashPrefab)
    {
        GameObject prefab = Instantiate(splashPrefab, new Vector3(1f, transform.position.y, -3), textPrefab.transform.rotation, transform);
        prefab.GetComponentInChildren<TMPro.TMP_Text>().text = text;
    }

    private void SuperHit()
    {
        ScoreManager.SuperHit();
    }

    private void Hit()
    {
        ScoreManager.Hit();
    }

    private void Miss()
    {
        ScoreManager.Miss();
    }

    private void SuperMiss()
    {
        ScoreManager.SuperMiss();
    }

    private void WrongPressMiss()
    {
        ScoreManager.WrongPressMiss();
    }
}