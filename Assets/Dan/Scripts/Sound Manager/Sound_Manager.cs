using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour {

    public static Sound_Manager sm;

    public bool playBackgroundMusicOnAwake = true;
    public AudioClip[] backgroundMusic;
    [Space]
    public AudioClip[] Adult_Grunt;
    [Space]
    public AudioClip[] Adult_Scream;
    [Space]
    public AudioClip[] Child_Grunt;
    [Space]
    public AudioClip[] Child_Scream;

    private AudioSource me;

    
    private void Awake()
    {
        sm = this;
    }
    private void Start()
    {
        me = GetComponent<AudioSource>();
        me.clip = backgroundMusic[Random.Range(0, backgroundMusic.Length)];
        if (playBackgroundMusicOnAwake)
        {
            me.Play();
        }       
    }

    public void PauseUnPauseMusic(bool pause)
    {
        if (pause)
            me.Pause();
        else
            me.Play();
    }

    public AudioClip GetHitSound(int index)
    {
        switch (index)
        {
            case 0:
                 return Adult_Scream[Random.Range(0, Adult_Scream.Length)];
            case 1:
                 return Child_Scream[Random.Range(0, Child_Scream.Length)];
            case 2:
                return Adult_Grunt[Random.Range(0, Adult_Grunt.Length)];
            case 3:
                return Child_Grunt[Random.Range(0, Child_Grunt.Length)];
        }
        return Adult_Grunt[Random.Range(0, Adult_Grunt.Length)];
    }
}
