using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer sprite;
    bool changeColor;
    string enemyState; // att or idle or hited(?) or die
    [SerializeField]
    string enmeyMoveDirection; // left or right
    public bool iDontCareHited;

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
            //������Ʈ ���� �ؿ� �ٴ��� ������ Ȯ��
            rayPos = new Vector2(this.transform.position.x - .5f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            //�ٴ� ������ �������� �̵�
            if (checkFloor.collider != null)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, rayPos, speed * Time.deltaTime);
            }
            //�ٴ��� ������ ���������� �̵�
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

        //���� �÷��̾� ���� �����ʿ� ���� �� ������ ���ϸ� ���������� �и�
        if (transform.position.x >= playerPos.x)
        {
            rayPos = new Vector2(this.transform.position.x + 1f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            //�ٴ� ������ ���������� �̵�
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
