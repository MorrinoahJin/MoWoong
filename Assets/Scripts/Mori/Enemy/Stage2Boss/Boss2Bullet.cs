using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Bullet : MonoBehaviour
{
    Vector2 goalPos;
    Rigidbody2D rb;

    bool move;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            rb.AddForce(goalPos * 1.5f * Time.deltaTime, ForceMode2D.Impulse);
        }
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(posX, posY), 2f * Time.deltaTime);
    }

    public void MoveToPositoin(Vector2 pos)
    {
        move = true;
        goalPos = pos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if(col.tag == "Player")
        {
            col.GetComponent<PlayerWoong>().TakeDamage(10, transform.position);
            Destroy(gameObject);
        }
    }
}
