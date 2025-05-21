using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();        // Armazena a minutagem da m�sica em segundos de quando a classe Note foi instanciada
    }

    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;     // Calcula quanto tempo se passou desde a cria��o da nota, usando a diferen�a entre o tempo atual e a origem
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));         // Progress Ratio -> C�lculo da raz�o entre o tempo desde a instancia��o da Nota e o tempo que levar� � nota para chegar ao destino, exemplo: 3 (passou 3 segundos desde a cria��o da nota) / 2 (a nota deve chegar no maximo em 2 segundos ao destino final) = implica que atrasou 1.5 segundos e est� h� 50% do caminho � frente de destino

        if (t > 1)                  // Se t > 1, quer dizer que a nota passou do destino
        {
            Destroy(gameObject);    // Ent�o, destru�-la
        }
        else                        // Sen�o,
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * SongManager.Instance.noteSpawnZ, Vector3.forward * SongManager.Instance.noteDespawnZ, t);  // Configura uma nova posi��o com base na interpola��o entre o ponto de origem e o ponto de destino seguindo uma raz�o T, o que significa a porcentagem entre a origem e o destino, exemplo 0.75 = 75% do caminho
            GetComponent<MeshRenderer>().enabled = true; // Ativa o Renderer da Nota
        }
    }
}
