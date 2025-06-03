using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Configurações de Placar")]
    [SerializeField] private double specialNoteDamage = 15.5;
    [SerializeField] private double normalNoteDamage = 10.5;
    [SerializeField] private double wrongPressDamage = 4.5;
    [SerializeField] private double normalHeal = 5.0;
    [SerializeField] private double specialHeal = 10.0;

    [Header("Dependências")]
    [SerializeField] private ScoreTracker scoreTracker;
    [SerializeField] private HealthManager healthManager;
    [SerializeField] private VoicelineManager voicelineManager;

    [Header("Tags dos Audio Sources")]
    public string interactionsAudioSource;
    public string voicelineAudioSource;

    [Header("Elementos de UI")]
    public TMP_Text scoreText;
    public TMP_Text specialText;
    public TMP_Text missedText;
    public TMP_Text healthText;

    [Header("SFX")]
    [SerializeField] private AudioClip milestoneSFX;
    [SerializeField] private AudioClip superReadySFX;
    [SerializeField] private AudioClip endSFX;
    [SerializeField] private AudioClip loseSFX;
    [SerializeField] private AudioClip superActiveSFX;
    [SerializeField] private List<AudioClip> hitSFX;
    [SerializeField] private List<AudioClip> missSFX;
    [SerializeField] private List<AudioClip> wrongPressSFX;
    [SerializeField] private List<AudioClip> superMissSFX;
    [SerializeField] private List<AudioClip> superHitSFX;
    [SerializeField] private List<AudioClip> darkNoteHitSFX;

    public bool IsSuperActive() => isSuperActive;
    bool isSuperActive = false;

    private bool hasPlayedSuperReadySFX = false;
    private bool hasPlayedEndSFX = false;

    float superDuration = 5f;
    float superElapsed = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        scoreTracker.OnMilestoneReached += HandleMilestone;
        scoreTracker.Initialize(SongManager.Instance.musicNoteCount);

        InputManager.Instance.OnSuperScoreKeybindPressed += TryActivateSuper;
    }

    void Update()
    {
        UpdateUI();

        if (SongManager.Instance.HasSongEnded() && !hasPlayedEndSFX)
            EndGameSequence();

        if (!isSuperActive && scoreTracker.Special >= 100.0 && !hasPlayedSuperReadySFX)
        {
            Debug.Log("[SYSTEM ALERT] - Especial Saindo!");
            PlaySFX(superReadySFX, "interaction");
            hasPlayedSuperReadySFX = true;
        }

        if (isSuperActive)
            UpdateSuperState();
    }

    private void UpdateUI()
    {
        scoreText.text = scoreTracker.Combo.ToString();
        missedText.text = scoreTracker.Misses.ToString();
        specialText.text = $"{scoreTracker.Special:F0}%";
        healthText.text = $"{healthManager.CurrentHealth:F2}%";
    }

    private void EndGameSequence()
    {
        PlaySFX(endSFX, "interaction");

        hasPlayedEndSFX = true;

        if (healthManager.CurrentHealth <= 0)
            Invoke(nameof(PlayLoseSFX), 1.5f);

    }

    private void UpdateSuperState()
    {
        float drainRate = 100f / superDuration;
        scoreTracker.DrainSpecialOverTime(drainRate, Time.deltaTime);
        superElapsed += Time.deltaTime;

        if (superElapsed >= superDuration)
            ToggleSuper(false);
    }

    private void TryActivateSuper()
    {
        if (scoreTracker.Special >= 100f)
        {
            PlaySFX(superActiveSFX, "interaction");
            ToggleSuper(true);
        }
    }

    private void ToggleSuper(bool active)
    {
        isSuperActive = active;
        superElapsed = 0f;
        hasPlayedSuperReadySFX = false;
        scoreTracker.SetSpecial(active ? 100f : 0f);
        SongManager.Instance.ToggleReverbOnMusic(active);
        FullscreenTestController.SetCRT(active ? superDuration : 0);
    }

    private void HandleMilestone(int milestone)
    {
        PlaySFX(milestoneSFX, "interaction");
        voicelineManager.PlayNextVoiceline();
    }

    // ==== METODOS ESTATICOS A SEREM CHAMADOS PELO InputManager ====

    public static void Hit() => Instance.RegisterHit(false);
    public static void Miss() => Instance.RegisterMiss(false);
    public static void SuperHit() => Instance.RegisterHit(true);
    public static void SuperMiss() => Instance.RegisterMiss(true);
    public static void WrongPressMiss() => Instance.RegisterWrongPress();

    // ==== EVENTOS DE PLACAR ====

    public void RegisterHit(bool isSpecial, bool isSuperScoreActive = false)
    {
        scoreTracker.RegisterHit(isSpecial);

        healthManager.Heal(isSpecial ? specialHeal : normalHeal);
        if (isSpecial)
        {
            PlayRandomSFX(darkNoteHitSFX, "interaction");
        }
        else if (isSuperScoreActive)
        {
            PlayRandomSFX(superHitSFX, "interaction");
        }
        else
        {
            PlayRandomSFX(hitSFX, "interaction");
        }

        ShakeManager.instance.HitShake();
    }

    public void RegisterMiss(bool isSpecial)
    {
        scoreTracker.RegisterMiss(isSpecial);
        healthManager.Damage(isSpecial ? specialNoteDamage : normalNoteDamage);
        PlayRandomSFX(isSpecial ? superMissSFX : missSFX, "interaction");
    }

    public void RegisterWrongPress()
    {
        scoreTracker.RegisterWrongPress();
        healthManager.Damage(wrongPressDamage);
        PlayRandomSFX(wrongPressSFX, "interaction");
    }


    // ==== AUDIO ====

    private void PlaySFX(AudioClip clip, string audioSource)
    {
        if (clip != null) AudioManager.Instance.PlayOneShot(audioSource, clip);
    }

    private void PlayRandomSFX(List<AudioClip> clips, string audioSource)
    {
        if (clips == null || clips.Count == 0) return;
        int i = UnityEngine.Random.Range(0, clips.Count);

        AudioManager.Instance.PlayOneShot(audioSource, clips[i]);
    }

    private void PlayLoseSFX() => PlaySFX(loseSFX, interactionsAudioSource);
}