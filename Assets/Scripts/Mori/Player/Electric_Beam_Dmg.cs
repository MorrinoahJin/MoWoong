using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Beam_Dmg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "TutorialBoss")
        {
            other.GetComponent<TutorialBoss>().GetDamage(5500);
            Debug.Log("asdf");
        }
    }
}
