using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2Bullet : MonoBehaviour
{
    public float speed;
    Animator anim;
    Vector3 playerPos;
    bool isHited;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerPos = GameObject.FindWithTag("Player").transform.position;
        StartCoroutine(DestroyObj());

        if (transform.position.x > playerPos.x)
            transform.localEulerAngles = new Vector3(0, 180, 0);
        else
            transform.localEulerAngles = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        if(!isHited)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos, speed * Time.deltaTime);
        }

        Vector3 thisPos = transform.position;
        if (playerPos == thisPos)
        {
            anim.SetTrigger("Hited");
            Destroy(gameObject, .66f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" || col.tag == "Wall")
        {
            anim.SetTrigger("Hited");
            Destroy(gameObject, .66f);
            isHited = true;
        }
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(3f);
        anim.SetTrigger("Hited");
        Destroy(gameObject, 1f);
    }
}
