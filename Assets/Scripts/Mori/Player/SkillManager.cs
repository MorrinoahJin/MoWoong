using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    GameObject curSkill;
    static public GameObject[] skill;
    public GameObject orb;
    Vector3 orbPos;
    float player_direction;
    public GameObject razorbeam;
    public GameObject fireTornado;
    float direction;
    [SerializeField] private GameObject UI_Orb_Fire;
    [SerializeField] private GameObject UI_Orb_Electric;
    // static public bool fireMove;
    // Start is called before the first frame update
    void Start()
    {
        skill = new GameObject[2];
        skill[0] = razorbeam;
        skill[1] = fireTornado;
        curSkill = skill[0];
        if(UI_Orb_Electric != null) { UI_Orb_Electric.SetActive(false); }
        if(UI_Orb_Fire != null) { UI_Orb_Fire.SetActive(true); }
     
    }

    // Update is called once per frame
    void Update()
    {
        orbPos = orb.transform.position;
        player_direction = GameObject.Find("Player").GetComponent<PlayerWoong>().moveHorizontal;
        direction = orb.GetComponent<Orb>().DirectionOrb();
      
    }
    public void skillChange()
    {
        if (curSkill != null)
        {
            if (curSkill == skill[0])
            {
                UI_Orb_Fire.SetActive(true);
                UI_Orb_Electric.SetActive(false);
              

                GameObject.Find("Orb").GetComponent<Orb>().nowForm(1);
                UnityEngine.Debug.Log("스킬전환: 레이저 빔");
                curSkill = skill[1];
            }
            else
            {
                UI_Orb_Electric.SetActive(true);
                UI_Orb_Fire.SetActive(false);
               
                GameObject.Find("Orb").GetComponent<Orb>().nowForm(0);
                UnityEngine.Debug.Log("스킬전환: 파이어토네이도");
                curSkill = skill[0];
            }
        }
    }
    IEnumerator OrbSkillOn(float time)
    {
        Orb.isSkillOn = true;
        yield return new WaitForSeconds(time);
        Orb.isSkillOn = false;
    }
    public void skillIndex()
    {
        if (curSkill == skill[0])
        {
            RazorBeam();
            StartCoroutine(OrbSkillOn(4f));
        }
        else if (curSkill==skill[1])
        {
            FireTornade();
            StartCoroutine(OrbSkillOn(3f));
        }
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
    public void FireTornade()
    {
        if (direction == 1)
        {
            Instantiate(fireTornado, orbPos, Quaternion.identity);
        }
        else
        {
            Instantiate(fireTornado, orbPos, Quaternion.Euler(new Vector3(0, 180, 0)));
        }
    }
   
}
