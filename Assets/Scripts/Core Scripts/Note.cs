using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    double hideNotePosition;

    public bool isSpecialNote;
    public double assignedTime;
    public Lane assignedLane;

    public bool hasBeenProcessed = false;

    void Start()
    {
        timeInstantiated = SongManager.Instance.GetAudioSourceTime();
        hideNotePosition = SongManager.Instance.noteTapZ * 2;
    }

    void Update()
    {
        double timeSinceInstantiated = SongManager.Instance.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));                 // Note Time Ratio - Usado para calcular taxa do Tempo de Existência de uma Nota em relação ao Tempo Permitido na Lane. A divisão gera a taxa da posição da nota, que é utilizada para calcular a interpolação

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.z < hideNotePosition)                                    // Se a nota passar do TAPZONE e chegar em uma posição configurada em hideNotePosition, ela é ocultada para não poluir a tela
        {
            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * SongManager.Instance.noteSpawnZ, Vector3.forward * SongManager.Instance.NoteDespawnZ, t);
        }

    }
}