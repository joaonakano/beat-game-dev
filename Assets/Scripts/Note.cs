using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;

    public bool isSpecialNote;
    public double assignedTime;
    public Lane assignedLane;

    public bool hasBeenProcessed = false;

    private float hideZ => SongManager.Instance.noteHideZ;
    private float despawnZ => SongManager.Instance.noteDespawnZ;
    private float tapZ => SongManager.Instance.noteTapZ;
    private float spawnZ => SongManager.Instance.noteSpawnZ;

    void Start()
    {
        timeInstantiated = SongManager.Instance.GetAudioSourceTime();
    }

    void Update()
    {
        double timeSinceInstantiated = SongManager.Instance.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));                 // Note Time Ratio - Usado para calcular taxa do Tempo de Existência de uma Nota em relação ao Tempo Permitido na Lane. A divisão gera a taxa da posição da nota, que é utilizada para calcular a interpolação

        transform.localPosition = Vector3.Lerp(
            Vector3.forward * spawnZ,
            Vector3.forward * tapZ,
            t
        );

        float currentZ = transform.localPosition.z;

        if (currentZ < hideZ)
        {
            foreach (var renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = false;
            }
        }

        if (currentZ < despawnZ)
        {
            Destroy(gameObject);
        }
    }
}