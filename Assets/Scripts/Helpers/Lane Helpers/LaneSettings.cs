using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

[CreateAssetMenu(fileName = "LaneSettings", menuName = "Scriptable Objects/LaneSettings")]
public class LaneSettings : ScriptableObject
{
    [Header("Configurações Principais")]
    [Tooltip("Nota do Midi (A, B, C, D, F ou G) que a Lane será encarregada de cuidar")] public NoteName noteRestriction;
    [Tooltip("Chance de spawnar uma nota especial")] public float darkNoteChance = 0.15f;

    [Header("Prefab das Notas - Cores")]
    [Tooltip("Prefab da nota simples")] public GameObject lightNotePrefab;
    [Tooltip("Prefab da nota especial")] public GameObject darkNotePrefab;

    [Header("Outras Configurações")]
    [Tooltip("Prefab do texto de erro/miss")] public GameObject smallTextPrefab;

    [Tooltip("Prefab da partícula padrão utilizada quando uma nota é destruída")] public ParticleSystem hitParticleEffects;
    [Tooltip("Prefab da partícula especial quando uma nota especial é destruída")] public ParticleSystem hitSpecialParticleEffects;
}
