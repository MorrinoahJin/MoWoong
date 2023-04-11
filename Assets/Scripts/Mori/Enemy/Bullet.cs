using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attPower, speed;
    Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector2(transform.position.x, transform.position.y - 1);
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerWoong>().TakeDamage(attPower);
            Destroy(gameObject, .16f);
        }
        else if (collision.tag == "Wall")
            Destroy(gameObject, .33f);
    }
}
