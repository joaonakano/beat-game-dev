using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public AudioSource hitAudioSource;
    public AudioSource missAudioSource;
    public AudioSource voicelinesAudioSource;

    static AudioClip endClip;
    static AudioClip loseClip;
    static AudioClip milestoneClip;
    static List<AudioClip> hitClips;
    static List<AudioClip> voicelines;

    static bool alreadyPlayedEndingSFX = false;

    [SerializeField]
    private AudioClip endSFX;
    
    [SerializeField]
    private AudioClip milestoneSFX;

    [SerializeField]
    private AudioClip loseSFX;

    [SerializeField]
    private List<AudioClip> hitsSFX;

    [SerializeField]
    private List<AudioClip> voicelinesSFX;

    public TMP_Text scoreText;
    public static int comboScore;

    public TMP_Text missedText;
    public static int missedNotesScore;

    public TMP_Text healthText;
    public static double healthScore;

    public static int lastMilestone;

    static int voicelineIndex = 0;


    void Start()
    {
        Instance = this;

        lastMilestone = 0;
        comboScore = 0;
        missedNotesScore = 0;
        healthScore = 100.0;

        loseClip = loseSFX;
        endClip = endSFX;
        hitClips = hitsSFX;
        voicelines = voicelinesSFX;
        milestoneClip = milestoneSFX;
    }

    void Update()
    {
        scoreText.text = comboScore.ToString();
        missedText.text = missedNotesScore.ToString();
        healthText.text = $"{healthScore:F2}%";

        if (SongManager.HasSongEnded() && !alreadyPlayedEndingSFX)
        {
            End();
        }

        if (comboScore >= 10 && comboScore % 20 == 0 && comboScore > lastMilestone)
        {
            lastMilestone = comboScore;
            PlayMilestone();
            ActivateVoiceline();
            Debug.Log($"New milestone: {lastMilestone}");
        }

        if (healthScore == 0 && !SongManager.HasSongEnded()) Lose();
    }

    public static void Hit()
    {
        comboScore += 1;

        var randIndex = UnityEngine.Random.Range(0, hitClips.Count);

        ShakeManager.instance.HitShake();

        healthScore += 15.52;                                               // Arrumar depois e colocar um valor melhor de pontos de vida
        healthScore = Math.Clamp(healthScore, 0, 100);

        Instance.hitAudioSource.PlayOneShot(hitClips[randIndex]);
    }

    public static void Miss()
    {
        comboScore = 0;
        missedNotesScore += 1;

        healthScore -= 10.52;                                               // Arrumar depois e colocar um valor melhor de pontos de dano
        healthScore = Math.Clamp(healthScore, 0, 100);

        Instance.missAudioSource.Play();
    }

    public static void Lose()
    {
        Instance.missAudioSource.clip = loseClip;
        Instance.missAudioSource.Play();
    }

    public static void End()
    {
        Instance.missAudioSource.clip = endClip;
        Instance.missAudioSource.Play();
        alreadyPlayedEndingSFX = true;
    }

    public static void PlayMilestone()
    {
        Instance.hitAudioSource.PlayOneShot(milestoneClip);
    }

    public static void ActivateVoiceline()
    {
        if (voicelineIndex >= voicelines.Count)
        {
            voicelineIndex = 0;
        }

        Instance.voicelinesAudioSource.clip = voicelines[voicelineIndex];
        Instance.voicelinesAudioSource.Play();
        voicelineIndex++;
    }
}