using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Beam : MonoBehaviour
{
    public GameObject step1, step2, step3, step4;

    // Start is called before the first frame update
    void OnEnable()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        step4.SetActive(false);
        StartCoroutine(StartBeam());
    }

    void Update()
    {
        
    }
    IEnumerator StartBeam()
    {
        //플레이어 무적 코드 넣기

        yield return new WaitForSeconds(.2f);
        step1.SetActive(true);

        yield return new WaitForSeconds(.3f);
        step2.SetActive(true);
        step3.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        step4.SetActive(true);
        yield return new WaitForSeconds(1.7f);
        step3.SetActive(false);
        yield return new WaitForSeconds(.1f);
        step4.SetActive(false);

        yield return new WaitForSeconds(.1f);
        step2.SetActive(false);

        yield return new WaitForSeconds(.1f);
        step1.SetActive(false);
        //플레이어 무적해제 코드 넣기
    }

}
