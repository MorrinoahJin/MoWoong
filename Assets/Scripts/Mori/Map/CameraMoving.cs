using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    //카메라 제어 변수
    static public bool cameraMovingStop;

    Vector3 defaultCamPos;
    Vector3 playerPos;
    float camPosY;
    //맵오브젝트 좌표를 받아오기 위해 맵 수 만큼 변수 할당
    public GameObject[] cameraPoint = new GameObject[2];
    public bool justFollowPlayer;

    //페이드 인아웃
    static public bool camBlack;
    public Image blackImage;
    float fadeSpeed = 1f, time = 0;

    public Camera cam;

    //카메라 줌인
    static public bool camZoomIn;
    float zoomSpeed = 0.033f;

    //카메라 상하좌우 반전
    static public bool mirrorMod;
    float mirrorSpeed = 0.0125f;
    float angleY;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        StartCoroutine(StageStart());
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;

        defaultCamPos = new Vector3(playerPos.x, camPosY, -10);

        if (!cameraMovingStop)
        {
            //거울모드일 때 y축 변경
            if (!mirrorMod)
                camPosY = playerPos.y + 2;
            else
            {
                camPosY = (playerPos.y * -1) - 2;
            }

            //기본 - 플레이어 따라다님
            if (justFollowPlayer)
                justFollowCam();
            else
                CamPoint();
        }

        CamZoomInOut();

        //CamMirror();
    }

    void CamPoint()
    {
        //맵 좌표, 플레이어 위치
        float camX = cameraPoint[Map.mapNum].transform.position.x;

        //일정 범위를 넘어가면 카메라가 플레이러를 계속 따라가지 않기 하기 위한 함수
        if (playerPos.x <= camX - 5)
        {
            this.transform.position = new Vector3(camX - 5, camPosY, -10);
        }
        else if (playerPos.x >= camX + 5)
        {
            this.transform.position = new Vector3(camX + 5, camPosY, -10);
        }
        else
            this.transform.position = new Vector3(playerPos.x, camPosY, -10);
    }
    void justFollowCam()
    {
        if (transform.position.y > -7.5f)
            this.transform.position = defaultCamPos;
        else
            this.transform.position = new Vector3(transform.position.x, -7.5f, transform.position.z);
    }
    void CamZoomInOut()
    {
        if (camZoomIn)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 3, zoomSpeed);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, zoomSpeed);
        }
    }

    void CamMirror()
    {
        if (mirrorMod)
        {
            angleY = Mathf.Lerp(transform.localEulerAngles.z, 180, mirrorSpeed);
            //Mathf.Lefp(a, -a, -1*2*a / 180 * speed)
            transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angleY);
        }
        else
        {
            angleY = Mathf.Lerp(transform.localEulerAngles.z, 0, mirrorSpeed);
            transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, angleY);
        }

    }
    public void StartFadeInOut()
    {
        if(!camBlack)
            StartCoroutine(FadeFlow());
    }

    IEnumerator FadeFlow()
    {
        camBlack = true;

        blackImage.gameObject.SetActive(true);
        Color tempColor = blackImage.color;

        time = 0;
        while (tempColor.a < 1f)
        {
            time += Time.deltaTime / fadeSpeed;
            tempColor.a = Mathf.Lerp(0, 1, time);
            blackImage.color = tempColor;
            yield return null;
        }
        time = 0;

        yield return new WaitForSeconds(1f);

        while (tempColor.a > 0)
        {
            time += Time.deltaTime / fadeSpeed;
            tempColor.a = Mathf.Lerp(1, 0, time);
            blackImage.color = tempColor;
            yield return null;
        }

        blackImage.gameObject.SetActive(false);
        yield return null;

        camBlack = false;
    }

    public IEnumerator StageStart()
    {
        blackImage.gameObject.SetActive(true);
        Color tempColor = blackImage.color;
        tempColor.a = 1f;

        while (tempColor.a > 0)
        {
            time += Time.deltaTime / fadeSpeed;
            tempColor.a = Mathf.Lerp(1, 0, time);
            blackImage.color = tempColor;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        blackImage.gameObject.SetActive(false);
        tempColor.a = 1f;
    }

    //튜토리얼 스테이지 중 보스룸에서 플레이어가 죽었을 때 카메라 위치 이동
    public void MoveCamWhenPlayerDied(Vector3 position)
    {
        cameraMovingStop = true;
        transform.position = Vector3.MoveTowards(transform.position, position, 2 * Time.deltaTime);
    }

    //카메라 동작을 멈추고 특정 지점까지 카메라 텔레포트
    public void TeleportCam(Vector3 position)
    {
        cameraMovingStop = true;
        transform.position = position;
    }

}
