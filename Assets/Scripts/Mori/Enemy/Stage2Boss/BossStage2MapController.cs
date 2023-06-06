using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (playerPosY <= 0.54)
            GameObject.Find("Player").GetComponent<PlayerWoong>().jumpCount = 0;

    }

}
