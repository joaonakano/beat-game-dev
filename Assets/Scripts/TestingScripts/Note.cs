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
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));                // Note Time Ratio - Usado para calcular taxa do Tempo de Exist�ncia de uma Nota em rela��o ao Tempo Permitido na Lane. A divis�o gera a taxa da posi��o da nota, que � utilizada para calcular a interpola��o

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