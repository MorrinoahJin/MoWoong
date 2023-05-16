using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void StartGame()
    {
        // 게임 시작 버튼 클릭 시 실행되는 코드
        SceneManager.LoadScene("Tutorial woong");
        Time.timeScale = 1f;

    }
    public void QuitGame()
    {
        // 게임 종료 버튼 클릭 시 실행되는 코드
        Application.Quit(); // 어플리케이션 종료
    }
}
