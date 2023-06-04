using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage2MapController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(doUpDownSwitch());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator doUpDownSwitch()
    {
        yield return new WaitForSeconds(2f);
        CameraMoving.mirrorMod = true;
    }
}
