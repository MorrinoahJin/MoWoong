using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer sprite;
    bool chageColor;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        chageColor = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage()
    {
        if (!chageColor)
        {
            Debug.Log("Attacked");
            StartCoroutine(ChangeEnemyColor());
        }
    }

    IEnumerator ChangeEnemyColor()
    {

        chageColor = true;
        sprite.color = Color.red;

        yield return new WaitForSeconds(.66f);

        sprite.color = Color.white;
        chageColor = false;
    }
}
