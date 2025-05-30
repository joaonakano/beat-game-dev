using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    // Spawna as Notas
    public IEnumerator SpawnNotesCoroutine(List<NoteInfo> noteData)
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
    