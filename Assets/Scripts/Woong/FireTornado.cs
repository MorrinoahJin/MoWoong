using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTornado : MonoBehaviour
{
    //GameObject orb;
    public GameObject step1, step2,step3;
    bool fireMove;
    float direction;
    void OnEnable()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
      
        StartCoroutine(StartFireTornado());
    }
    private void Start()
    {
        fireMove = false;
       
    }
    void Update()
    {        
        
        if (fireMove)
        {
            //앞으로나아감
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(100f, 0, 0), Orb.orbDirection*3f * Time.deltaTime);
        }
    }
    IEnumerator StartFireTornado()
    {
        //Orb.isSkillOn = true;
        //플레이어 무적 코드 넣기
        yield return new WaitForSeconds(.1f);
        step1.SetActive(true);
        yield return new WaitForSeconds(.7f);
        step1.SetActive(false);
        step2.SetActive(true);
        fireMove = true;
        yield return new WaitForSeconds(3.0f);
        fireMove = false;
        step2.SetActive(false);
        step3.SetActive(true);
        yield return new WaitForSeconds(.1f);
        step3.SetActive(false) ;
        //플레이어 무적해제 코드 넣기
        //Orb.isSkillOn = false;
        Destroy(gameObject);
    }
}
