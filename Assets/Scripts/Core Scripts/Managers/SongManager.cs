using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    private Coroutine noteSpawningCoroutine;

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
    public float NoteDespawnZ => noteTapZ - (noteSpawnZ - noteTapZ);

    [Header("Estat�sticas para Nerds")]
    public bool hasEnded = false;
    public bool isPaused = false;
    public double lastNoteTimestamp;
    public int musicNoteCount;
    public bool theGameHasStarted = false;

    public event Action OnSongEnded;

    #region Unity Lifecycle

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadSongAudioClip(songFileName);

        EnsureAudioSource();

        ReadMidiFile();
    }

    void Update()
    {
        double currentAudioTime = System.Math.Round(GetAudioSourceTime(), 3);

        if (HealthManager.Instance != null && HealthManager.Instance.CurrentHealth == 0)
        {
            if (noteSpawningCoroutine != null)
                StopCoroutine(noteSpawningCoroutine);

            if (IsSongAudioSourceValid())
            {
                if (songAudioSource.pitch > 0.2f)
                {
                    songAudioSource.pitch -= 0.5f * Time.deltaTime;
                }
                else
                {
                    if (!hasEnded)
                        hasEnded = true;
                }
            }
        }

        if (theGameHasStarted && GetAudioSourceTime() >= lastNoteTimestamp && HealthManager.Instance.CurrentHealth > 0)
        {
            hasEnded = true;
            OnSongEnded?.Invoke();
        }
    }

    #endregion

    #region Audio Management

    private void EnsureAudioSource()
    {
        if (songAudioSource == null)
            songAudioSource = GetSongAudioSource(songAudioSourceTag);
    }

    private bool IsSongAudioSourceValid()
    {
        if (songAudioSource == null)
            songAudioSource = GetSongAudioSource(songAudioSourceTag);

        return songAudioSource != null;
    }

    private AudioSource GetSongAudioSource(string tag)
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogError("[SongManager] AudioManager.Instance est� nulo. Verifique a ordem de execu��o dos scripts.");
            return null;
        }

        AudioSource source = AudioManager.Instance.GetAudioSource(tag);

        if (source == null)
            Debug.LogError($"[SongManager] Falha ao encontrar a tag '{tag}' no AudioManager.");

        return source;
    }

    public void StartSong()
    {
        Debug.Log("Starting SONG!");
        theGameHasStarted = true;

        EnsureAudioSource();

        if (AudioManager.Instance != null)
            AudioManager.Instance.Play(songAudioSourceTag, song);

        noteSpawningCoroutine = StartCoroutine(noteSpawner.SpawnNotesCoroutine(midiHandler.noteDataList));
    }

    public void ToggleReverbOnMusic(bool desiredEffect)
    {
        if (IsSongAudioSourceValid())
        {
            var reverb = songAudioSource.GetComponent<AudioReverbFilter>();
            if (reverb != null)
                reverb.enabled = desiredEffect;
        }
    }

    #endregion

    #region Song Handling

    private string FormatFileName(string filename, string beginningArgs = "", string endArgs = "")
    {
        return beginningArgs + filename.Trim() + endArgs;
    }

    private void LoadSongAudioClip(string filename)
    {
        string path = $"Audio/Songs/{filename}";

        song = Resources.Load<AudioClip>(path);

        if (song == null)
            Debug.LogError($"[SongManager] Erro ao tentar carregar a m�sica: {songFileName} de {path}");
        else
            Debug.Log($"[SongManager] M�sica carregada: {songFileName}");
    }

    public void ReadMidiFile()
    {
        if (!midiFileName.Contains(".mid"))
            midiFileName = FormatFileName(midiFileName, endArgs: ".mid");

        Debug.Log($"[SongManager] Lendo arquivo MIDI: {midiFileName}");

        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/Audio/Notes/" + midiFileName);

        Debug.Log($"[SongManager] Leitura finalizada do MIDI: {midiFileName}");

        midiHandler.GetDataFromMidiAndParseNotes(midiFile);

        lastNoteTimestamp = midiHandler.GetLastNoteTimestamp(midiFile);
        musicNoteCount = midiHandler.GetSongNoteCount();

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    #endregion

    #region Getters

    public double GetAudioSourceTime()
    {
        if (!IsSongAudioSourceValid() || songAudioSource.clip == null)
            return 0;

        return (double)songAudioSource.timeSamples / songAudioSource.clip.frequency;
    }

    public double GetAudioLastNoteTime()
    {
        return midiHandler.GetLastNoteTimestamp(midiFile);
    }

    public bool HasSongEnded()
    {
        return hasEnded;
    }

    public bool HasSongBeenPaused()
    {
        if (!IsSongAudioSourceValid())
        {
            Debug.LogWarning("[SongManager] songAudioSource est� nulo ao verificar pausa.");
            return true;
        }

        return !songAudioSource.isPlaying;
    }

    public bool IsGameRunning()
    {
        bool healthOk = HealthManager.Instance != null && HealthManager.Instance.CurrentHealth > 0;
        bool audioOk = IsSongAudioSourceValid() && !HasSongBeenPaused();

        return !HasSongEnded() && audioOk && healthOk;
    }

    #endregion
}
