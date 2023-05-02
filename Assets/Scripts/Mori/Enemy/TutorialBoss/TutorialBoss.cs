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
    int doNum, animNum;
    bool doDie;
    Vector2 attBoxSize;
    public float AttPower, hp;

    public GameObject miniMob, miniMobPos, miniMobPos2;
    public GameObject Attack2Skill, Attack2SkillPos;

    // Start is called before the first frame update
    void OnEnable()
    {
        animNum = 0;
        attBoxSize = new Vector2(11f, 5f);

        anim = GetComponent<Animator>();
        StartCoroutine(IdleStart());
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

    IEnumerator IdleStart()
    {
        yield return new WaitForSeconds(6f);
        StartCoroutine(Idle());
    }

    IEnumerator Idle()
    {
        doNum = Random.Range(1, 7);
        switch (doNum)
        {
            /*
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
                currentState = BossState.Attack2;
                break;
            */

            case 1:
            case 2:
            case 3:
                currentState = BossState.Attack1;
                break;
            case 4:
                currentState = BossState.SpawnMob;
                break;
            case 5:
            case 6:
                currentState = BossState.Attack2;
                break;
        }

        doAnim("Idle");
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
        doAnim("AttackReady");
        yield return new WaitForSeconds(1.5f);
        doAnim("Attack1");
        yield return new WaitForSeconds(.5f);
        CheckAttak1();
        yield return new WaitForSeconds(.3f);
        yield return StartCoroutine(Idle());
    }

    IEnumerator Attack2()
    {
        Instantiate(Attack2Skill, Attack2SkillPos.transform);
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(Idle());
    }

    IEnumerator Attack3()
    {
        doAnim("Idle");
        yield return StartCoroutine(Idle());
    }

    IEnumerator SpawnMob()
    {
        int num = Random.Range(0, 2);
        yield return new WaitForSeconds(1.5f);
        doAnim("Idle");
        if(num == 0)
            Instantiate(miniMob, miniMobPos.transform);
        else
            Instantiate(miniMob, miniMobPos2.transform);
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(Idle());
    }

    IEnumerator Die()
    {
        doDie = true;
        doAnim("Die");
        yield return StartCoroutine(Idle());
    }

    void CheckAttak1()
    {
        Collider2D[] PlayerCollider = Physics2D.OverlapBoxAll(transform.position, attBoxSize, 0f);

        foreach (Collider2D collider in PlayerCollider)
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("데미지 계산");
                collider.GetComponent<PlayerWoong>().TakeDamage(AttPower);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, attBoxSize);
    }


    void doAnim(string animName)
    {
        int currentAnimNum = 0;

        if (animName == "Idle")
            currentAnimNum = 0;
        else if (animName == "AttackReady")
            currentAnimNum = 1;
        else if (animName == "Attack1")
            currentAnimNum = 2;
        else if (animName == "Attack2")
            currentAnimNum = 3;
        else if (animName == "Die")
            currentAnimNum = 4;

        if(animNum != currentAnimNum)
        {
            animNum = currentAnimNum;
            changeAnim(currentAnimNum);
        }
    }

    void changeAnim(int animNum)
    {
        if (animNum == 0)
            anim.SetTrigger("Idle");
        else if (animNum == 1)
            anim.SetTrigger("AttackReady");
        else if (animNum == 2)
            anim.SetTrigger("Attack");
        else if (animNum == 3)
            anim.SetTrigger("Idle");
        else if (animNum == 4)
            anim.SetTrigger("Die");

    }
}
