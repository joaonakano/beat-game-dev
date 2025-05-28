using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    public bool hasEnded = false;
    public bool isPaused = false;
    public double lastNoteTimestamp;

    public AudioSource audioSource;
    public AudioSource crackleAudioSource;
    public AudioClip trainStartClip;

    public double marginOfError;

    public float songDelayInSeconds;
    public int inputDelayInMilliseconds;

    public string fileName;
    public float noteTime;

    public NoteSpawner noteSpawner;

    public int musicNoteCount;

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


    public MidiFile midiFile;

    void Start()
    {
        Instance = this;
        ReadFromFile();
    }

    // Leitura o arquivo Midi
    public void ReadFromFile()
    {
        Debug.Log("Started reading the midi file");
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileName + ".mid");
        Debug.Log("Finished read the midi file");

        Debug.Log("Started getting the data from the midi file");
        GetDataFromMidi();
    }

    // Extração das notas do arquivo Midi e início da música
    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        SetMusicNoteCount(array);
        lastNoteTimestamp = GetLastNoteTimestamp();

        // Passagem da lista de notas extraídas para serem spawnadas na Lane correta
        noteSpawner.SetTimeStamps(array);

        Debug.Log("Finished getting the data from the midi file");
        // Inicio da musica
        // crackleAudioSource.PlayOneShot(trainStartClip);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void SetMusicNoteCount(Melanchall.DryWetMidi.Interaction.Note[] noteArray)
    {
        musicNoteCount = noteArray.Count();
    }

    private void Update()
    {
        double currentAudioTime = System.Math.Round(GetAudioSourceTime(), 3);

        if (currentAudioTime >= lastNoteTimestamp)
        {
            if (audioSource.pitch > 0.2)
            {
                audioSource.pitch -= 0.08f * Time.deltaTime;
                hasEnded = true;
            }
        }

        if (ScoreManager.healthScore == 0)
        {
            if (audioSource.pitch > 0.2)
            {
                audioSource.pitch -= 0.5f * Time.deltaTime;
            }
            else
            {
                if (!HasSongEnded())
                {
                    hasEnded = true;
                }
            }
        }
    }

    public double GetLastNoteTimestamp()
    {
        MetricTimeSpan metricLastTimeStamp = TimeConverter.ConvertTo<MetricTimeSpan>(midiFile.GetDuration(TimeSpanType.Metric), midiFile.GetTempoMap());
        double stamp = (double)metricLastTimeStamp.Minutes * 60f + metricLastTimeStamp.Seconds + (double)metricLastTimeStamp.Milliseconds / 1000f;

        double laststamp = System.Math.Round(stamp, 3);
        return laststamp;
    }

    public void StartSong()
    {
        Debug.Log("Starting SONG!");
        audioSource.Play();
        StartCoroutine(noteSpawner.SpawnNotesCoroutine());
        //crackleAudioSource.Play();
    }

    public void UnpauseSong()
    {
        audioSource.UnPause();
        crackleAudioSource.UnPause();
        isPaused = false;
    }

    public void PauseSong()
    {
        audioSource.Pause();
        crackleAudioSource.Pause();
        isPaused = true;
    }

    public void EndSong()
    {
        audioSource.Stop();
    }

    // Solicitar a minutagem em segundos da música
    public double GetAudioSourceTime()
    {
        return (double)audioSource.timeSamples / audioSource.clip.frequency;
    }

    public bool HasSongEnded()
    {
        if (hasEnded)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void ToggleReverbOnMusic(bool desireEffect)
    {
        if (desireEffect)
        {
            audioSource.GetComponent<AudioReverbFilter>().enabled = true;
        } else
        {
            audioSource.GetComponent<AudioReverbFilter>().enabled = false;
        }
    }
}