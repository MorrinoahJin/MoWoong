using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isFire : MonoBehaviour
{
    Transform player;
    float speed=2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }
    private void OnEnable()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.position.x - player.position.x > 0)
        {
            transform.Translate(new Vector2(-1, 0) * Time.deltaTime * speed);            
        }
        else if (transform.position.x - player.position.x <= 0)
        {
            transform.Translate(new Vector2(1, 0) * Time.deltaTime * speed);
        }
        if (transform.position.y - player.position.y > 0)
        {
            transform.Translate(new Vector2(0, -1) * Time.deltaTime * speed);
        }
        else if (transform.position.y - player.position.y <= 0 )
            transform.Translate(new Vector2(0, 1) * Time.deltaTime * speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.tag == "Player")
        {
            PlayerWoong.isFire = true;
            Destroy(gameObject,.5f);
        }
    }
}
