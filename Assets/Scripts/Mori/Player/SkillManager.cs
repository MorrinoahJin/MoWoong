using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject orb;
    Vector3 orbPos;
    float player_direction;
    public GameObject razorbeam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        orbPos = orb.transform.position;
        player_direction = GameObject.Find("Player").GetComponent<PlayerWoong>().moveHorizontal;
    }

    public void RazorBeam()
    {
        if(player_direction >= 0)
        {
            Instantiate(razorbeam, orbPos, Quaternion.identity);
        }
        else
        {
            Instantiate(razorbeam, orbPos, Quaternion.identity);
        }
    }
}
