using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAnimTime : MonoBehaviour
{
    Animator anim;

    float attackAnimTime, hitedAnimTIme, DieAnimTime;
    bool attackTime, hitedTime, dieTime;
    public bool checkOne; //체크할 애니메이션이 하나밖에 없을 때
    float time;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attackAnimTime = 0;
        hitedAnimTIme = 0;
        DieAnimTime = 0;
        time = 0;

        //체크할 애니메이션이 여러개일 때
        if (!checkOne)
            StartCoroutine(doAnim());
    }

    // Update is called once per frame
    void Update()
    {
        //하나의 애니메이션 체크
        if(checkOne)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            time += Time.deltaTime;
            if (stateInfo.normalizedTime >= 1.0f)
            {
                Debug.Log(time);
                this.gameObject.SetActive(false);
            }
        }

        //공격, 피격, 죽음 애니메이션 체크
        if(attackTime)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            attackAnimTime += Time.deltaTime;
            if (stateInfo.normalizedTime >= 1.0f)
            {
                attackTime = false;
                Debug.Log("공격시간" + attackAnimTime);
            }
        }
        if (hitedTime)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            hitedAnimTIme += Time.deltaTime;
            if (stateInfo.normalizedTime >= 1.0f)
            {
                hitedTime = false;
                Debug.Log("피격시간" + hitedAnimTIme);
            }
        }
        if (dieTime)
        {
            AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            DieAnimTime += Time.deltaTime;
            if (stateInfo.normalizedTime >= 1.0f)
            {
                dieTime = false;
                Debug.Log("죽음시간"+DieAnimTime);
            }
        }
    }

    IEnumerator doAnim()
    {
        
        anim.SetTrigger("Attack");
        attackTime = true;
        yield return new WaitForSeconds(3f);

        anim.SetTrigger("Die");
        dieTime = true;
        yield return new WaitForSeconds(3f);

        /*
        anim.SetTrigger("Hited");
        hitedTime = true;
        yield return new WaitForSeconds(3f);
        */
    }
}
