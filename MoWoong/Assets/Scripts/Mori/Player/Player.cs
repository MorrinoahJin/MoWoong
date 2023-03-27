using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    Animator anim;
    [SerializeField]
    float playerSpeed = 5;
    //플레이어 상태(점프는 없음)
    [SerializeField]
    string playerState;  // Attack, SkillAttack, Idle, GoLeft, GoRight, Hited, Die
    int playerAnimNum;
    //회피
    bool playerShiftOn;
    float moveHorizontal;
    
    //공격 콤보
    int attackComboCount = 0;
    bool doNextAttack, checkAttack;

    //점프
    Rigidbody2D rigid;
    float jumpForce = 27;
    int jumpTIme;
    bool chakJi;

    //기본공격 범위 설정
    public GameObject attBoxObj;
    BoxCollider2D attBox;

    //플레이어 스텟
    //static public float playerHp;
    public float playerHp;

    //플레이어 무적
    bool invincibleMode = false;
    

    //플레이어 죽음관련
    bool isDead = false;
    bool isDieAnim = false;
    // Start is called before the first frame update
    void Awake()
    {
        //playerHp = 20;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    void Start()
    {
        // UnityEngine.Debug.Log("Debug message");

        rigid = GetComponent<Rigidbody2D>();
        playerState = "Idle";
        jumpTIme = 0;
        playerAnimNum = 0;

        attBox = gameObject.AddComponent<BoxCollider2D>();
        attBox.size = new Vector2(2f, 1f);
        attBox.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 컨트롤
        //죽었을때 
        /*
        if (isDead&& !isDieAnim)
        {
            die();
        }
        */
        //안 죽었을때
        /*
        if (!isDead)
        {
            //피격함수
            //어찌저찌 맞아서 체력이 0이되면 isDead가 true됨
            if(playerHp<=0)
                isDead = true;
            
        }*/
        if (playerState != "Hited" && playerState != "Die")
        {
            Attack();

            //콤보 공격 체크
            if (checkAttack)
            {
                if ((Input.GetKey("f") || Input.GetMouseButton(0)))
                    doNextAttack = true;
            }
        }
        if (playerState != "Attack" && playerState != "Hited" && playerState != "Die")
        {
            //패링함수
        }
        if (playerState != "Parrying" && playerState != "Attack" && playerState != "Hited" && playerState != "Die")
        {
            Jump();
            Move();
            //플레이어 이동키 값을 받음
            moveHorizontal = Input.GetAxisRaw("Horizontal");

            //플레이어 좌우 반전
            if (moveHorizontal < 0)
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            else if (moveHorizontal > 0)
                this.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (playerState != "GoRight" && playerState != "GoLeft" && playerState != "Jump" && playerState != "ShiftGo" && playerState != "Jump" && playerState != "Parrying" && playerState != "Attack" && playerState != "Hited" && playerState != "Die")
        {
            playerState = "Idle";
            PlayerAnim("Idle");
        }
    }

    void Attack()
    {
        if ((Input.GetKey("f") || Input.GetMouseButton(0)) && playerState != "Attack")
            StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        checkAttack = false;
        doNextAttack = false;
        playerState = "Attack";

        Debug.Log("attack");

        attackComboCount += 1;

        //공격콤보에 따라 다른 애니메이션 실행
        if (attackComboCount == 1)
            PlayerAnim("Attack1");
        else if (attackComboCount == 2)
            PlayerAnim("Attack2");
        else
            PlayerAnim("Attack3");

        Collider2D[] EnemyCollider = Physics2D.OverlapBoxAll(attBoxObj.transform.position, attBox.size, 0f);

        foreach (Collider2D collider in EnemyCollider)
        {
            if (collider.CompareTag("Enemy"))
            {
                //공격코드

            }
        }

        //공격애니메이션 끝나는 시간, 마지막 콤보 이후 콤보카운트 초기화
        if (attackComboCount == 1)
            yield return new WaitForSeconds(.4f);
        else if (attackComboCount == 2)
            yield return new WaitForSeconds(.05f);
        else
        {
            yield return new WaitForSeconds(.5f);
            attackComboCount = 0;
        }

        //공격키를 입력했는지 확인하고 입력했으면 PlayerAttack함수 실행해서 다음 공격 실행
        checkAttack = true;
        yield return new WaitForSeconds(.5f);
        if (doNextAttack)
        {
            StartCoroutine(PlayerAttack());
            if(attackComboCount == 3)
                attackComboCount = 0;
        }
        else
        {
            checkAttack = false;
            attackComboCount = 0;
            playerState = "Idle";
        }
    }

    //플레이어 이동 처리
    void FixedUpdate()
    {
        //플레이어 이동
        if (playerState != "Die" && playerState != "Hited" && playerState != "Attack" && playerState != "Parrying")
        {
            Vector3 move = new Vector3(moveHorizontal, 0f, 0f);
            transform.position += move * playerSpeed * Time.deltaTime;
        }
    }

    void Move()
    {
        //플레이어 상태에 따른 이동 변경
        if (moveHorizontal < 0 && playerState != "GoRight" && playerState != "GoDown")
        {
            if(playerState != "Jump")
            {
                PlayerAnim("MoveRight");
                playerState = "GoLeft";
            }

            if (Input.GetKeyDown("left shift") && !playerShiftOn)
            {
                StartCoroutine(ShiftGO());

            }
        }
        else if (moveHorizontal > 0 && playerState != "GoLeft" && playerState != "GoDown")
        {
            if (playerState != "Jump")
            {
                PlayerAnim("MoveRight");
                playerState = "GoRight";
            }
            if (Input.GetKeyDown("left shift") && !playerShiftOn)
            {
                StartCoroutine(ShiftGO());
            }
        }
        else
        {
            playerState = "Idle";
            PlayerAnim("Idle");
        }
    }

    IEnumerator ShiftGO()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        playerShiftOn = true;
        playerState = "ShiftGo";

        float speed = playerSpeed;
        playerSpeed = speed * 3;

        yield return new WaitForSeconds(.1f);
        rigid.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

        //애니메이션을 0.1 + 0.1초간 보여준다.
        yield return new WaitForSeconds(.1f);

        playerSpeed = speed;
        playerState = "Idle";

        yield return new WaitForSeconds(1.8f); //재사용 대기시간

        playerShiftOn = false;

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpTIme < 2)
        {
            playerState = "Jump";
            jumpTIme += 1;
            StartCoroutine(DoJump());
        }
    }

    IEnumerator DoJump()
    {
        PlayerAnim("JumpStart");
        yield return new WaitForSeconds(.06f);

        if (jumpTIme == 0)
            rigid.velocity = new Vector2(0f, jumpForce);
        else
            rigid.velocity = new Vector2(0f, jumpForce * 0.66f);

        yield return new WaitForSeconds(.27f);
        PlayerAnim("JumpMiddle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall" && jumpTIme != 0)
        {
            jumpTIme = 0;
            StartCoroutine(PlayerJumpEnd());
        }
    }

    IEnumerator PlayerJumpEnd()
    {
        PlayerAnim("Idle");
        rigid.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(.1f);
        PlayerAnim("Idle");
        playerState = "Idle";
    }

    void PlayerAnim(string playerDoAnim)
    {
        int nowAnimNum = 0;
        if (playerDoAnim == "Idle")
            nowAnimNum = 0;
        //이동
        else if (playerDoAnim == "MoveRight")
            nowAnimNum = 1;
        else if (playerDoAnim == "MoveLeft")
            nowAnimNum = 2;
        //공격
        else if (playerDoAnim == "Attack1")
            nowAnimNum = 3;
        else if (playerDoAnim == "Attack2")
            nowAnimNum = 4;
        else if (playerDoAnim == "Attack3")
            nowAnimNum = 5;
        //점프
        else if (playerDoAnim == "JumpStart")
            nowAnimNum = 6;
        else if (playerDoAnim == "JumpMiddle")
            nowAnimNum = 7;
        else if (playerDoAnim == "JumpEnd")
            nowAnimNum = 8;
        else if (playerDoAnim == "Die")
            nowAnimNum = 9;

        if (nowAnimNum != playerAnimNum)
        {
            playerAnimNum = nowAnimNum;
            PlayerAnimChange();
        }
    }
    void PlayerAnimChange()
    {
        if (playerAnimNum == 0)
            anim.SetTrigger("Idle");
        //이동
        else if (playerAnimNum == 1)
            anim.SetTrigger("RunRight");
        else if (playerAnimNum == 2)
            anim.SetTrigger("RunLeft");
        //공격
        else if (playerAnimNum == 3)
            anim.SetTrigger("FirstAttack");
        else if (playerAnimNum == 4)
            anim.SetTrigger("SecondAttack");
        else if (playerAnimNum == 5)
            anim.SetTrigger("LastAttack");
        //점프
        else if (playerAnimNum == 6)
            anim.SetTrigger("JumpStart");
        else if (playerAnimNum == 7)
            anim.SetTrigger("JumpMiddle");
        else if (playerAnimNum == 8)
            anim.SetTrigger("JumpEnd");
        else if (playerAnimNum == 9)
            anim.SetTrigger("Die");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            float damage = enemy.enemyAtkPower;
            float nextDamageTime=0.5f;
            if (!invincibleMode)
            {
                hit(damage);
                StartCoroutine(BeInvincible(nextDamageTime));
            }
           
        }
    }
    //피격 및 데미지 
    void hit(float damage)
    {
        playerHp -= damage;
        if(playerHp <= 0)
        {
            die();
        }
    }
   
    //무적
    private IEnumerator BeInvincible(float invincibleTime)
    {
        invincibleMode = true;
        yield return new WaitForSeconds(invincibleTime);
        invincibleMode = false;
    }
    void die()
    {
       
        isDieAnim = true;
        UnityEngine.Debug.Log("플레이어가 죽었습니다.");
        playerState = "Die";
        PlayerAnim("Die");
        //Invoke("StopScript", 2f);
        //rigid.velocity = new Vector2(transform.position.x, 0); //죽으면 y좌표를 고정하게 하기
        //GetComponent<Player>().enabled = false; //플레이어 키입력 제한하기

    }
    void StopScript()
    {
       //플레이어 사망시 스크립트 멈추기
        enabled = false;
        UnityEngine.Debug.Log("스크립트가 종료되었습니다.");
    }
}
