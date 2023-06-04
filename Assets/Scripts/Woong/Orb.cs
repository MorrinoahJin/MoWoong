using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Orb : MonoBehaviour
{
    private Animator anim;
    static int formNum;
    Transform player;
    Rigidbody2D rigid;
    public float distance;
    public float speed;
    public float jumpForce;
    public float teldistance;    
    public LayerMask groundLayer;
    //static public bool orbControl;
    static public bool isSkillOn;
    static public float orbDirection;
  
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 11);
     
    }

    // Update is called once per frame
    void Update()
    {
        //UnityEngine.Debug.Log(isSkillOn);
        if (!isSkillOn)
        {
            //오브 좌우 이동
            if (Mathf.Abs(transform.position.x - player.position.x) > distance)
            {
                transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speed);
                DirectionOrb();
           
            }

            //오브 상하 이동
            if(transform.position.y - player.position.y > 0.38)
            {
                transform.Translate(new Vector2(0, -1) * Time.deltaTime * speed);
            }
            else if(transform.position.y - player.position.y < 0.38)
                transform.Translate(new Vector2(0, 1) * Time.deltaTime * speed);
           
            //오브 텔포
            if (Vector2.Distance(player.position, transform.position) > teldistance)
            {
                transform.position = player.position;
            }
        }
        else if(isSkillOn)
        {

        }

    }
    public void nowForm(int num)
    {
        int curnum = 0;
        if (num== 0)
            curnum = 0;
        //이동
        else if (num == 1)
            curnum = 1;
        if (curnum != formNum)
        {
            formNum = curnum;
            formChange();
        }
    }
    void formChange()
    {

        if (formNum == 0)
           anim.SetTrigger("Electric");
        //이동
        else if (formNum == 1)
           anim.SetTrigger("Fire");
       
    }
    public float DirectionOrb()
    {
        
        if (transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            orbDirection = 1;
            return 1;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            orbDirection = -1;
            return -1;
        }
    }
   
}

