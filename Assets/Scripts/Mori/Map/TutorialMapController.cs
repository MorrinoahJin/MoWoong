using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMapController : MonoBehaviour
{
    public GameObject bossObj;
    Vector3[] camPos = new Vector3[2];
    Vector3 playerPos;
    public float BossPosX;
    bool camMove, timeControl, playerDie;
    public float camSpeed, timeSpeed;
    Camera cam;
    int playerInBossStageCount, playerHpZeroCount;

    // Start is called before the first frame update
    void Start()
    {
        CameraMoving.cameraMovingStop = false;
        CameraMoving.camZoomIn = false;
        cam = Camera.main;
        playerInBossStageCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;
        camPos[1] = new Vector3(playerPos.x, playerPos.y + 2, -10);
        camPos[0] = new Vector3(BossPosX, playerPos.y + 2, -10);

        CameraMoveToBoss();
        TimeController();

        if (PlayerWoong.playerHp >= 100)
            playerHpZeroCount = 0;

        if (PlayerWoong.playerHp <= 0 && playerHpZeroCount == 0)
        {
            playerHpZeroCount += 1;
            StartCoroutine(PlayerDieFX());
        }

        if (PlayerWoong.playerHp <= 0)
        {
            if (playerInBossStageCount == 0 && !playerDie)
            {
                StartCoroutine(PlayerDie());
            }
            else
            {

            }
        }
        //Debug.Log(Time.timeScale);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && playerInBossStageCount == 0)
        {
            playerInBossStageCount += 1;
            bossObj.SetActive(true);
            StartCoroutine(CameraControllAndZoom());
        }
    }
    
    IEnumerator CameraControllAndZoom()
    {
        yield return new WaitForSeconds(.33f);
        //플레이어 이동 불가
        CameraMoving.cameraMovingStop = true;
        CameraMoving.camZoomIn = true;
        camMove = true;
        yield return new WaitForSeconds(3f);
        camMove = false;
        yield return new WaitForSeconds(2f);
        //플레이어 이동 허용
        CameraMoving.cameraMovingStop = false;
        CameraMoving.camZoomIn = false;
    }

    void CameraMoveToBoss()
    {
        if(camMove)
            cam.transform.position = Vector3.Lerp(cam.transform.position, camPos[0], camSpeed);
        else
            cam.transform.position = Vector3.Lerp(cam.transform.position, camPos[1], camSpeed);
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

   void TimeController()
   {
        if (timeControl)
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.25f, timeSpeed);
        else
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, timeSpeed);
   }

    IEnumerator PlayerDie()
    {
        playerDie = true;
        if (playerInBossStageCount == 0)
        {
            yield return new WaitForSeconds(3.5f);
            GameObject.Find("Main Camera").GetComponent<CameraMoving>().StartFadeInOut();
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("Tutorial");
        }
        else
        {

        }
    }

}
