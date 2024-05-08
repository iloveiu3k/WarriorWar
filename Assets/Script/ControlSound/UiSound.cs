using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    void Start()
    {
        // Bắt đầu phát lại âm thanh

        audioSource.clip = backgroundMusic;
        audioSource.PlayOneShot(backgroundMusic);
        if (audioSource.isPlaying)
        {
             Debug.Log("run");
        }
    }

    //void Update()
    //{
    //    if (!audioSource.isPlaying)
    //    {
    //        Debug.Log("ngu");
    //        audioSource.Play();
    //    }
    //    if (audioSource.isPlaying)
    //    {
    //        Debug.Log("ngu");
    //        audioSource.Play();
    //    }
    //}
}
