using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] sequenceSounds;
    public AudioClip StartGame;
    public AudioClip CutBlock;
    private int sequenceIndex = 0;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void PlaySound(AudioClip clip) {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StartGameSound() {
        PlaySound(StartGame);
    }

    public void PerfectStackSound() {
        sequenceIndex = sequenceIndex >= sequenceSounds.Length ? sequenceSounds.Length - 1: sequenceIndex;
        audioSource.PlayOneShot(sequenceSounds[sequenceIndex]);
        sequenceIndex++;
    }

    public void PartialStackSound() {
        PlaySound(CutBlock);
        sequenceIndex = 0;
    }

    public void GameOverSound() {
        PlaySound(StartGame);
    }
}
