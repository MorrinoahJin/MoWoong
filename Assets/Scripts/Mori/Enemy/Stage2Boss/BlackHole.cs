using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public GameObject dmg;

    Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        dmg.SetActive(false);

        if (GameObject.Find("Player") != null)
            playerPos = GameObject.Find("Player").transform.position;

        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos, 2.75f * Time.deltaTime);
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
