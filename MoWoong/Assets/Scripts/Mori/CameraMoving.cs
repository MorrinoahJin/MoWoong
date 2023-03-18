using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    //�ʿ�����Ʈ ��ǥ�� �޾ƿ��� ���� �� �� ��ŭ ���� �Ҵ�
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
        //�� ��ǥ, �÷��̾� ��ġ
        float camX = cameraPoint[Map.mapNum].transform.position.x;
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;

        //���� ������ �Ѿ�� ī�޶� �÷��̷��� ��� ������ �ʱ� �ϱ� ���� �Լ�
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
