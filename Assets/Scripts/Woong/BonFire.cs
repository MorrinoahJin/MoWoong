using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonFire : MonoBehaviour
{
    public GameObject player;
    bool isPlayerEnter;
    // Start is called before the first frame update
    void Awake()
    {
        isPlayerEnter = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerEnter)
        {

        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            //Debug.Log("플레이어 접근");
            isPlayerEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            //Debug.Log("플레이어 접근X");
            isPlayerEnter = false;
        }
    }
}
