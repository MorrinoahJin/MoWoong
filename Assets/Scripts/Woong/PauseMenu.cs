using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class PauseMenu : MonoBehaviour
{
    static public bool isPause;
    public GameObject pauseMenu;
    public GameObject Option;
    void Start()
    {
        isPause = false;
    }

    
    void Update()
    {

    }
    public void restart()
    {
        SceneManager.LoadScene("tutorial woong");
        if (isPause)
        {
            isPause = false;
            Debug.Log("해제");

            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            Option.SetActive(true);
        }
    }
    public void gotoMain()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void pause()
    {
       
        if (!isPause)
        {
            isPause = true;
            Debug.Log("일시정지");
            PlayerWoong.canControl = false;
            Time.timeScale = 0f;
            Debug.Log(Time.timeScale);
            pauseMenu.SetActive(true) ;
            Option.SetActive(false);
           
        }
        else if (isPause)
        {
            isPause = false;
            Debug.Log("해제");
            PlayerWoong.canControl = true;
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
            Option.SetActive(true) ;

        }
    }
}

