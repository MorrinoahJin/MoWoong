using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Dialogue : MonoBehaviour
{
    
    Vector3[] camPos = new Vector3[2];
    Vector3 playerPos;
    public float witchPosX, camSpeed;
    bool camMove;
    Camera cam;
    public GameObject firstText, secondText, thirdText,fourthText;
    bool witchMove;
    public GameObject witch,portal,bumper;
    int playerInBossStageCount;
    // Start is called before the first frame update
    private void Start()
    {
        camMove= false;
        cam = Camera.main;
        firstText.SetActive(false);
        secondText.SetActive(false);
        thirdText.SetActive(false);
        fourthText.SetActive(false);
        portal.SetActive(false);
        bumper.SetActive(false);
        CameraMoving.cameraMovingStop = false;
        //CameraMoving.camZoomIn = false;
        playerInBossStageCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;
        camPos[1] = new Vector3(playerPos.x, playerPos.y + 2, -10);
        camPos[0] = new Vector3(witchPosX, -0.8838511f, -10);
        CameraMoveToBoss();
        if (witchMove)
        {
            witch.transform.position = Vector3.MoveTowards(witch.transform.position, new Vector3(5f, .5f, 0), 1f * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && playerInBossStageCount == 0) {
            playerInBossStageCount += 1;
            StartCoroutine(Dialogue());
          
        }
    }
    void CameraMoveToBoss()
    {
        if (camMove)
            cam.transform.position = Vector3.Lerp(cam.transform.position, camPos[0], camSpeed);
        else
            cam.transform.position = Vector3.Lerp(cam.transform.position, camPos[1], camSpeed);
    }
    IEnumerator Dialogue()
    {
        //플레이어 제어 멈추고 카메라이동
        PlayerWoong.canControl = false;
        CameraMoving.cameraMovingStop = true;
        camMove = true;
        yield return new WaitForSeconds(2f);
        
        //첫번째 대사
        firstText.SetActive(true);
        yield return new WaitForSeconds(1f);
        firstText.SetActive(false);
        yield return new WaitForSeconds(.5f);

        //두번째 대사
        secondText.SetActive(true);
        yield return new WaitForSeconds(1f);
        secondText.SetActive(false);
        //마법사 이동
        witchMove = true;
        yield return new WaitForSeconds(4f);

        //마법사 이동 멈춤
        witchMove = false;
        yield return new WaitForSeconds(0.5f);
        //포탈 생성
        portal.SetActive(true);
        yield return new WaitForSeconds(2f);
        //마법사 다시 이동
        witchMove = true;
        yield return new WaitForSeconds(1f);
        witch.SetActive(false);
        //세번째 대사
        yield return new WaitForSeconds(1f);
        thirdText.SetActive(true);
        yield return new WaitForSeconds(1f);
        thirdText.SetActive(false);
        //네번째 대사
        yield return new WaitForSeconds(2f);
        fourthText.SetActive(true);
        yield return new WaitForSeconds(1f);
        fourthText.SetActive(false);
        yield return new WaitForSeconds(.5f);
        bumper.SetActive(true);
        PlayerWoong.canControl = true;

        
        
    }
}
