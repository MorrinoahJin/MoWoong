using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    public GameObject firstText, secondText, thirdText, fourText, fiveText;

    private void Start()
    {
        firstText.SetActive(false);
        secondText.SetActive(false);
        thirdText.SetActive(false);
        fourText.SetActive(false);
        fiveText.SetActive(false);

    }
    public IEnumerator Dialogue()
    {
        
        yield return new WaitForSeconds(5f);
        //대사치는 케릭터가 옴

        yield return new WaitForSeconds(3f);
        //첫번째 대사
        firstText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        firstText.SetActive(false);
        yield return new WaitForSeconds(1f);
        //두번째 대사
        secondText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        secondText.SetActive(false);
        yield return new WaitForSeconds(1f);
        //세번째 대사
        thirdText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        thirdText.SetActive(false);
        yield return new WaitForSeconds(1f);
        //네번째 대사와 함께 페이드 인
        fourText.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("Main Camera").GetComponent<CameraMoving>().StartFadeInOut();
        yield return new WaitForSeconds(1f);
        //카메라 이동
        GameObject.Find("Main Camera").GetComponent<CameraMoving>().TeleportCam(new Vector3(200, 202.3f, -10));
        yield return new WaitForSeconds(1f);
        fourText.SetActive(false);
        //다섯번째 대사와 함께 페이드 아웃
        fiveText.SetActive(true);
        yield return new WaitForSeconds(3f);
        fiveText.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<CameraMoving>().StartFadeInOut();
        //다음씬으로 ㄱㄱ
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Stage 1");
    }
}
