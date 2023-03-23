using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Attack,
    Move,
    Hited,
}


public class Enemy : MonoBehaviour
{
    EnemyState currentState;

    SpriteRenderer sprite;
    bool changeColor;
    
    [SerializeField]
    string enmeyMoveDirection; // left or right
    
    public bool iDontCareHited;
    
    bool thinkTime, attTime;
    
    float speed = 3f;
    Vector2 playerPos;
    float playerDistance;
    float attDistance = 4f;
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
        switch (currentState)
        {
            case EnemyState.Attack:
                EnemyAttack();
                break;
            case EnemyState.Move:
                {
                    if (!attTime)
                        EnemyMoving();
                    else
                        ChangeState(EnemyState.Attack);
                }
                EnemyMoving();
                break;
            case EnemyState.Hited:
                GetDamage();
                break;
            default:
                break;
        }

        //공격 범위안에 들어오면 공격하게 함
        playerPos = GameObject.FindWithTag("Player").transform.position;
        playerDistance = Vector2.Distance(playerPos, this.transform.position);
        if (attDistance <= playerDistance)
            attTime = true;
        else
            attTime = false;
        
    }
    void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }

    void EnemyMoving()
    {
        RaycastHit2D checkFloor;
        Vector2 rayPos = new Vector2();

        if (!thinkTime)
            StartCoroutine(ThinkMove());

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
        else if(enmeyMoveDirection == "Center")
        {
            //idle애니메이션 실행
        }
    }
    //일정 시간이 지나면 이동방향을 새로 바꿈
    IEnumerator ThinkMove()
    {
        thinkTime = true;
        int num = Random.Range(1,4);
        switch(num)
        {
            case 1:
                enmeyMoveDirection = "Left";
                break;
            case 2:
                enmeyMoveDirection = "Right";
                break;
            case 3:
                enmeyMoveDirection = "Center";
                break;
        }
        yield return new WaitForSeconds(3f);
        thinkTime = false;
    }

    IEnumerator EnemyAttack()
    {
        //공격 애니메이션 실행
        yield return new WaitForSeconds(1f);
        //공격범위 안에 들어 와 있을 경우 데미지를 입힘
        if (attTime)
        {

        }
        else
            ChangeState(EnemyState.Move);
        
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
        //현재 이동방향을 저장
        string direction = enmeyMoveDirection;
        enmeyMoveDirection = "Null";
        Vector2 playerPos = GameObject.FindWithTag("Player").transform.position;
        RaycastHit2D checkFloor;
        Vector2 rayPos = new Vector2();

        //피격모션이 있는 몹일 경우
        if (!iDontCareHited)
        {
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
        }   
        
        changeColor = true;
        sprite.color = Color.red;

        yield return new WaitForSeconds(.66f);

        sprite.color = Color.white;
        changeColor = false;

        //상태변경
        if (!attTime)
            ChangeState(EnemyState.Move);
        else
            ChangeState(EnemyState.Attack);
    }
    
}
