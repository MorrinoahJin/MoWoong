using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    Animator anim;
    bool isOpen;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
           //anim.SetTrigger("Open");
           anim.SetTrigger("Idle");

    }
    public void Open()
    {
        isOpen = true;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        //플레이어와 충돌하면 연결된 텔레포트 위치로 플레이어를 이동
        if (col.tag == "Player")
        {
            StartCoroutine(SceneChange());
        }
    }
    IEnumerator SceneChange()
    {
        PlayerWoong.canControl = false;
        yield return new WaitForSeconds(.15f);
        GameObject.Find("Main Camera").GetComponent<CameraMoving>().StartFadeInOut();
        yield return new WaitForSeconds(1.5f);        
        SceneManager.LoadScene("Boss Stage 1 test");
    }
}
