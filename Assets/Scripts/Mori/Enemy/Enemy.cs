using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Die,
    Chase,
    Attack,
    Move,
    Hited,
}

public class Enemy : MonoBehaviour
{
    EnemyState currentState;

    SpriteRenderer sprite;
    bool canGetDamage; //몹이 데미지를 받을 수 있는 상태인지 판별하는 변수
    
    [SerializeField]
    string enemyMoveDirection; // left or right
    
    public bool iDontCareHited; //피격모션 유무를 제어하기 위한 변수

    bool thinkTime; //이동상태를 결정중인지 확인하는 변수
    bool playerInAttRange; //플레이어가 공격 범위 안에 들어왔는지 확인
    bool playerInChaseRange; //플레이어가 탐지 범위 안에 들어 왔는지를 확인
    bool attacking; //공격동작 중 인지 확인

    RaycastHit2D checkFloor, checkLeftSide, checkRightSide; //바닥확인, 왼쪽, 오른쪽에 벽이 위치해 있는지를 확인
    Vector2 rayPos; //checkFloor레이를 쏠 위치

    //종류
    [SerializeField]
    bool IsLongRangeAtt; //원거리 공격을 하는 몹인지를 판별
    public GameObject bullet; //발사체

    //스탯
    float speed = 3f; //몹의 이동속도
    Vector2 playerPos; //플레이어의 위치
    float playerDistance; //플레이어와의 거리
    float ChaseDistance = 3f; //탐지 범위
    float attDistance = 1f; //공격 범위
    public float enemyAtkPower = 0; //공격데미지
    float hp = 100;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        canGetDamage = false;
        attacking = false;

        // 몹의 기본상태 = 이동상태
        currentState = EnemyState.Move;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Die:
                break;
            case EnemyState.Hited:
                break;
            case EnemyState.Attack:
                EnemyAttack();
                break;
            case EnemyState.Chase:
                {
                    if (!playerInAttRange)
                        ChasePlayer();
                    else
                        ChangeState(EnemyState.Attack);
                }
                break;
            case EnemyState.Move:
                {
                    if (!playerInChaseRange)
                        EnemyMoving();
                    else
                        ChangeState(EnemyState.Chase);
                }
                break;
            default:
                break;
        }

        //공격 범위안에 플레이어간 들어오면 공격하게 함
        playerPos = GameObject.FindWithTag("Player").transform.position;
        playerDistance = Vector2.Distance(playerPos, this.transform.position);
        if(ChaseDistance <= playerDistance)
        {
            playerInChaseRange = true;
            if (attDistance <= playerDistance)
                playerInAttRange = true;
            else
                playerInAttRange = false;
        }
        else
            playerInChaseRange = false;
    }
    //상태 변경하는 함수
    void ChangeState(EnemyState newState)
    {
        currentState = newState;
    }
    //플레이어가 공격 범위 안에 들어와 있을 때 공격 상태로 변경
    void ChasePlayer()
    {
        if (playerInAttRange)
            ChangeState(EnemyState.Attack);
    }

    void EnemyMoving()
    {
        if (!thinkTime)
            StartCoroutine(ThinkMove());

        if (enemyMoveDirection == "Left")
        {
            transform.localEulerAngles = new Vector3(0, 0, 0); //좌우 반전
            //오브젝트 왼쪽 밑에 바닥이 없는지 확인
            rayPos = new Vector2(this.transform.position.x - .5f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            checkLeftSide = Physics2D.Raycast(transform.position, Vector2.left, .5f, LayerMask.GetMask("Wall"));

            //바닥 있으면 왼쪽으로 이동
            if (checkFloor.collider != null && checkLeftSide.collider != null) 
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, rayPos, speed * Time.deltaTime);
            }
            //바닥이 없으면 오른쪽으로 이동
            else
                enemyMoveDirection = "Right";

        }
        else if(enemyMoveDirection == "Right")
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);  //좌우 반전
            rayPos = new Vector2(this.transform.position.x + .5f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
            checkRightSide = Physics2D.Raycast(transform.position, Vector2.right, .5f, LayerMask.GetMask("Wall"));

            if (checkFloor.collider != null && checkRightSide.collider != null)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, rayPos, speed * Time.deltaTime);
            }
            else
                enemyMoveDirection = "Left";
        }
        else if(enemyMoveDirection == "Center")
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
                enemyMoveDirection = "Left";
                break;
            case 2:
                enemyMoveDirection = "Right";
                break;
            case 3:
                enemyMoveDirection = "Center";
                break;
        }
        yield return new WaitForSeconds(3f);
        thinkTime = false;
    }

    IEnumerator EnemyAttack()
    {
        attacking = true;
        if (!IsLongRangeAtt) //근접 공격일 경우, 애니메이션에 따라 데미지를 입히는 시간을 설정
        {
            //공격 애니메이션 실행
            yield return new WaitForSeconds(1f);
        }
        //공격범위 안에 들어 와 있을 경우 데미지를 입힘 아닐 경우 이동 실행
        if (playerInAttRange)
        {
            if (!IsLongRangeAtt) //근접 공격
            {
                //공격실행
                Debug.Log("공격");
            }
            else //원거리 공격
                StartCoroutine(LongRangeAttack());
        }
        else
            ChangeState(EnemyState.Move);

        yield return null;
        attacking = false;
    }

    IEnumerator LongRangeAttack()
    {
        //공격 애니메이션 실행
        yield return new WaitForSeconds(1f); //발사체 나오는 시간 설정
        Debug.Log("원거리 공격");
    }

    public void GetDamage(float damage)
    {
        if (!canGetDamage)
        {
            hp -= damage;
            StartCoroutine(ChangeEnemyColor());
            if (hp < 0)
                ChangeState(EnemyState.Die);
        }
    }

    IEnumerator ChangeEnemyColor() //피격시 색 변경, 피격애니메이션 실행
    {
        canGetDamage = true;
        sprite.color = Color.red;

        //현재 이동방향을 저장
        string direction = enemyMoveDirection;
        enemyMoveDirection = "Null";
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
            else //몹이 플레이어보다 왼쪽에 있을 경우
            {
                rayPos = new Vector2(this.transform.position.x - 1f, this.transform.position.y);
                checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Wall"));
                if (checkFloor.collider != null)
                {
                    transform.position = new Vector2(this.transform.position.x - .66f, transform.position.y);
                }
            }
        }

        yield return new WaitForSeconds(.66f);

        sprite.color = Color.white;
        canGetDamage = false;

        //상태변경
        if (!playerInAttRange)
            ChangeState(EnemyState.Move);
        else
            ChangeState(EnemyState.Attack);
    }
    
}
