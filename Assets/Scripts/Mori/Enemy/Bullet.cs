using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float attPower, speed;
    public float rotateSpeed = 1f; // 회전 속도
    Vector2 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotateSpeed += 15;
        targetPos = new Vector2(transform.position.x, transform.position.y - 1);
        this.transform.position = Vector2.MoveTowards(this.transform.position, targetPos, speed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(0,0,rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerWoong>().TakeDamage(attPower, transform.position);
            Destroy(gameObject, .01f);
        }
        else if (collision.tag == "Wall")
            Destroy(gameObject, .01f);
    }
}
