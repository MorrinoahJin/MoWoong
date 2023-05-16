using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource bgmPlayer;
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum Sfx { Button, Attack }
    int sfxCursor;
    void Start()
    {
        bgmPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
