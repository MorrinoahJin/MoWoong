using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dmg : MonoBehaviour
{

    public float damage;

    bool dmg;

    // Start is called before the first frame update
    void Start()
    {
        dmg = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dmg)
        {
            dmg = true;
            other.GetComponent<PlayerWoong>().TakeDamage(damage, transform.position);
        }

    }
}
