using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMapController : MonoBehaviour
{
    Vector3[] camPos = new Vector3[2];
    Vector3 playerPos;
    public float BossPosX;
    bool camMove;
    public float camSpeed;
    Camera cam;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;
        camPos[1] = new Vector3(playerPos.x, playerPos.y + 2, -10);
        camPos[0] = new Vector3(BossPosX, playerPos.y + 2, -10);

        CameraMoveToBoss();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && count == 0)
        {
            count += 1;
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
}
