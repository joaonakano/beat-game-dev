using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public List<Lane> lanes;

    public struct NoteInfo
    {
        public double time;
        public Lane lane;
        public bool isSpecial;

        public NoteInfo(double t, Lane l, bool s)
        {
            time = t;
            lane = l;
            isSpecial = s;
        }
    }

    public List<NoteInfo> noteData = new List<NoteInfo>();


    // Converte TICKS em SEGUNDOS de um parametro fornecido (Length, EndTime e Time)
    public double ConvertToMetricStamp(long noteParamToConvert)
    {
        var metricStamp = TimeConverter.ConvertTo<MetricTimeSpan>(noteParamToConvert, SongManager.Instance.midiFile.GetTempoMap());
        var stamp = (double)metricStamp.Minutes * 60f + metricStamp.Seconds + (double)metricStamp.Milliseconds / 1000f;

        return stamp;
    }


    // Faz toda a correspondencia de NOTA, LANE e se é uma NOTA ESPECIAL
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        System.Random random = new System.Random();

        Debug.Log("Starting the timestamp note addressment");

        foreach (var note in array)
        {
            var noteTime = ConvertToMetricStamp(note.Time);
            if (noteTime < SongManager.Instance.noteTime) continue;

            foreach (var lane in lanes)
            {
                if (note.NoteName == lane.settings.noteRestriction)
                {
                    // Define se a nota será preta
                    bool isSpecial = random.NextDouble() < lane.settings.darkNoteChance;

                    noteData.Add(new NoteInfo(noteTime, lane, isSpecial));
                    break;
                }
            }
        }

        Debug.Log("Ended the timestamp note addressment");

    }

    // Spawna as Notas
    public IEnumerator SpawnNotesCoroutine()
    {
        double onScreenDesiredTime = SongManager.Instance.noteTime;

        Debug.Log("Started the note spawning");

        for (int i = 0; i < noteData.Count; i++)
        {
            NoteInfo currentNote = noteData[i];
            double currentTime = noteData[i].time;
            double nextSpawnTime = currentTime - onScreenDesiredTime;

            yield return new WaitUntil(() => SongManager.Instance.GetAudioSourceTime() >= nextSpawnTime);

            GameObject prefabToUse = currentNote.isSpecial
                ? currentNote.lane.settings.darkNotePrefab
                : currentNote.lane.settings.lightNotePrefab;

            Vector3 spawnPosition = new Vector3(currentNote.lane.transform.position.x, currentNote.lane.transform.position.y, SongManager.Instance.noteSpawnZ);

            // Instanciação da Nota na posição correta (y = lane e z = spawn definido em Song Manager) 
            GameObject spawnedNote = Instantiate(prefabToUse, spawnPosition, Quaternion.identity);
            spawnedNote.transform.SetParent(currentNote.lane.transform);
            

            // Adição das informações da estrutura (STRUCT) no componente Note da nota instanciada
            Note noteComponent = spawnedNote.GetComponent<Note>();
            noteComponent.assignedLane = currentNote.lane;
            noteComponent.assignedTime = currentNote.time;
            noteComponent.isSpecialNote = currentNote.isSpecial;

            // Registro desse componente na FILA de notas da Lane
            currentNote.lane.RegisterNoteToQueue(noteComponent);
            
        }

        Debug.Log("Ended the note spawning");
    }
}
