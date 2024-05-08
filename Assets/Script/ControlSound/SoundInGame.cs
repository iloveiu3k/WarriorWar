using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInGame : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource backgroundSource;
    public AudioSource winSource;
    public AudioSource loseSource;
    public AudioSource welcomeSource;
    void Start()
    {
        backgroundSource.Play();
        welcomeSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
