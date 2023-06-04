using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public GameObject orb;
    Vector3 orbPos;
    float player_direction;
    public GameObject razorbeam;
    float direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        orbPos = orb.transform.position;
        player_direction = GameObject.Find("Player").GetComponent<PlayerWoong>().moveHorizontal;
        direction = orb.GetComponent<Orb>().DirectionOrb();
    }

    public void RazorBeam()
    {
        //if(player_direction > 0)
        if(direction == 1)
        {
            Instantiate(razorbeam, orbPos, Quaternion.identity);
        }
        else
        {
            Instantiate(razorbeam, orbPos, Quaternion.Euler(new Vector3(0, 180, 0)));
        }
    }
}
