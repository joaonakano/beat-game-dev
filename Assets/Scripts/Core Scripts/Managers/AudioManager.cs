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
    [Header("Audio Sources Registrados")]
    public List<NamedAudioSource> audioSourceList = new();

    [Header("Audio Mixer")]
    public AudioMixer mainMixer;

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

        audioSourceMap = new Dictionary<string, AudioSource>();

        foreach (var entry in audioSourceList)
        {
            entry.audioSource = entry.audioObject.GetComponent<AudioSource>();
            if (entry.audioSource == null)
                entry.audioSource = entry.audioObject.GetComponentInChildren<AudioSource>();

            if (entry.audioSource == null)
            {
                Debug.LogError($"AudioManager: Nenhum AudioSource encontrado no objeto '{entry.tag}'.");
                continue;
            }

            if (!audioSourceMap.ContainsKey(entry.tag))
                audioSourceMap.Add(entry.tag, entry.audioSource);
            else
                Debug.LogWarning($"AudioManager: Tag duplicada '{entry.tag}'.");
        }
    }

    #region Playback

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
            Debug.LogWarning($"AudioManager: Tag '{tag}' não encontrada.");
        }
    }

    public void PlayOneShot(string tag, AudioClip clip)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
        {
            source.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"AudioManager: Tag '{tag}' não encontrada.");
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

    public void SetPitch(string tag, float pitch)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.pitch = Mathf.Clamp(pitch, -3f, 3f);
    }

    public AudioSource GetAudioSource(string tag)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            return source;

        return null;
    }

    #endregion

    #region Mixer Volume Controls

    private float ConvertToDecibel(float volume)
    {
        return Mathf.Lerp(-80f, 0f, Mathf.Clamp01(volume));
    }

    public void SetMasterVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", ConvertToDecibel(volume));
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", ConvertToDecibel(volume));
    }

    public void SetSFXVolume(float volume)
    {
        mainMixer.SetFloat("SFXVolume", ConvertToDecibel(volume));
    }

    #endregion
}
