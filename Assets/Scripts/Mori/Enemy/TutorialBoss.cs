using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle,
    Attack1,
    Attack2,
    Attack3,
    SpawnMob,
    Die,
}

public class TutorialBoss : MonoBehaviour
{
    BossState currentState;
    Animator anim;
    int doNum;
    bool doDie;
    public float AttDamage, hp;

    Vector2 attboxPos;
    BoxCollider2D attBox;

    public GameObject miniMob;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(Idle());
    }

    void Update()
    {
        if (hp < 0)
        {
            currentState = BossState.Die;
            if (!doDie)
                StartCoroutine(Die());
        }
    }

    IEnumerator Idle()
    {
        doNum = Random.Range(0, 5);
        switch (doNum)
        {
            case 1:
                currentState = BossState.Idle;
                break;
            case 2:
                currentState = BossState.Attack1;
                break;
            case 3:
                currentState = BossState.SpawnMob;
            break;
        }


        anim.SetTrigger("Idle");
        yield return new WaitForSeconds(2f);

        switch (currentState)
        {
            case BossState.Idle:
                yield return StartCoroutine(Idle());
                break;
            case BossState.Attack1:
                yield return StartCoroutine(Attack1());
                break;
            case BossState.Attack2:
                yield return StartCoroutine(Attack2());
                break;
            case BossState.Attack3:
                yield return StartCoroutine(Attack3());
                break;
            case BossState.SpawnMob:
                yield return StartCoroutine(SpawnMob());
                break;
            case BossState.Die:
                yield return StartCoroutine(Die());
                break;
        }
        yield return StartCoroutine(Idle());
    }

    IEnumerator Attack1()
    {
        anim.SetTrigger("Attack1");
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(Idle());
    }

    IEnumerator Attack2()
    {
        anim.SetTrigger("Idle");
        yield return StartCoroutine(Idle());
    }

    IEnumerator Attack3()
    {
        anim.SetTrigger("Idle");
        yield return StartCoroutine(Idle());
    }

    IEnumerator SpawnMob()
    {
        anim.SetTrigger("Idle");
        Instantiate(miniMob, transform);
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(Idle());
    }

    IEnumerator Die()
    {
        doDie = true;
        anim.SetTrigger("Die");
        yield return StartCoroutine(Idle());
    }

    void CheckAttak1()
    {

    }
}
