using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedAudioSource
{
    public string tag;
    public AudioSource audioSource;
}

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources Utilizados")]
    public List<NamedAudioSource> audioSourceList = new();

    private Dictionary<string, AudioSource> audioSourceMap;
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        audioSourceMap = new();

        foreach (var entry in audioSourceList)
        {
            if (!audioSourceMap.ContainsKey(entry.tag))
                audioSourceMap.Add(entry.tag, entry.audioSource);
            else
                Debug.LogWarning($"Audio Source duplicado: {entry.tag}");
        }
    }

    /// <summary>
    /// Metodo que tocara um clip no AudioSource correspondente a tag
    /// </summary>
    /// <param name="tag">audiosource name</param>
    /// <param name="clip">clip</param>
    /// <param name="loop">loop</param>
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

    /// <summary>
    /// Metodo que para de tocar o clip no AudioSource correspondente a tag
    /// </summary>
    /// <param name="tag">audiosource name</param>
    public void Stop(string tag)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.Stop();
    }

    /// <summary>
    /// Metodo que pausa um clip no AudioSource correspondente a tag
    /// </summary>
    /// <param name="tag">audiosource name</param>
    public void Pause(string tag)
    {
        if (audioSourceMap.TryGetValue(tag,out var source))
            if (source.isPlaying)
                source.Pause();
    }

    /// <summary>
    /// Metodo que retoma um clip pausado no AudioSource correspondente a tag
    /// </summary>
    /// <param name="tag">audiosource name</param>
    public void Unpause(string tag)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            if (!source.isPlaying)
                source.UnPause();
    }

    /// <summary>
    /// Metodo que tocara um clip apenas uma vez no AudioSource correspondente a tag
    /// </summary>
    /// <param name="tag">audiosource name</param>
    /// <param name="clip">clip</param>
    public void PlayOneShot(string tag, AudioClip clip)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.PlayOneShot(clip);
        else
        {
            Debug.LogWarning($"Não foi possível encontrar Audio Source com a tag {tag}");
        }
    }

    /// <summary>
    /// Metodo que altera o volume de um AudioSource correspondente a tag
    /// </summary>
    /// <param name="tag">audiosource name</param>
    /// <param name="volume">volume</param>
    public void SetVolume(string tag, float volume)
    {
        if (audioSourceMap.TryGetValue(tag, out var source))
            source.volume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// Metodo que altera o pitch de um AudioSource correspondente a tag
    /// </summary>
    /// <param name="tag">audiosource name</param>
    /// <param name="pitch">pitch</param>
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
}
