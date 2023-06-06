using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossStage2MapController : MonoBehaviour
{
    float playerPosY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPosY = GameObject.Find("Player").transform.position.y;

        if (playerPosY <= 0.65)
            GameObject.Find("Player").GetComponent<PlayerWoong>().jumpCount = 0;
        if (PlayerWoong.playerHp <= 0)
        {
            SceneManager.LoadScene("Boss Stage 1");
        }

    }

}
