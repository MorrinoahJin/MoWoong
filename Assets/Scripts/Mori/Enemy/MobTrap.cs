using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTrap : MonoBehaviour
{
    Animator anim;
    public GameObject bullet;
    public float fireTime, bulletTime, IdleTime, hp;
    public bool isHasIdle;
    public float attSpeed, IdleSpeed;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        if (isHasIdle)
            StartCoroutine(Idle());
        else
            StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        if (hp > 0)
        {
            anim.SetTrigger("Attack");
            yield return new WaitForSeconds(bulletTime * attSpeed);
            if (hp > 0)
            {
                Instantiate(bullet, transform.position, Quaternion.identity);
                //Debug.Log("발사");
            }
            else
                anim.SetTrigger("Die");
            yield return new WaitForSeconds((fireTime - bulletTime) * attSpeed);
            if (isHasIdle)
                StartCoroutine(Idle());
            else
                StartCoroutine(Fire());
        }
        else
            anim.SetTrigger("Die");
    }

    IEnumerator Idle()
    {
        if(hp > 0)
        {
            anim.SetTrigger("Idle");
            yield return new WaitForSeconds((IdleTime) * IdleSpeed);
            StartCoroutine(Fire());
        }
        else
            anim.SetTrigger("Die");
    }
}
