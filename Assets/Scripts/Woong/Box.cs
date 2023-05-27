using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject player;
    Animator anim;
    bool open;
    bool isPlayerEnter;
    // Start is called before the first frame update
    void Awake()
    {
        isPlayerEnter = false;
        open = false;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Open();
    }
    void Open()
    {
        if (!open && isPlayerEnter && Input.GetKeyDown("e"))
        {
            Debug.Log("상자열림");
            open = true;
            anim.SetTrigger("Box_Open");
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("플레이어 접근");
            isPlayerEnter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("플레이어 접근X");
            isPlayerEnter = false;
        }
    }
  
}
