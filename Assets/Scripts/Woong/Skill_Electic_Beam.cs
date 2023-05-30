using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Electic_Beam : MonoBehaviour
{
    Vector3 orbPos;
    public GameObject electric_beamPrefab;
    Vector2 skill1size;
    // Start is called before the first frame update
    void Start()
    {
        skill1size = new Vector2(5f, 1f);
        orbPos = GameObject.FindWithTag("Orb").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skill1(float AtkPower)
    {

        Instantiate(electric_beamPrefab, orbPos, Quaternion.identity);
        Destroy(gameObject, 2f);
        Collider2D[] EnemyCollider = Physics2D.OverlapBoxAll(orbPos, skill1size, 0f);
        foreach (Collider2D collider in EnemyCollider)
        {
            if (collider.CompareTag("Enemy"))
            {
                //공격코드
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.GetDamage(AtkPower);
            }
            /*
            if (collider.CompareTag("TutorialBoss"))
            {
                //공격코드
                collider.GetComponent<TutorialBoss>().GetDamage(AtkPower);
            }*/

        }
    }
    private void DestroySkill1()
    {
        
    }
}
