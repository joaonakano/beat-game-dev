using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class NamedAudioSource
{
    public string tag;
    public GameObject audioObject;
    [HideInInspector] public AudioSource audioSource;
}

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources Utilizados")]
    public List<NamedAudioSource> audioSourceList = new();

    [Header("Audio Mixer")]
    public AudioMixer mainMixer; // Arraste o AudioMixer aqui no Inspector

    private Dictionary<string, AudioSource> audioSourceMap;
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSourceMap = new();

        foreach (var entry in audioSourceList)
        {
            // Busca o AudioSource no objeto informado ou em seus filhos
            entry.audioSource = entry.audioObject.GetComponent<AudioSource>();
            if (entry.audioSource == null)
            {
                entry.audioSource = entry.audioObject.GetComponentInChildren<AudioSource>();
            }

            if (entry.audioSource == null)
            {
                Debug.LogError($"[AudioManager] Nenhum AudioSource encontrado no objeto '{entry.tag}'. Verifique se o AudioSource está corretamente atribuído.");
                continue;
            }

            if (!audioSourceMap.ContainsKey(entry.tag))
                audioSourceMap.Add(entry.tag, entry.audioSource);
            else
                Debug.LogWarning($"Audio Source duplicado: {entry.tag}");
        }
    }

    #region Controles de Playback

    public void Play(string tag, AudioClip clip, bool loop = false)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
        {
            source.clip = clip;
            source.loop = loop;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"Não foi possível encontrar o Audio Source com a tag {tag}");
        }
    }

    public void Stop(string tag)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.Stop();
    }

    public void Pause(string tag)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            if (source.isPlaying)
                source.Pause();
    }

    public void Unpause(string tag)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            if (!source.isPlaying)
                source.UnPause();
    }

    public void PlayOneShot(string tag, AudioClip clip)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.PlayOneShot(clip);
        else
        {
            Debug.LogWarning($"Não foi possível encontrar Audio Source com a tag {tag}");
        }
    }

    public void SetVolume(string tag, float volume)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.volume = Mathf.Clamp01(volume);
    }

    public void SetPitch(string tag, float pitch)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.pitch = Mathf.Clamp(pitch, -3, 3);
    }

    public AudioSource GetAudioSource(string tag)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            return source;

        return null;
    }

    #endregion

    #region Controle do Mixer (Master, Music, SFX, Voice)

    // Volume vai de 0.0001 até 1 no slider (nunca 0 porque log10 de 0 dá erro)
    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20);
    }

    #endregion
}
