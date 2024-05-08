using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPick : MonoBehaviour
{
    public AudioSource backgroundSource;
    public AudioSource selectSource;
    public AudioSource timeSource;
    public AudioClip selectAudio;
    public AudioClip btnChooseAudio;
    void Start()
    {
        backgroundSource.Play();
    }
    public void SelectSound()
    {
        selectSource.clip = selectAudio;
        selectSource.Play();
    }
    public void ChooseSound()
    {
        selectSource.clip = btnChooseAudio;
        selectSource.Play();
    }
}
