using System;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.PackageManager;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public AudioSource hitAudioSource;
    public AudioSource missAudioSource;
    public AudioSource voicelinesAudioSource;

    static AudioClip endClip;
    static AudioClip loseClip;
    static AudioClip milestoneClip;

    static List<AudioClip> superMissClips;
    static List<AudioClip> missesClips;
    static List<AudioClip> hitClips;
    static List<AudioClip> voicelines;
    static List<AudioClip> wrongPressMissClips;

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

    [SerializeField]
    private List<AudioClip> superMissesSFX;

    [SerializeField]
    private List<AudioClip> missesSFX;

    [SerializeField]
    private List<AudioClip> wrongPressMissSFX;

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

        superMissClips = superMissesSFX;
        missesClips = missesSFX;
        wrongPressMissClips = wrongPressMissSFX;

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

        if (!alreadyPlayedEndingSFX && SongManager.HasSongEnded())
        {
            End();

            if (healthScore == 0)
            {
                Invoke(nameof(Lose), 2f);
            }
        }
        
        if (comboScore >= 10 && comboScore % 20 == 0 && comboScore > lastMilestone)
        {
            lastMilestone = comboScore;
            PlayMilestone();
            ActivateVoiceline();
            Debug.Log($"New milestone: {lastMilestone}");
        }
    }

    // HIT COUNT LOGIC
    public static void Hit()
    {
        comboScore += 1;

        var randIndex = UnityEngine.Random.Range(0, hitClips.Count);

        ShakeManager.instance.HitShake();

        float missingHealthPercentage = (float)(100 - healthScore) / 100f;
        float baseHeal = 5.5f;
        float dynamicHeal = baseHeal * missingHealthPercentage;

        healthScore += dynamicHeal;                                         // Arrumar depois e colocar um valor melhor de pontos de vida
        healthScore = Math.Clamp(healthScore, 0, 100);

        Instance.hitAudioSource.PlayOneShot(hitClips[randIndex]);
    }

    // MISS COUNT LOGIC
    public static void Miss()
    {
        comboScore = 0;
        missedNotesScore += 1;

        AudioClip clipToPlay;
        var randIndex = UnityEngine.Random.Range(0, missesClips.Count);

        clipToPlay = missesClips[randIndex];

        healthScore -= 5.5;                                                 // Arrumar depois e colocar um valor melhor de pontos de dano
        healthScore = Math.Clamp(healthScore, 0, 100);

        Instance.missAudioSource.PlayOneShot(clipToPlay);
    }

    // SUPER NOTE MISS LOGIC
    public static void SuperMiss()
    {
        comboScore = 0;
        missedNotesScore += 2;

        AudioClip clipToPlay;
        var randIndex = UnityEngine.Random.Range(0, superMissClips.Count);

        healthScore -= 10.5;                                               // Arrumar depois e colocar um valor melhor de pontos de dano
        healthScore = Math.Clamp(healthScore, 0, 100);

        clipToPlay = superMissClips[randIndex];
        Instance.missAudioSource.PlayOneShot(clipToPlay);
    }
    
    // TOO EARLY MISS LOGIC
    public static void WrongPressMiss()
    {
        comboScore = 0;

        healthScore -= 5;
        healthScore = Math.Clamp(healthScore, 0, 100);

        var randIndex = UnityEngine.Random.Range(0, wrongPressMissClips.Count);
        Instance.missAudioSource.PlayOneShot(wrongPressMissClips[randIndex]);
    }

    // PLAYING MATCH LOSE AUDIO
    public void Lose()
    {
        Instance.missAudioSource.PlayOneShot(loseClip);
    }

    // MATCH ENDING LOGIC
    public static void End()
    {
        Instance.missAudioSource.clip = endClip;
        Instance.missAudioSource.Play();
        alreadyPlayedEndingSFX = true;
    }

    // PLAYING MILESTONE AUDIO
    public static void PlayMilestone()
    {
        Instance.hitAudioSource.PlayOneShot(milestoneClip);
    }

    // VOICELINES PLAY LOGIC
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