using System.Collections.Generic;
using UnityEngine;

public class VoicelineManager : MonoBehaviour
{
    [Tooltip("Lista de falas do Narrador durante o jogo")]
    [SerializeField] private List<AudioClip> voicelines;

    [Tooltip("Tag da Audio Source para as voicelines do jogo")]
    [SerializeField] private string audioSourceTag = "voiceline";

    private int currentVoicelineIndex = 0;

    public void PlayNextVoiceline()
    {
        if (voicelines == null || voicelines.Count == 0 || audioSourceTag == null)
            return;

        if (currentVoicelineIndex >= voicelines.Count)
            currentVoicelineIndex = 0;

        AudioManager.Instance.PlayOneShot(audioSourceTag, voicelines[currentVoicelineIndex]);

        currentVoicelineIndex++;
    }
}
