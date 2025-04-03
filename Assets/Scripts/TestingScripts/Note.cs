using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    void Start ()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    void Update ()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));                // Note Time Ratio - Usado para calcular taxa do Tempo de Existência de uma Nota em relação ao Tempo Permitido na Lane. A divisão gera a taxa da posição da nota, que é utilizada para calcular a interpolação

        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * SongManager.Instance.noteSpawnZ, Vector3.forward * SongManager.Instance.noteDespawnZ, t);
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}