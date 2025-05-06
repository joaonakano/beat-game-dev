using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public static bool hasEnded = false;
    public static double lastNoteTimestamp;

    public AudioSource audioSource;

    public Lane[] lanes;

    public double marginOfError;

    public float songDelayInSeconds;
    public int inputDelayInMilliseconds;

    public string fileName;
    public float noteTime;

    public static int musicNoteCount;

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

        SetMusicNoteCount(array);
        lastNoteTimestamp = GetLastNoteTimestamp();

        // Passagem da lista de notas extraídas para serem spawnadas na Lane correta
        foreach (var lane in lanes) lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public static void SetMusicNoteCount(Melanchall.DryWetMidi.Interaction.Note[] noteArray)
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
                audioSource.pitch -= 0.2f * Time.deltaTime;
            }
            else
            {
                // SongManager.Instance.audioSource.pitch = 0;
                if (!HasSongEnded())
                {
                    hasEnded = true;
                }
            }
        }
    }

    public static double GetLastNoteTimestamp()
    {
        MetricTimeSpan metricLastTimeStamp = TimeConverter.ConvertTo<MetricTimeSpan>(midiFile.GetDuration(TimeSpanType.Metric), midiFile.GetTempoMap());
        double stamp = (double)metricLastTimeStamp.Minutes * 60f + metricLastTimeStamp.Seconds + (double)metricLastTimeStamp.Milliseconds / 1000f;

        double laststamp = System.Math.Round(stamp, 3);
        return laststamp;
    }

    public void StartSong()
    {
        audioSource.Play();
    }

    public static void EndSong()
    {
        Instance.audioSource.Stop();
    }

    // Solicitar a minutagem em segundos da música
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    public static bool HasSongEnded()
    {
        if (hasEnded)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public static void ToggleReverbOnMusic(bool desireEffect)
    {
        if (desireEffect)
        {
            Instance.audioSource.GetComponent<AudioReverbFilter>().enabled = true;
        } else
        {
            Instance.audioSource.GetComponent<AudioReverbFilter>().enabled = false;
        }
    }
}