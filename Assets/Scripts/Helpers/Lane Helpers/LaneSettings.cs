using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

[CreateAssetMenu(fileName = "LaneSettings", menuName = "Scriptable Objects/LaneSettings")]
public class LaneSettings : ScriptableObject
{
    public NoteName noteRestriction;

    public float darkNoteChance = 0.15f;

    public GameObject lightNotePrefab;
    public GameObject darkNotePrefab;

    public GameObject smallTextPrefab;

    public ParticleSystem hitParticleEffects;
}
