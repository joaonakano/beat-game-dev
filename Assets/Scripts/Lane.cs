using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lane : MonoBehaviour
{
    public NoteName noteRestriction;
    public KeyCode input;
    public GameObject notePrefab;

    private bool hasEnded = false;

    public RawImage crosshair;
    public ParticleSystem burstExplosionParticles;

    // Lista que armazena objetos Note
    List<Note> notes = new List<Note>();

    // Listas que armazenam minutagens
    public List<double> timeStamps = new List<double>();
    public List<double> durationTimeStamps = new List<double>();

    // Indices de controle
    int spawnIndex = 0;
    int inputIndex = 0;


    // Converte TICKS em SEGUNDOS de um parâmetro fornecido (Length, EndTime e Time)
    public double ConvertToMetricStamp(long noteParamToConvert)
    {
        var metricStamp = TimeConverter.ConvertTo<MetricTimeSpan>(noteParamToConvert, SongManager.midiFile.GetTempoMap());
        var stamp = (double)metricStamp.Minutes * 60f + metricStamp.Seconds + (double)metricStamp.Milliseconds / 1000f;

        return stamp;
    }

    // Define os segundos em que devem surgir as notas
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var noteTime = ConvertToMetricStamp(note.Time);
                var noteEndTime = ConvertToMetricStamp(note.EndTime);
                var noteDuration = ConvertToMetricStamp(note.Length);

                durationTimeStamps.Add(noteDuration);
                timeStamps.Add(noteTime);

                /* CÓDIGO QUE ADICIONA NOTAS LONGAS (EXTRAMAMENTE INSTÁVEL)
                
                if (noteDuration >= 1)
                {
                    var numberOfNotes = Mathf.RoundToInt((float) noteDuration / 0.02f);
                    for (int i = 1; i <= numberOfNotes; i++)
                    {
                        timeStamps.Add(noteTime + i * 0.02f);
                    }
                }
                Debug.Log($"Note start time: {noteTime} - Note end time: {noteEndTime} - Duration: {noteDuration}");

                */

            }
        }
    }

    void Update()
    {
        if (Input.GetKey(input))
        {
            crosshair.CrossFadeAlpha(1, 0.1f, false);
        }
        else
        {
            crosshair.CrossFadeAlpha(0.1f, 0, false);
        }

        if (spawnIndex < timeStamps.Count)                              // Lógica de Spawn das Notas
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                note.SetActive(true);
                spawnIndex++;
            }

        }

        if (inputIndex < timeStamps.Count)                              // Lógica de Interação com as Notas
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input) && ScoreManager.HealthScore > 0)
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {

                    Hit();
                    ParticleSystem explosionInstance = Instantiate(burstExplosionParticles, new Vector3(0, notes[inputIndex].gameObject.transform.position.y, notes[inputIndex].gameObject.transform.position.z), transform.rotation);
                    Destroy(notes[inputIndex].gameObject);
                    Destroy(explosionInstance.gameObject, 1);
                    inputIndex++;
                }
                else
                {
                    //print($"Hit impreciso na nota ${inputIndex} com delay de {Math.Abs(audioTime - timeStamp)} segundos");
                }
            }

            if (timeStamp + marginOfError <= audioTime)                 // Se o tempo da música for superior, exemplo 2 segundos, ao timeStamp da nota, exemplo 1.5 segundos, quer dizer que a nota passou do limite aceitável de TAPPING (2s > 1.5s)
            {
                if (!notes[inputIndex].gameObject.activeSelf)           // Se a nota a ser verificada tiver seu Estado de Ativação como FALSO, isso quer dizer que o AIMBOT reconheceu a nota e registrou um acerto, portanto, ela deve ser destruída
                {
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    if (ScoreManager.HealthScore > 0)
                    {
                        Miss();
                    }
                    inputIndex++;
                }
            }

        }

        if (ScoreManager.HealthScore == 0)
        {
            if (SongManager.Instance.audioSource.pitch > 0)
            {
                SongManager.Instance.audioSource.pitch -= 0.08f * Time.deltaTime;
            }
            else
            {
                SongManager.Instance.audioSource.pitch = 0;
                if (!hasEnded)
                {
                    End();
                    hasEnded = true;
                }
            }
        }
    }

    private void Hit()
    {
        ScoreManager.Hit();
    }

    private void Miss()
    {
        ScoreManager.Miss();
    }

    private void End()
    {
        ScoreManager.End();
    }
}