using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    // Instance
    public static ScoreManager Instance;

    // Input Variables
    public KeyCode specialToggleKey = KeyCode.T;

    // Audio Source Variables
    public AudioSource hitAudioSource;
    public AudioSource missAudioSource;
    public AudioSource voicelinesAudioSource;

    [SerializeField]
    private AudioClip endClips;
    
    [SerializeField]
    private AudioClip milestoneClips;

    [SerializeField]
    private AudioClip loseClips;

    [SerializeField]
    private List<AudioClip> hitsClips;

    [SerializeField]
    private List<AudioClip> voicelinesClips;

    [SerializeField]
    private List<AudioClip> superMissesClips;

    [SerializeField]
    private List<AudioClip> missesClips;

    [SerializeField]
    private List<AudioClip> wrongPressMissClips;

    [SerializeField]
    private List<AudioClip> superScoreActivation;

    [SerializeField]
    private List<AudioClip> superScoreActive;

    // Text Variables
    public TMP_Text scoreText;
    public static int comboScore;

    public TMP_Text specialPercentageText;
    public static double specialPercentageScore;

    public TMP_Text missedText;
    public static int missedNotesScore;

    public TMP_Text healthText;
    public static double healthScore;

    // Score Varibles
    public int comboConstantIncrement = 1;
    public int milestoneStep = 100;
    public static int lastMilestone;
    private double specialComboRatio;
    private double decreaseAmount = 100;
    private double duration = 5.0;
    private double elapsedTime = 0;

    // Index Variables
    static int voicelineIndex = 0;

    // Control Boolean Variables
    private bool alreadyPlayedEndingSFX = false;
    private bool alreadyPlayedSpecialSFX = false;
    private bool isDecreasing = false;

    void Start()
    {
        Instance = this;

        // Initial Variable Values
        lastMilestone = 0;
        comboScore = 0;
        missedNotesScore = 0;
        specialPercentageScore = 0.0;
        healthScore = 100.0;
        specialComboRatio = (1 / (SongManager.musicNoteCount * 0.3)) * 100.0;
    }

    void Update()
    {
        // Changing text
            // Top
        scoreText.text = comboScore.ToString();
        missedText.text = missedNotesScore.ToString();
        specialPercentageText.text = $"{specialPercentageScore:F0}%";
            // Bottom
        healthText.text = $"{healthScore:F2}%";

        // End match or Lose Match
        if (!alreadyPlayedEndingSFX && SongManager.HasSongEnded())
        {
            Invoke(nameof(PlayEndingSFX), 0);

            if (healthScore == 0)
            {
                Invoke(nameof(PlayLoserSFX), 2f);
            }
        }
        
        // If the special is ready...
        if (specialPercentageScore >= 100.0 && !alreadyPlayedSpecialSFX)
        {
            PlayRandomSFXFromList(superScoreActivation, missAudioSource);
            Debug.Log("Special is Ready!");
            alreadyPlayedSpecialSFX = true;
        }

        // If the special is ready and it was toggled, then...
        if (Input.GetKeyDown(specialToggleKey) && specialPercentageScore == 100.0)
        {
            PlayRandomSFXFromList(superScoreActive, missAudioSource);
            ToggleSpecial(10);
            SongManager.ToggleReverbOnMusic(true);
            FullscreenTestController.SetCRT((float)duration);
        }

        // If it was toggled the special condition, then increases the combo const
        if (isDecreasing)
        {
            double amountPerSecond = decreaseAmount / duration;
            double delta = amountPerSecond * Time.deltaTime;

            specialPercentageScore -= delta;
            elapsedTime += Time.deltaTime;

            specialPercentageScore = Math.Clamp(specialPercentageScore, 0.0, 100.0);

            if (elapsedTime >= duration)
            {
                ToggleSpecial(1);
                alreadyPlayedSpecialSFX = false;
                SongManager.ToggleReverbOnMusic(false);
            }
        }
 
        // Check milestones
        while (comboScore >= lastMilestone + milestoneStep)
        {
            lastMilestone += milestoneStep;
            PlayMilestone();
            ActivateVoiceline();
            Debug.Log($"New milestone: {lastMilestone}");
        }
    }


    // HIT COUNT LOGIC
    public static void Hit()
    {
        comboScore += Instance.comboConstantIncrement;

        ShakeManager.instance.HitShake();

        Instance.AddToSuperScoreIfNormal(Instance.specialComboRatio);
        Instance.DynamicHealthHealing(5.5f);
        Instance.PlayRandomSFXFromList(Instance.hitsClips, Instance.hitAudioSource);
    }

    // DARK NOTE HIT COUNT LOGIC
    public static void SuperHit()
    {
        comboScore += Instance.comboConstantIncrement * 3;

        ShakeManager.instance.HitShake();

        Instance.AddToSuperScoreIfNormal(Instance.specialComboRatio * 1.5);
        Instance.DynamicHealthHealing(10f);
        Instance.PlayRandomSFXFromList(Instance.hitsClips, Instance.hitAudioSource);            // Adicionar novos sons para super
    }

    // MISS COUNT LOGIC
    public static void Miss()
    {
        comboScore = 0;
        missedNotesScore += 1;

        Instance.SetSuperScoreIfNormal(0f);
        Instance.DynamicHealthDamaging(5.5f);
        Instance.PlayRandomSFXFromList(Instance.missesClips, Instance.missAudioSource);
    }

    // SUPER NOTE MISS LOGIC
    public static void SuperMiss()
    {
        comboScore = 0;
        missedNotesScore += 2;

        Instance.SetSuperScoreIfNormal(0f);
        Instance.DynamicHealthDamaging(10.5f);
        Instance.PlayRandomSFXFromList(Instance.superMissesClips, Instance.missAudioSource);
    }
    
    // TOO EARLY MISS LOGIC
    public static void WrongPressMiss()
    {
        comboScore = 0;

        Instance.SetSuperScoreIfNormal(0f);
        Instance.DynamicHealthDamaging(4f);
        Instance.PlayRandomSFXFromList(Instance.wrongPressMissClips, Instance.missAudioSource);
    }

    // MATCH ENDING LOGIC


    // (UTILITIES) - AUDIO PLAYERS
    private void PlayOneSFX(AudioClip clip, AudioSource audioSource)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private void PlayRandomSFXFromList(List<AudioClip> clips, AudioSource audiosource)
    {
        int randIndex = UnityEngine.Random.Range(0, clips.Count);
        audiosource.PlayOneShot(clips[randIndex]);
    }

    private void PlayOneShotSFX(AudioClip clip, AudioSource audioSource)
    {
        audioSource.PlayOneShot(clip);
    }

    private void PlayEndingSFX()
    {
        alreadyPlayedEndingSFX = true;
        PlayOneSFX(endClips, missAudioSource);
    }

    private void PlayLoserSFX()
    {
        PlayOneShotSFX(loseClips, missAudioSource);
    }

    public void PlayMilestone()
    {
        PlayOneShotSFX(milestoneClips, hitAudioSource);
    }

    public static void ActivateVoiceline()
    {
        if (voicelineIndex >= Instance.voicelinesClips.Count)
        {
            voicelineIndex = 0;
        }

        Instance.PlayOneSFX(Instance.voicelinesClips[voicelineIndex], Instance.voicelinesAudioSource);
        voicelineIndex++;
    }


    // (UTILITIES) - HEALTH MODIFIERS
    private void DynamicHealthHealing(float baseHeal)
    {
        float missingHealthPercentage = (100f - (float)healthScore) / 100f;
        float dynamicHeal = baseHeal * missingHealthPercentage;

        healthScore += dynamicHeal;
        healthScore = Math.Clamp(healthScore, 0, 100);
    }

    private void DynamicHealthDamaging(float damagePoints)
    {
        healthScore -= damagePoints;
        healthScore = Math.Clamp(healthScore, 0, 100);
    }


    // (UTILITIES) - SCORE MODIFIERS
    public void AddToSuperScoreIfNormal(double addRatio)
    {
        if (!isDecreasing)
        {
            specialPercentageScore += addRatio;
            specialPercentageScore = Math.Clamp(specialPercentageScore, 0.0, 100.0);
        }
    }

    public void SetSuperScoreIfNormal(float desiredScore)
    {
        if (!isDecreasing)
        {
            specialPercentageScore = desiredScore;
        }
    }

    private void ToggleSpecial(int desiredIncrement = 1)
    {
        if (isDecreasing)
        {
            isDecreasing = false;
        } else
        {
            isDecreasing = true;
        }

        comboConstantIncrement = desiredIncrement;
        elapsedTime = 0;
    }
}