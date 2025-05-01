using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError;

    public int inputDelayInMilliseconds;

    public string fileName;
    public float noteTime;
    
    // PRINCIPAIS ÁREAS DE EVENTO DAS NOTAS (Spawn, Despawn e Tap)
    public float noteSpawnZ;
    public float noteTapZ;
    public float noteDespawnZ
    {
        get
        {
            return noteTapZ - (noteSpawnZ - noteTapZ);
        }
    }

    public static MidiFile midiFile;

    void Start()
    {
        Instance = this;
        ReadFromFile();
    }

    // Leitura o arquivo Midi
    public void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileName + ".mid");
        GetDataFromMidi();
    }

    // Extração das notas do arquivo Midi e início da música
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        // Passagem da lista de notas extraídas para serem spawnadas na Lane correta
        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
        Debug.Log(notes.Count);
    }

    public void StartSong()
    {
        audioSource.Play();
    }

    // Solicitar a minutagem em segundos da música
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}