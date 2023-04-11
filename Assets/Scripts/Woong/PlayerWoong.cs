using System.Collections;
using UnityEngine;


public class PlayerWoong: MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    Animator anim;
    [SerializeField]
    float playerSpeed = 5;
    int playerLayer,passableGroundLayer;
   
    //플레이어 상태(점프는 없음)
    [SerializeField]
    string playerState;  // Attack, SkillAttack, Idle, GoLeft, GoRight, Hited, Die
    int playerAnimNum;
    //회피
    bool playerShiftOn;
    float moveHorizontal;

    //공격 및 공격 콤보
    public float playerAtkPower = 10;
    int attackComboCount = 0;
    bool doNextAttack, checkAttack;

    //점프
    Rigidbody2D rigid;
    [SerializeField]
    float jumpForce=16;
    public int jumpCount;
    public int maxJumpCount;
    bool chakJi;
    [SerializeField]
    bool isGround=true;

    //기본공격 범위 설정
    public GameObject attBoxObj;
    BoxCollider2D attBox;

    //플레이어 스텟
    //static public float playerHp;
    public float playerHp;

    //플레이어 무적
    bool invincibleMode = false;
    //플레이어 피격시 색변경 
    bool HitedColor;

    //플레이어 죽음/피격관련
    bool isDead = false;
    bool isDieAnim = false;
    bool ishited = false;
    // Start is called before the first frame update
    void Awake()
    {
       
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        // UnityEngine.Debug.Log("Debug message");
        playerLayer = LayerMask.NameToLayer("Player");
        passableGroundLayer = LayerMask.NameToLayer("PassableGround");        
        playerState = "Idle";
        jumpCount = 0;
        maxJumpCount = 2;
        playerAnimNum = 0;
        


        attBox = gameObject.AddComponent<BoxCollider2D>();
        attBox.size = new Vector2(2f, 1f);
        attBox.isTrigger = true;
        HitedColor = false; 
    }

   
    void Update()
    {
        //UnityEngine.Debug.Log(playerHp);
        if (playerState != "Die")
        {
           
            ishited = true;
          
        }
        if (playerState != "Hited" && playerState != "Die")
        {
            Attack();
            ishited = true;
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
           
            if(isGround==false&&jumpCount!=0)
            {
                CheckGround();            
            }
            
            //플레이어 이동키 값을 받음
            moveHorizontal = Input.GetAxisRaw("Horizontal");

            //플레이어 좌우 반전
            if (moveHorizontal < 0)
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            else if (moveHorizontal > 0)
                this.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (playerState != "GoRight" && playerState != "GoLeft" && playerState != "Jump" && playerState != "ShiftGo" && playerState != "Parrying" && playerState != "Attack" && playerState != "Hited" && playerState != "Die")
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
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.GetDamage(playerAtkPower);
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
            if (attackComboCount == 3)
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
            if (playerState != "Jump")
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
            if(isGround==true)
            {
               
                playerState = "Idle";
                PlayerAnim("Idle");
            }
           
        }
    }
    //대쉬관련
    IEnumerator ShiftGO()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        playerShiftOn = true;
        playerState = "ShiftGo";
        PlayerAnim("ShiftGo");
        
        float speed = playerSpeed;
        playerSpeed = speed * 3;

        yield return new WaitForSeconds(.1f);
        rigid.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

        //애니메이션을 0.1 + 0.1초간 보여준다.
        yield return new WaitForSeconds(.2f);

        playerSpeed = speed;
      
            playerState = "Idle";
            PlayerAnim("Idle");
       

        yield return new WaitForSeconds(1.8f); //재사용 대기시간

        playerShiftOn = false;

    }

    void Jump()
    {
        UnityEngine.Debug.Log(rigid.velocity);
        //아래점프 동작
        if (Input.GetKeyDown(KeyCode.Space)&& Input.GetKey(KeyCode.DownArrow))
        {
            isGround = false;
            playerState = "Jump";
            StartCoroutine(DoJumpDown());
            CheckGround();
            Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, true);                            
        }
        //위 점프 동작
        if(Input.GetKeyDown(KeyCode.Space) &&jumpCount <maxJumpCount)
        {
            isGround = false;
            playerState = "Jump";
            jumpCount += 1;
            StartCoroutine(DoJump());
            CheckGround();
            Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, true);                     
        }
      
    }
    //점프 및 2단점프
    IEnumerator DoJump()
    {
         PlayerAnim("JumpStart");
        
        yield return new WaitForSeconds(.0f);
     
        if (jumpCount == 0)
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
        else if(jumpCount == 1)
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce * 0.66f);

        yield return new WaitForSeconds(0.1f);
        
        yield return new WaitForSeconds(.35f);

        Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, false);
        yield return new WaitForSeconds(.13f);
        PlayerAnim("JumpMiddle");
        
    }
    //아래 점프
    IEnumerator DoJumpDown()
    {
        PlayerAnim("JumpMiddle"); 
        
        StartCoroutine(PlayerJumpEnd());
        //  ResetJumpCount();
        yield return new WaitForSeconds(.2f);
        Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, false);     //벨로시티 y 가 마이너스 면 펄스로 변경 예정             
    }

    
   //레이를 플레이어 아래로 쏴서 점프를 초기화 해주는 함수
    void CheckGround()
    {                
        Vector2 rayPos = new Vector2(this.transform.position.x, this.transform.position.y);
        RaycastHit2D checkGround = Physics2D.Raycast(rayPos, Vector2.down, 0.8f, LayerMask.GetMask("Ground", "PassableGround"));
       
        if (checkGround.collider != null)
        {
                UnityEngine.Debug.DrawRay(rayPos, Vector2.down, Color.red, 1f);
                

                //StartCoroutine(PlayerJumpEnd());
                jumpCount = 0;
                
                            
                if(rigid.velocity.y<0)
                StartCoroutine(PlayerJumpEnd());
                //PlayerAnim("JumpEnd");
        }                        
    }
    IEnumerator PlayerJumpEnd()
    {       
        isGround = true;
        rigid.velocity = new Vector2(0f, 0f);        
        yield return new WaitForSeconds(.1f);      
        playerState = "Idle";
        PlayerAnim("Idle");     
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
        else if (playerDoAnim == "Hited")
            nowAnimNum = 10;
        else if (playerDoAnim == "ShiftGo")
            nowAnimNum = 11;
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
        else if (playerAnimNum == 1) {
            if (jumpCount == 0)
                anim.SetTrigger("RunRight");
        }
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
        else if (playerAnimNum == 10)
            anim.SetTrigger("Hit");
        else if (playerAnimNum == 11)
            anim.SetTrigger("FloorDash");
      
    }
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            //적의 공격력 받아오기
            float damage = enemy.enemyAtkPower;
            //충돌 시 플레이어 피격 애니메이션 시간
            float HitAnimTime = 0.2f;
            if (!invincibleMode)
                StartCoroutine(Hit(damage,HitAnimTime));              
            
        }
    }*/
    //피격 및 데미지 
    
    public void TakeDamage(float damage)
    {
        Debug.Log(playerHp);
        float HitAnimTime = 0.2f;
        if (!invincibleMode)
            StartCoroutine(Hit(damage, HitAnimTime));
    }
    
    
    private IEnumerator Hit(float damage, float AnimTime)
    {
        Debug.Log("플레이어가 데미지를 입었습니다.");
        if (ishited==true)
        {
            float invincibleTime = 0.6f;
            if (playerHp > 0)
            {
                HitedColor = true;
                spriteRenderer.color = Color.red;
                playerState = "Hited";
                PlayerAnim("Hited");
                playerHp -= damage;
                yield return new WaitForSeconds(AnimTime);
                spriteRenderer.color = Color.white;
                HitedColor = false;
                //애니메이션이 끝나면 다시 idle상태로 돌아오게끔     
                playerState = "Idle";
                PlayerAnim("Idle");
                //피격 후 재 피격판정까지 무적 시간   
                StartCoroutine(BeInvincible(invincibleTime));

            }
            else
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
        playerState = "Die";
        PlayerAnim("Die");
       
        UnityEngine.Debug.Log("플레이어가 죽었습니다.");
       
      
        //Invoke("StopScript", 2f);
        //rigid.velocity = new Vector2(transform.position.x, 0); //죽으면 y좌표를 고정하게 하기
        //GetComponent<Player>().enabled = false; //플레이어 키입력 제한하기
        ishited = false;
    }
    void StopScript()
    {
        //플레이어 사망시 스크립트 멈추기
        enabled = false;
        UnityEngine.Debug.Log("스크립트가 종료되었습니다.");
    }
    /*
 void JumpControlloer()
 {
     rigid.velocity=new Vector2(rigid.velocity.x,rigid.velocity.y);
     UnityEngine.Debug.Log(rigid.velocity);
     if (rigid.velocity.y > 0)
     {
         UnityEngine.Debug.Log("점프시작");
         PlayerAnim("JumpStart");
     }
     else if (rigid.velocity.y < 0 )
     {
         UnityEngine.Debug.Log("떨어진다잇");
         PlayerAnim("JumpMiddle");
     }

 }*/
}