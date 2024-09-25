using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip StartGame;
    public AudioClip CutBlock;
    public AudioClip PerfectStack;

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
        PlaySound(PerfectStack);
    }

    public void PartialStackSound() {
        PlaySound(CutBlock);
    }

    public void GameOverSound() {
        PlaySound(StartGame);
    }
}
