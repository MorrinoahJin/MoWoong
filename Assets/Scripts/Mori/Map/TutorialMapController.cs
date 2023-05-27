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

    //게임연출에 쓰일 구체와 레이저빔
    public GameObject circle, razorBeam;
    //구체 첫번째 움직임, 두번째 움직임
    bool circleMove1, circleMove2, moveCamPlayerDied;
    public GameObject ui_HP;

    // Start is called before the first frame update
    void Start()
    {
        moveCamPlayerDied = false;
        circle.SetActive(false);
        razorBeam.SetActive(false);
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

        if (PlayerWoong.playerHp >= 100)
            playerHpZeroCount = 0;

        if (PlayerWoong.playerHp <= 0 && playerHpZeroCount == 0)
        {
            playerHpZeroCount += 1;
            StartCoroutine(PlayerDieFX());
        }

        //플레이어가 보스스테이지에 있을 경우 죽으면 이벤트 발생, 보스스테이지에 아닐 경우에 죽으면 재시작
        if (PlayerWoong.playerHp <= 0)
        {
            if (playerInBossStageCount == 0 && !playerDie)
            {
                StartCoroutine(PlayerDie());
            }
            else if(playerInBossStageCount >= 1 && !playerDie)
            {
                playerDie = true;
                StartCoroutine(circleMoving());
            }
        }
        else
            CameraMoveToBoss();

        //if(!PauseMenu.isPause)
        TimeController();

        //Debug.Log(Time.timeScale);
        if (circleMove1 && !circleMove2)
        {
            circle.transform.position = Vector3.MoveTowards(circle.transform.position, new Vector3(110, .66f, 0), 3f * Time.deltaTime);
        }
        if(circleMove2)
        {
            circle.transform.position = Vector3.MoveTowards(circle.transform.position, new Vector3(107, .66f, 0), 2f * Time.deltaTime);
        }
        if(moveCamPlayerDied)
        {
            GameObject.Find("Main Camera").GetComponent<CameraMoving>().MoveCamWhenPlayerDied(new Vector3(110, 2, -10));
        }
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
        yield return new WaitForSeconds(3.5f);
        GameObject.Find("Main Camera").GetComponent<CameraMoving>().StartFadeInOut();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Tutorial");
    }

    IEnumerator circleMoving()
    {
        //모든 UI삭제
        ui_HP.SetActive(false);
        moveCamPlayerDied = true;
        yield return new WaitForSeconds(8f);
        circle.SetActive(true);
        circleMove1 = true;
        yield return new WaitForSeconds(2.5f);
        circleMove2 = true;
        yield return new WaitForSeconds(1.5f);
        razorBeam.SetActive(true);
        yield return new WaitForSeconds(.5f);
        circle.SetActive(false);
        yield return new WaitForSeconds(3f);
        circleMove1 = false;
        circleMove2 = false;
        circle.SetActive(true);
        yield return new WaitForSeconds(5f);
        bossObj.SetActive(false);
        razorBeam.SetActive(false);
        moveCamPlayerDied = false;
        StartCoroutine(GameObject.Find("TutorialManager").GetComponent<TutorialDialogue>().Dialogue());
    }
}
