using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric_Beam_Dmg : MonoBehaviour
{

    public float damage;
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
        if (other.CompareTag("Enemy"))
        {
            Enemy[] mob = other.GetComponents<Enemy>();
            foreach (Enemy enemy in mob)
            {
                enemy.GetDamage(damage);
            }
        }

        if (other.CompareTag("TutorialBoss"))
        {
            other.GetComponent<TutorialBoss>().GetDamage(damage);
        }
    }
}
