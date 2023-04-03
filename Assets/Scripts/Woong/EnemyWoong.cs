using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWoong : MonoBehaviour
{
    SpriteRenderer sprite;
    bool changeColor;
    string enemyState; // att or idle or hited(?) or die
    [SerializeField]
    string enmeyMoveDirection; // left or right
    public bool iDontCareHited;
    public float enemyAtkPower = 10;
    float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        changeColor = false;

        int num = Random.Range(0, 2);
        if (num == 0)
            enmeyMoveDirection = "Left";
        else
            enmeyMoveDirection = "Right";

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        EnemyMoving();

    }

    void EnemyMoving()
    {
        RaycastHit2D checkFloor;
        Vector2 rayPos = new Vector2();

        if (enmeyMoveDirection == "Left")
        {
            //오브젝트 왼쪽 밑에 바닥이 없는지 확인
            rayPos = new Vector2(this.transform.position.x - .5f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            //바닥 있으면 왼쪽으로 이동
            if (checkFloor.collider != null)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, rayPos, speed * Time.deltaTime);
            }
            //바닥이 없으면 오른쪽으로 이동
            else
                enmeyMoveDirection = "Right";

        }
        else if(enmeyMoveDirection == "Right")
        {
            rayPos = new Vector2(this.transform.position.x + .5f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            if (checkFloor.collider != null)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, rayPos, speed * Time.deltaTime);
            }
            else
                enmeyMoveDirection = "Left";
        }
    }

    public void GetDamage()
    {
        if (!changeColor)
        {
            //hp - damage;
            StartCoroutine(ChangeEnemyColor());
        }
    }

    IEnumerator ChangeEnemyColor()
    {
        string direction = enmeyMoveDirection;
        enmeyMoveDirection = "Null";
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
        RaycastHit2D checkFloor;
        Vector2 rayPos = new Vector2();

        //몹이 플레이어 보다 오른쪽에 있을 때 공격을 당하면 오른쪽으로 밀림
        if (transform.position.x >= playerPos.x)
        {
            rayPos = new Vector2(this.transform.position.x + 1f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            //바닥 있으면 오른쪽으로 이동
            if (checkFloor.collider != null)
            {
                transform.position = new Vector2(this.transform.position.x + .66f, transform.position.y);
            }
        }
        else
        {
            rayPos = new Vector2(this.transform.position.x - 1f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            if (checkFloor.collider != null)
            {
                transform.position = new Vector2(this.transform.position.x - .66f, transform.position.y);
            }

        }
        changeColor = true;
        sprite.color = Color.red;

        yield return new WaitForSeconds(.66f);

        sprite.color = Color.white;
        changeColor = false;

        if(direction != "Null")
            enmeyMoveDirection = direction;
        else
        {
            int num = Random.Range(0, 2);
            if (num == 0)
                enmeyMoveDirection = "Left";
            else
                enmeyMoveDirection = "Right";
        }
    }
    
}
