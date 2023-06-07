using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public GameObject player;
    Animator anim;
    bool open;
    bool isPlayerEnter;
    public GameObject Element_Fire;
    public GameObject Text;
    // Start is called before the first frame update
  
    private void Start()
    {
        isPlayerEnter = false;
        open = false;
        Text.SetActive(false);
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
            StartCoroutine(createItem());
        }
        
    }
    IEnumerator createItem()
    {
        yield return new WaitForSeconds(.5f);
        Instantiate(Element_Fire, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(.5f);
        Text.SetActive (true);
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
