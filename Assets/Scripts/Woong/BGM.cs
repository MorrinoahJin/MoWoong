using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }
    public BgmType[] BGMList;
    private AudioSource bgm;
    private string NowBGMname = "";
    public float volume ;
    // Start is called before the first frame update
    void Start()
    {
        bgm = gameObject.AddComponent<AudioSource>();
        bgm.loop = true;
        if (BGMList.Length > 0) PlayBGM(BGMList[0].name);
        bgm.volume = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayBGM(string name)
    {
        
        if (NowBGMname.Equals(name)) return;

        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                bgm.clip = BGMList[i].audio;
                bgm.Play();
                 
                NowBGMname = name;
            }
    }
    public void SetVolume(float volume) 
    {
        bgm.volume = volume;
    }
}
