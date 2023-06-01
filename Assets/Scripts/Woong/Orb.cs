using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Orb : MonoBehaviour
{
    Transform player;
    Rigidbody2D rigid;
    public float distance;
    public float speed;
    public float jumpForce;
    public float teldistance;    
    public LayerMask groundLayer;
    // static public bool orbControl;
    static public bool isSkillOn;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        Physics2D.IgnoreLayerCollision(7, 11);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isSkillOn)
        {
            //오브 이동
            if (Mathf.Abs(transform.position.x - player.position.x) > distance)
            {
                transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speed);
                DirectionOrb();
                RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * -1f, 0.5f, groundLayer);
                RaycastHit2D hit2 = Physics2D.Raycast(transform.position, new Vector2(1 * DirectionOrb(), 1), 2f, groundLayer);
                if (player.position.y - transform.position.y <= 0)
                    hit2 = new RaycastHit2D();
                if (hit || hit2)
                {
                    rigid.velocity = Vector2.up * jumpForce;
                }
            }
            //오브 텔포
            if (Vector2.Distance(player.position, transform.position) > teldistance)
            {
                transform.position = player.position;
            }
        }
        //else
            //transform.position = transform.position;
    }

    float DirectionOrb()
    {
        if (transform.position.x - player.position.x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            return 1;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            return -1;
        }
    }
   
}

