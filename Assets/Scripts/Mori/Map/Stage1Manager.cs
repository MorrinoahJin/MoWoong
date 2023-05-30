using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class Stage1Manager : MonoBehaviour
{
    int playerHpZeroCount;
    bool camMove, timeControl, playerDie;

    // Start is called before the first frame update
    void Start()
    {
        playerHpZeroCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerWoong.playerHp >= 100)
            playerHpZeroCount = 0;

        if (PlayerWoong.playerHp <= 0 && playerHpZeroCount == 0)
        {
            playerHpZeroCount += 1;
            StartCoroutine(PlayerDieFX());
        }

        if(PlayerWoong.playerHp <= 0)
            StartCoroutine(PlayerDie());


        //if(!PauseMenu.isPause)
        TimeController();
    }

    IEnumerator PlayerDieFX()
    {
        yield return new WaitForSeconds(.33f);
        CameraMoving.cameraMovingStop = true;
        CameraMoving.camZoomIn = true;
        timeControl = true;
        yield return new WaitForSeconds(2f);
        timeControl = false;
        yield return new WaitForSeconds(1.5f);
        CameraMoving.cameraMovingStop = false;
        CameraMoving.camZoomIn = false;
    }

    IEnumerator PlayerDie()
    {
        playerDie = true;
        yield return new WaitForSeconds(3.5f);
        GameObject.Find("Main Camera").GetComponent<CameraMoving>().StartFadeInOut();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Stage 1");
    }

    void TimeController()
    {
        if (timeControl)
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.25f, 0.0033f);
        else
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.0033f);
    }
}
