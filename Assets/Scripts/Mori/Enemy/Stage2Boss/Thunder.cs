using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public GameObject dmg;

    // Start is called before the first frame update
    void Start()
    {
        dmg.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DmgSetActiveTrue()
    {
        dmg.SetActive(true);
    }

    public void DmgSetActiveFalse()
    {
        dmg.SetActive(false);
    }

    public void DestroyObj()
    {
        Destroy(gameObject, .33f);
    }
}
