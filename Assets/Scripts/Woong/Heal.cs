using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public GameObject healFX;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void HealFX()
    {
        Vector3 playerPosition = Player.transform.position;
       // GameObject clone = Instantiate(healFX, playerPosition, Quaternion.identity);
        healFX.SetActive(true);
    }
    public void falseHealFX()
    {
        healFX.SetActive(false);
    }
}
