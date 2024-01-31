using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource ClickSound;
    public AudioSource BGSound;
    private static SoundManager instance;
    public static SoundManager Instance { get => instance; }
    private void Awake()
    {
        instance = this;    
    }

    private void Start()
    {
        DontDestroyOnLoad(BGSound);
    }

    public void PlayClickSound(AudioClip clip)
    {
        ClickSound.PlayOneShot(clip);
    }
}
