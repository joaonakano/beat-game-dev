using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    [Header("Informa��es de M�sica")]
    [Tooltip("Nome do arquivo MIDI")] public string midiFileName;
    [Tooltip("Nome do arquivo de m�sica")] public string songFileName;

    [Header("Audio Clips")]
    [Tooltip("Clip tocado quando uma nova rodada � iniciada")] public AudioClip startingARoundClip;

    [Header("Audio Source")]
    [Tooltip("Nome da tag de m�sica configurada no Script AudioManager.cs")] public string songAudioSourceTag = "song";

    [HideInInspector] public AudioClip song;
    [HideInInspector] public MidiFile midiFile;
    [HideInInspector] public AudioSource songAudioSource;

    [Header("Informa��es de Gameplay")]
    [Tooltip("Tempo permitido para o jogador acertar uma nota fora do tempo em segundos")] public double marginOfError;
    [Tooltip("Tempo de delay at� a m�sica come�ar em segundos")] public float songDelayInSeconds;
    [Tooltip("Tempo de delay dos controles em milisegundos")] public int inputDelayInMilliseconds;
    [Tooltip("Tempo de dura��o que uma nota deve ficar em cena. Quanto menor, mais r�pido a nota viaja.")] public float noteTime;

    [Header("Referencias Obrigat�rias a Outros Scripts")]
    [Tooltip("Refer�ncia ao script de Gerenciamento de Midi (MidiHandler.cs)")] public MidiHandler midiHandler;
    [Tooltip("Refer�ncia ao script de Spawn de Notas (NoteSpawner.cs)")] public NoteSpawner noteSpawner;

    [Header("Informa��es Obrigat�rias de Posi��o")]
    [Tooltip("Posi��o do Spawn das notas no eixo Z")] public float noteSpawnZ;
    [Tooltip("Posi��o da �rea de Tapping no eixo Z")] public float noteTapZ;
    public float NoteDespawnZ
    {
        get
        {
            return noteTapZ - (noteSpawnZ - noteTapZ);
        }
    }

    [Header("Estat�sticas para Nerds")]
    public bool hasEnded = false;
    public bool isPaused = false;
    public double lastNoteTimestamp;
    public int musicNoteCount;

    void Start()
    {
        Instance = this;

        LoadSongAudioClip(songFileName);

        if (songAudioSource == null)
            songAudioSource = GetSongAudioSource(songAudioSourceTag);

        // Game Start
        ReadMidiFile();
    }

    private string FormatFileName(string filename, string beginningArgs = "", string endArgs = "")
    {
        string newStr = beginningArgs + filename.Trim() + endArgs;
        return newStr;
    }

    private void LoadSongAudioClip(string filename)
    {
        string path = $"Audio/Songs/{filename}";

        song = Resources.Load<AudioClip>(path);

        if (song == null)
            Debug.LogError($"Erro ao tentar carregar a m�sica: {songFileName} de {path}");
        else
            Debug.Log($"M�sica carregada: {songFileName}");
    }

    private AudioSource GetSongAudioSource(string tag)
    {
        AudioSource songAudioSource = AudioManager.Instance.GetAudioSource(tag);

        if (songAudioSource == null)
            Debug.LogError($"Falha ao tentar procurar a tag especificada no AudioManager.cs: {tag}. Verifique se existe a tag {tag} no Script/Componente.");

        return songAudioSource;
    }

    public void ReadMidiFile()
    {
        if (!midiFileName.Contains(".mid"))
            midiFileName = FormatFileName(midiFileName, endArgs: ".mid");
                
        Debug.Log($"Started reading the midi file: {midiFileName}");

        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/Audio/Notes/" + midiFileName);

        Debug.Log($"Finished read the midi file: {midiFileName}");

        midiHandler.GetDataFromMidiAndParseNotes(midiFile);

        lastNoteTimestamp = midiHandler.GetLastNoteTimestamp(midiFile);
        musicNoteCount = midiHandler.GetSongNoteCount();

        // Inicio da musica
        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        Debug.Log("Starting SONG!");

        AudioManager.Instance.Play("song", song);

        StartCoroutine(noteSpawner.SpawnNotesCoroutine(midiHandler.noteDataList));
    }

    // Solicitar a minutagem em segundos da m�sica
    public double GetAudioSourceTime()
    {
        if (songAudioSource == null) return 0;

        return (double)songAudioSource.timeSamples / songAudioSource.clip.frequency;
    }

    public bool HasSongEnded()
    {
        return hasEnded;
    }

    public bool HasSongBeenPaused()
    {
        return (!songAudioSource.isPlaying? true : false);
    }

    public bool IsGameRunning()
    {
        return !HasSongEnded() && !HasSongBeenPaused() && HealthManager.Instance.CurrentHealth > 0;
    }

    public void ToggleReverbOnMusic(bool desiredEffect)
    {
        if (desiredEffect)
        {
            songAudioSource.GetComponent<AudioReverbFilter>().enabled = true;
        } else
        {
            songAudioSource.GetComponent<AudioReverbFilter>().enabled = false;
        }
    }

    private void Update()
    {
        double currentAudioTime = System.Math.Round(GetAudioSourceTime(), 3);

        /* (NAO SEI O QUE FAZ ISSO, TALVEZ DE PRA DELETAR)
        if (currentAudioTime >= lastNoteTimestamp)
        {
            //if (audioSource.pitch > 0.2)
            //{
            //    audioSource.pitch -= 0.08f * Time.deltaTime;
            //    hasEnded = true;
            //}
        } */

        // (FAZER DEPOIS UMA CORROTINA QUE DIMINUI O PITCH NO AUDIOMANAGER)
        if (HealthManager.Instance.CurrentHealth == 0)
        {
            if (songAudioSource.pitch > 0.2)
            {
                songAudioSource.pitch -= 0.5f * Time.deltaTime;
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
}