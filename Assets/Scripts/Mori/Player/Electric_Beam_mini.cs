using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Beam_mini : MonoBehaviour
{
    public GameObject step3, step4;

    // Start is called before the first frame update
    void OnEnable()
    {
        step3.SetActive(false);
        step4.SetActive(false);
        StartCoroutine(StartBeam());
    }

    void Update()
    {
        
    }
    IEnumerator StartBeam()
    {
        Orb.isSkillOn = true;
        //플레이어 무적 코드 넣기
        yield return new WaitForSeconds(.5f);
        step3.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        step4.SetActive(true);
        yield return new WaitForSeconds(1.7f);
        step3.SetActive(false);
        yield return new WaitForSeconds(.1f);
        step4.SetActive(false);
        yield return new WaitForSeconds(.2f);
        //플레이어 무적해제 코드 넣기
        Orb.isSkillOn = false;
        Destroy(gameObject);
    }

}
