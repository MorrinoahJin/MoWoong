using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour
{
    public GameObject Bullet;
    Animator anim;
    Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pos = new Vector2(transform.position.x - 1f, transform.position.y - .3f);
        StartCoroutine(FireBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FireBullet()
    {
        yield return new WaitForSeconds(1.2f);
        Instantiate(Bullet, pos, Quaternion.identity);
        yield return new WaitForSeconds(.3f);
        anim.SetTrigger("Reverse");
        yield return new WaitForSeconds(.8f);
        Destroy(this.gameObject);
    }
}
