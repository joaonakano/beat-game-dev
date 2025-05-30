using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

[CreateAssetMenu(fileName = "LaneSettings", menuName = "Scriptable Objects/LaneSettings")]
public class LaneSettings : ScriptableObject
{
    [Header("Configura��es Principais")]
    [Tooltip("Nota do Midi (A, B, C, D, F ou G) que a Lane ser� encarregada de cuidar")] public NoteName noteRestriction;
    [Tooltip("Chance de spawnar uma nota especial")] public float darkNoteChance = 0.15f;

    [Header("Prefab das Notas - Cores")]
    [Tooltip("Prefab da nota simples")] public GameObject lightNotePrefab;
    [Tooltip("Prefab da nota especial")] public GameObject darkNotePrefab;

    [Header("Outras Configura��es")]
    [Tooltip("Prefab do texto de erro/miss")] public GameObject smallTextPrefab;

    [Tooltip("Prefab da part�cula padr�o utilizada quando uma nota � destru�da")] public ParticleSystem hitParticleEffects;
    [Tooltip("Prefab da part�cula especial quando uma nota especial � destru�da")] public ParticleSystem hitSpecialParticleEffects;
}
