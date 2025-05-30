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
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));                 // Note Time Ratio - Usado para calcular taxa do Tempo de Exist�ncia de uma Nota em rela��o ao Tempo Permitido na Lane. A divis�o gera a taxa da posi��o da nota, que � utilizada para calcular a interpola��o

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else if (gameObject.transform.position.z < hideNotePosition)                                    // Se a nota passar do TAPZONE e chegar em uma posi��o configurada em hideNotePosition, ela � ocultada para n�o poluir a tela
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