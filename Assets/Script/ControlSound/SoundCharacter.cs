using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SoundCharacter : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioClip dameSound, dieSound, moveSound,craft1,craft2;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }
    public void SoundMove()
    {
        audioSource.clip = moveSound;
        audioSource.Play();
    }
    public void SoundDame()
    {
        audioSource.clip = dameSound;
        audioSource.Play();
    }
    public void SoundDie()
    {
        audioSource.clip = dieSound;
        audioSource.Play();
    }
    public void SoundCraft(int i)
    {
        if (i == 1)
        {
            audioSource.clip = craft1;
        }
        else if (i == 2)
        {
            audioSource.clip = craft2;
        }
        audioSource.Play();
    }
}
