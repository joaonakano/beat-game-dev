using UnityEngine;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime;

    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();        // Armazena a minutagem da música em segundos de quando a classe Note foi instanciada
    }

    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;     // Calcula quanto tempo se passou desde a criação da nota, usando a diferença entre o tempo atual e a origem
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));         // Progress Ratio -> Cálculo da razão entre o tempo desde a instanciação da Nota e o tempo que levará à nota para chegar ao destino, exemplo: 3 (passou 3 segundos desde a criação da nota) / 2 (a nota deve chegar no maximo em 2 segundos ao destino final) = implica que atrasou 1.5 segundos e está há 50% do caminho à frente de destino

        if (t > 1)                  // Se t > 1, quer dizer que a nota passou do destino
        {
            Destroy(gameObject);    // Então, destruí-la
        }
        else                        // Senão,
        {
            transform.localPosition = Vector3.Lerp(Vector3.forward * SongManager.Instance.noteSpawnZ, Vector3.forward * SongManager.Instance.noteDespawnZ, t);  // Configura uma nova posição com base na interpolação entre o ponto de origem e o ponto de destino seguindo uma razão T, o que significa a porcentagem entre a origem e o destino, exemplo 0.75 = 75% do caminho
            GetComponent<MeshRenderer>().enabled = true; // Ativa o Renderer da Nota
        }
    }
}
