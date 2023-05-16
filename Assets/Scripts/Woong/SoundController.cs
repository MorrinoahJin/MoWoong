using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    public AudioSource BGMsound;
    public AudioClip soundClip;
    public AudioClip[] bgmList;
    // Start is called before the first frame update
    /*
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        throw new System.NotImplementedException();
    }

    private void OnSceneLoaded(Scene arg0,LoadSceneMode arg1)
    {
        for(int i = 0; i <bgmList.Length; i++){
            if (arg0.name == bgmList[i].name)
                BGMPlay(bgmList[i]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SFXPlay(string sfxName,AudioClip clip)
    {
        GameObject sfx = new GameObject(sfxName + "Sound");
       AudioSource audioSource = sfx.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(sfx,clip.length);
    }
    public void BGMPlay()
    {
        BGMsound.clip = clip;
        BGMsound.loop = true;
        BGMsound.Play();        
    }*/
}
