using System.Collections;
using UnityEngine;

public enum EnemyState
{
    Stop,
    Die,
    Chase,
    Attack,
    Move,
    Hited,
}

public class Enemy : MonoBehaviour
{
    [SerializeField]
    EnemyState currentState;

    SpriteRenderer sprite;
    bool canGetDamage; //몹이 데미지를 받을 수 있는 상태인지 판별하는 변수
    
    string enemyMoveDirection; // left or right
    
    public bool iDontCareHited; //피격모션 유무를 제어하기 위한 변수

    bool thinkTime; //이동상태를 결정중인지 확인하는 변수
    bool playerInAttRange; //플레이어가 공격 범위 안에 들어왔는지 확인
    bool playerInChaseRange; //플레이어가 탐지 범위 안에 들어 왔는지를 확인
    bool attacking; //공격동작 중 인지 확인
    bool isStop; //모든 동작을 중지 시키는 변수
    RaycastHit2D checkFloor, checkLeftSide, checkRightSide; //바닥확인, 왼쪽, 오른쪽에 벽이 위치해 있는지를 확인
    Vector2 rayPos; //checkFloor레이를 쏠 위치

    Animator anim;

    //종류
    public bool IsLongRangeAtt; //원거리 공격을 하는 몹인지를 판별
    public GameObject bullet; //발사체

    //스탯
    Vector2 playerPos; //플레이어의 위치
    float playerDistance, playerDistanceY; //플레이어와의 거리
    public float ChaseDistance; //탐지 범위
    int previousAnimNum; //작동되는 애니메이션 넘버
    public float attDistance, enemyAtkPower, speed; //공격데미지 , 공격사거리, 이동속도
    public float hp;
    public float attTime, damageTime, hitedTime, dieTime; //공격시간, 데미지를 입히는 시간, 피격시간, 죽을 때 시간

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        canGetDamage = false;
        attacking = false;

        // 몹의 기본상태 = 이동상태
        ChangeState(EnemyState.Move);
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            ChangeState(EnemyState.Die);
            enemyMoveDirection = "Center";
        }

        if(isStop)
        {
            ChangeState(EnemyState.Stop);
        }


        switch (currentState)
        {
            case EnemyState.Stop:
                break;
            case EnemyState.Die:
                {
                    DoAnim("Die");
                    Destroy(this.gameObject, 2f);
                }
                break;
            case EnemyState.Hited:
                break;
            case EnemyState.Attack:
                {
                    if (!attacking)
                        StartCoroutine(EnemyAttack());
                }
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
        //y축이 다를 때 공격을 추격상태가 되지않는다.

        //y축끼리의 좌표를 구해서 y축이 멀어지면 
        playerDistanceY = Mathf.Abs(playerPos.y - transform.position.y);
        if (playerDistanceY > 1)
            playerDistance = 100; //몹 인식거리에 플레이어가 들어오지 않게 변경

        if (ChaseDistance >= playerDistance)
        {
            playerInChaseRange = true;
            if (attDistance >= playerDistance)
            {
                playerInAttRange = true;
            }
            else
                playerInAttRange = false;

            if(transform.position.x > playerPos.x) //오른쪽에 위치해 있을 때 좌우 반전
                transform.localEulerAngles = new Vector3(0, 0, 0); //좌우 반전
            else
                transform.localEulerAngles = new Vector3(0, 180, 0); //좌우 반전
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
        DoAnim("Move");
        Vector2 targetPos = new Vector2(playerPos.x, this.transform.position.y);

        RaycastHit2D LeftSide = Physics2D.Raycast(transform.position, Vector2.left, 1f, LayerMask.GetMask("Ground")), RightSide = Physics2D.Raycast(transform.position, Vector2.left, 1f, LayerMask.GetMask("Ground"));

        Vector2 leftBottumPos = new Vector2(transform.position.x - .5f, this.transform.position.y), rightBottumPos = new Vector2(transform.position.x + .5f, this.transform.position.y);

        RaycastHit2D checkLeftFloor = Physics2D.Raycast(leftBottumPos, Vector2.down, 1f, LayerMask.GetMask("Ground")), checkRightFloor = Physics2D.Raycast(rightBottumPos, Vector2.down, 1f, LayerMask.GetMask("Ground"));

        if (LeftSide.collider == null && RightSide.collider == null && checkLeftFloor.collider != null && checkRightFloor.collider != null)
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
           

        if (!playerInChaseRange)
            ChangeState(EnemyState.Move);
    }
    //이동함수
    void EnemyMoving()
    {
        if (!thinkTime)
            StartCoroutine(ThinkMove());

        if (enemyMoveDirection == "Left")
        {
            DoAnim("Move"); //애니메이션 실행
            transform.localEulerAngles = new Vector3(0, 0, 0); //좌우 반전

            //오브젝트 왼쪽 밑에 바닥이 없는지 확인
            rayPos = new Vector2(this.transform.position.x - .5f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Ground"));
            checkLeftSide = Physics2D.Raycast(transform.position, Vector2.left, 1f, LayerMask.GetMask("Ground"));

            //바닥 있으면 왼쪽으로 이동
            if (checkFloor.collider != null && checkLeftSide.collider == null) 
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, rayPos, speed * Time.deltaTime);
            }
            //바닥이 없으면 오른쪽으로 이동
            else
                enemyMoveDirection = "Right";

        }
        else if(enemyMoveDirection == "Right")
        {
            DoAnim("Move"); //애니메이션 실행
            transform.localEulerAngles = new Vector3(0, 180, 0); //좌우 반전

            rayPos = new Vector2(this.transform.position.x + .5f, this.transform.position.y);
            checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Ground"));
            checkRightSide = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Ground"));

            if (checkFloor.collider != null && checkRightSide.collider == null)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, rayPos, speed * Time.deltaTime);
            }
            else
                enemyMoveDirection = "Left";
        }
        else if(enemyMoveDirection == "Center")
        {
            DoAnim("Idle");//idle애니메이션 실행
        }

        if (playerInChaseRange) //플레이어가 인식 범위 안에 들어와 있을 때 플레이러를 향해 이동
        {
            ChangeState(EnemyState.Chase);
        }
    }
    //일정 시간이 지나면 이동방향을 새로 바꿈
    IEnumerator ThinkMove()
    {
        thinkTime = true;
        int num = Random.Range(1,9);
        switch(num)
        {
            case 1:
            case 2:
            case 3:
                enemyMoveDirection = "Left";
                break;
            case 4:
            case 5:
            case 6:
                enemyMoveDirection = "Right";
                break;
            case 7:
            case 8:
                enemyMoveDirection = "Center";
                break;
        }
        yield return new WaitForSeconds(2f);
        thinkTime = false;
    }
    //공격함수
    IEnumerator EnemyAttack()
    {
        attacking = true;
        yield return new WaitForSeconds(.33f);
        if (!IsLongRangeAtt) //근접 공격일 경우, 애니메이션에 따라 데미지를 입히는 시간을 설정
        {
            ChangeAnim(2);
            yield return new WaitForSeconds(damageTime);
            //공격범위 안에 들어 와 있을 경우 데미지를 입힘 아닐 경우 이동 실행
            if (playerInAttRange)
            {
                GameObject.FindWithTag("Player").GetComponent<PlayerWoong>().TakeDamage(enemyAtkPower, transform.position);
                Debug.Log("공격");
            }
            yield return new WaitForSeconds(attTime-damageTime);

            ChangeState(EnemyState.Move);
            DoAnim("Move");
            attacking = false;
        }
        else //원거리 공격
            StartCoroutine(LongRangeAttack());
    }
    //공격함수 - 원거리공격함수
    IEnumerator LongRangeAttack()
    {
        ChangeAnim(2); //공격 애니메이션 실행
        yield return new WaitForSeconds(damageTime); //발사체 나오는 시간 설정
        Debug.Log("원거리 공격"); //발사체발사
        yield return new WaitForSeconds(attTime - damageTime); //애니메이션 끝나는 시간
        ChangeState(EnemyState.Move);
        attacking = false;

    }
    //피격함수
    public void GetDamage(float damage)
    {
        if (!canGetDamage && currentState != EnemyState.Die)
        {
            ChangeState(EnemyState.Hited); //스테이트 변경
            hp -= damage;
            StartCoroutine(ChangeEnemyColor());
            if (hp < 0)
                ChangeState(EnemyState.Die);
        }
    }
    //피격처리 함수
    IEnumerator ChangeEnemyColor() //피격시 색 변경, 피격애니메이션 실행
    {
        canGetDamage = true;
        sprite.color = Color.red;

        //현재 이동방향을 저장
        string direction = enemyMoveDirection;
        enemyMoveDirection = "Null";
        isStop = true;

        //피격모션이 있는 몹일 경우
        if (!iDontCareHited)
        {
            DoAnim("Hited");
            //몹이 플레이어 보다 오른쪽에 있을 때 공격을 당하면 오른쪽으로 밀림
            if (transform.position.x >= playerPos.x)
            {
                rayPos = new Vector2(this.transform.position.x + 1f, this.transform.position.y);
                checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Ground"));
                checkLeftSide = Physics2D.Raycast(transform.position, Vector2.left, 1f, LayerMask.GetMask("Ground"));
                //바닥 있으면 오른쪽으로 이동
                if (checkFloor.collider != null && checkLeftSide.collider == null)
                {
                    transform.position = new Vector2(this.transform.position.x + .66f, transform.position.y);
                    Debug.Log("오른쪽 피격");
                }
            }
            else //몹이 플레이어보다 왼쪽에 있을 경우
            {
                rayPos = new Vector2(this.transform.position.x - 1f, this.transform.position.y);
                checkFloor = Physics2D.Raycast(rayPos, Vector2.down, 1f, LayerMask.GetMask("Ground"));
                checkRightSide = Physics2D.Raycast(transform.position, Vector2.right, 1f, LayerMask.GetMask("Ground"));
                if (checkFloor.collider != null && checkRightSide.collider == null)
                {
                    transform.position = new Vector2(this.transform.position.x - .66f, transform.position.y);
                    Debug.Log("왼쪽 피격");
                }
            }
            yield return new WaitForSeconds(hitedTime - .33f); //피격모션 실행시간
        }
        yield return new WaitForSeconds(.33f);
        isStop = false;
        //피격모션 끝, 변수 초기화
        sprite.color = Color.white;
        canGetDamage = false;

        //상태변경
        if (playerInChaseRange)
            ChangeState(EnemyState.Chase);
        else
            ChangeState(EnemyState.Move);
    }

    void DoAnim(string animName)
    {
        int currentAnimNum = 0;

        if (animName == "Idle")
            currentAnimNum = 0;
        else if(animName == "Move")
            currentAnimNum = 1;
        else if (animName == "Attack")
            currentAnimNum = 2;
        else if (animName == "Hited")
            currentAnimNum = 3;
        else if (animName == "Die")
            currentAnimNum = 4;

        if (currentAnimNum != previousAnimNum)
            ChangeAnim(currentAnimNum);
    }

    void ChangeAnim(int animNum)
    {
        previousAnimNum = animNum;

        if (animNum == 0)
            anim.SetTrigger("Idle");
        else if (animNum == 1)
            anim.SetTrigger("Move");
        else if(animNum == 2)
            anim.SetTrigger("Attack");
        else if (animNum == 3)
            anim.SetTrigger("Hited");
        else if (animNum == 4)
            anim.SetTrigger("Die");
    }


}
