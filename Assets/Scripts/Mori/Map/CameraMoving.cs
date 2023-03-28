using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    //맵오브젝트 좌표를 받아오기 위해 맵 수 만큼 변수 할당
    public GameObject[] cameraPoint = new GameObject[2];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CamPoint();
    }

    void CamPoint()
    {
        //맵 좌표, 플레이어 위치
        float camX = cameraPoint[Map.mapNum].transform.position.x;
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;

        //일정 범위를 넘어가면 카메라가 플레이러를 계속 따라가지 않기 하기 위한 함수
        if (playerPos.x <= camX - 5)
        {
            this.transform.position = new Vector3(camX - 5, playerPos.y, -10);
        }
        else if (playerPos.x >= camX + 5)
        {
            this.transform.position = new Vector3(camX + 5, playerPos.y, -10);
        }
        else
            this.transform.position = new Vector3(playerPos.x, playerPos.y, -10);
    }
}
