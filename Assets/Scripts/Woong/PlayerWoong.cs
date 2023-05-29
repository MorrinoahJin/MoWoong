using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using UnityEngine.UI;


public class PlayerWoong : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image UI_Parry;
    [SerializeField] private Image UI_Buff_atk;
    
    [SerializeField] float playerSpeed = 5;
    int playerLayer, passableGroundLayer;
    //유저 컨트롤 제어용 
    static public bool canControl;

  
    Vector2 atkSize;
        
    //플레이어 상태(점프는 없음)
    [SerializeField]
    string playerState;  // Attack, SkillAttack, Idle, GoLeft, GoRight, Hited, Die
    int playerAnimNum;
    //회피
    bool playerShiftOn;
    float moveHorizontal;

    //공격 및 공격 콤보
    bool canAtk;
    public float playerAtkPower = 10;
    int attackComboCount = 0;
    bool doNextAttack, checkAttack;

    //점프
    Rigidbody2D rigid;
    [SerializeField] float jumpForce = 21;
    public int jumpCount;
    public int maxJumpCount;
    bool chakJi;
    [SerializeField] bool isGround = true;
    //소리설정
    public AudioSource audioSource;
    public AudioClip atkClip;
    public AudioClip jumpClip;
   
    //기본공격 범위 설정
    public GameObject attBoxObj;
    BoxCollider2D attBox;
    
    //플레이어 휴식
    GameObject bonFire;
    bool isFireEnter;
    bool isRest;

    //플레이어 스텟
    //static public float playerHp;
    [SerializeField]
    static public float playerHp;
    [SerializeField]
    static public float playerMaxHp=100;

    //플레이어 패링
  
    [SerializeField] bool canParrying = true; //패링 시작이 가능한 상태인지 체크(쿨타임 관련)
    [SerializeField] bool checkHited = false; //적에게 맞았는지 체크
    bool ParryingOn = false;  //패링 중인지 체크

    //플레이어 무적
    bool invincibleMode = false;
    //플레이어 피격시 색변경 
    bool HitedColor;

    //플레이어 죽음/피격관련
    bool isKnockBack = false;
    bool isDead = false;
    bool isDieAnim = false;
    bool ishited = false;

    //플레이어 공 버프
    bool isBuff_atk = false;
    // Start is called before the first frame update
    void Awake()
    {

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

    }
    void Start()
    {
        canControl = true;
        // UnityEngine.Debug.Log("Debug message");
        playerLayer = LayerMask.NameToLayer("Player");
        passableGroundLayer = LayerMask.NameToLayer("PassableGround");
        
        bonFire = GameObject.FindGameObjectWithTag("BonFire");
        isFireEnter = false;
        isRest = false;

        playerState = "Idle";
        
        jumpCount = 0;
        maxJumpCount = 2;
        playerAnimNum = 0;
        
        playerHp = playerMaxHp;
        hpBar.value = (float)playerHp / (float)playerMaxHp;
        
        atkSize= new Vector2(2f, 1f);
        
        canAtk = true;

    }
    void HpBar()
    {
        hpBar.value = (float)playerHp / (float)playerMaxHp;
    }
   
    void Update()
    {
        
        if (playerHp > playerMaxHp)
        {
            playerHp = playerMaxHp;
        }
        Vector2 rayPos = new Vector2(this.transform.position.x, this.transform.position.y);
        RaycastHit2D checkGround = Physics2D.Raycast(rayPos, Vector2.down, 0.8f, LayerMask.GetMask("Ground", "PassableGround"));
        //Debug.DrawRay(rayPos, Vector2.down, Color.red, 0.8f);
        if(playerHp < 0){
            die();
        }
        HpBar();
        if (playerState != "Die")
        {

            ishited = true;

        }
        if (playerState != "Hited" && playerState != "Die")
        {
            if (isGround == true) 
            {
                Sit();
                Attack();
                Interation();
            }

            ishited = true;
            //콤보 공격 체크
            if (checkAttack)
            {
                if ((Input.GetKey("f") || Input.GetMouseButton(0))&&canControl)
                    doNextAttack = true;
            }
        }
        if (playerState != "Attack" && playerState != "Hited" && playerState != "Die")     
        {
            //패링함수
            Parry();
            Buff_atk();
        }
        if (playerState != "Parrying" && playerState != "Attack" && playerState != "Hited" && playerState != "Die")
        {
            Jump();
            Move();
            if (checkGround.collider != null && jumpCount != 0)
            {
                CheckGround();
            }


            //플레이어 이동키 값을 받음
            moveHorizontal = Input.GetAxisRaw("Horizontal");

            //플레이어 좌우 반전
            if (moveHorizontal < 0 && canControl)
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            else if (moveHorizontal > 0 && canControl)
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
     if (isGround == true && (Input.GetKey("f") && playerState != "Attack") && canAtk && canControl)
        {
            StartCoroutine(PlayerAttack());
            StartCoroutine(CanAtk());
        }                
    }

    //애니메이션 이벤트 함수호출
    void checkAtk()
    {
        UnityEngine.Debug.Log("이벤트 공격 호출");
        Collider2D[] EnemyCollider = Physics2D.OverlapBoxAll(attBoxObj.transform.position, atkSize, 0f);
        
        foreach (Collider2D collider in EnemyCollider)
        {
            if (collider.CompareTag("Enemy"))
            {
                //공격코드
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.GetDamage(playerAtkPower);
            }
            if (collider.CompareTag("TutorialBoss"))
            {
                //공격코드
                collider.GetComponent<TutorialBoss>().GetDamage(playerAtkPower);
            }
           
        }
    }
   
    IEnumerator PlayerAttack()
    {
        checkAttack = false;
        doNextAttack = false;
        playerState = "Attack";



        attackComboCount += 1;

        //공격콤보에 따라 다른 애니메이션 실행
        if (attackComboCount == 1)
        {
            PlayerAnim("Attack1");
            audioSource.PlayOneShot(atkClip);
        }
        else if (attackComboCount == 2)
        {
            PlayerAnim("Attack2");
            audioSource.PlayOneShot(atkClip);
        }
        else
        {
            PlayerAnim("Attack3");
            audioSource.PlayOneShot(atkClip);
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

            if (Input.GetKeyDown("left shift") && !playerShiftOn && canControl)
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
            if (Input.GetKeyDown("left shift") && !playerShiftOn && canControl)
            {
                StartCoroutine(ShiftGO());
            }
        }
        else
        {
            if (isGround == true)
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
        StartCoroutine(BeInvincible(0.5f));
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
   
    void Interation()
    {
        Vector2 size = new Vector2(1.0f, 1.0f);
        Collider2D[] interationCollider = Physics2D.OverlapBoxAll(transform.position, size, 0);

        foreach (Collider2D collider in interationCollider)
        {
            /*
            if (collider.CompareTag("BonFire"))
            {
                //공격코드
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.GetDamage(playerAtkPower);
            }
            */
            /*
            if (collider.CompareTag("Box")&& Input.GetKeyDown("z"))
            {
                //공격코드
                collider.GetComponent<Box>().Open();
            }
            */
        }

    }
    
    void Sit()
    {
        if (isFireEnter && Input.GetKeyDown("e"))
        {
            if (!isRest)
            {
                StartCoroutine(RestCool());
                playerState = "Rest";
                PlayerAnim("Sit");
            }
        }     
    }
    IEnumerator RestCool()
    {
        isRest = true;
        yield return new WaitForSeconds(3f);
        isRest = false;
    }
    void Rest()
    {
        PlayerAnim("Rest");

    }
    void hpFill()
    {
        UnityEngine.Debug.Log(playerHp);
        if(playerHp < playerMaxHp) {
            playerHp += playerMaxHp / 10;
        }
     
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == bonFire)
        {
            //UnityEngine.Debug.Log("불 접근");
            isFireEnter = true;
           
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == bonFire)
        {
            //UnityEngine.Debug.Log("불 접근X");
            isFireEnter = false;
            
        }
    }
    void Jump()
    {
        //UnityEngine.Debug.Log(rigid.velocity);
        //아래점프 동작
        if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.DownArrow))
        {
            isGround = false;
            playerState = "Jump"; 
            StartCoroutine(DoJumpDown());
            Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, true);
        }
        //위 점프 동작
        else if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount && canControl)
        {
            
            isGround = false;
            playerState = "Jump";
            StartCoroutine(DoJump());
 
            Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, true);
        }

    }
    //점프 및 2단점프
    IEnumerator DoJump()
    {
        PlayerAnim("JumpStart");
        audioSource.PlayOneShot(jumpClip);
        yield return new WaitForSeconds(.0f);

        if (jumpCount == 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce);
            jumpCount += 1;
            yield return new WaitForSeconds(0.1f);
           // Debug.Log("1단점프 내려가는 애니메이션 실행");
            PlayerAnim("JumpMiddle");
            yield return new WaitForSeconds(.23f);
            Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, false);          
        }
        else if (jumpCount != 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpForce * 0.66f);
            jumpCount += 1;
            yield return new WaitForSeconds(0.1f);

            yield return new WaitForSeconds(.35f);
            Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, false);
            yield return new WaitForSeconds(.13f);
            PlayerAnim("JumpMiddle");
        }
    }
    //아래 점프
    IEnumerator DoJumpDown()
    {

        UnityEngine.Debug.Log("발판 무시");
        PlayerAnim("JumpMiddle");
        audioSource.PlayOneShot(jumpClip);
        StartCoroutine(PlayerJumpEnd());
        //  ResetJumpCount();
        yield return new WaitForSeconds(.3f);
        Physics2D.IgnoreLayerCollision(playerLayer, passableGroundLayer, false);     //벨로시티 y 가 마이너스 면 펄스로 변경 예정             
    }


    //레이를 플레이어 아래로 쏴서 점프를 초기화 해주는 함수
    void CheckGround()
    {
        if (rigid.velocity.y < 0)
        {
            //PlayerAnim("JumpMiddle");
            jumpCount = 0;
           // Debug.Log("아래로내려갑니다.");

            StartCoroutine(PlayerJumpEnd());
            //PlayerAnim("JumpEnd");
        }
    }
   
    void ConvertIdleAnim()
    {
        UnityEngine.Debug.Log("아이들");
        playerState = "Idle";
        PlayerAnim("idle");
    }
    void Buff_atk()
    {
        if (Input.GetKeyDown("v") && !isBuff_atk && canControl )
        {
            isBuff_atk = true;
            StartCoroutine(Buff_atkCoolDown(5.0f));
            StartCoroutine(UI_Buff_atkCoolDown(8.0f));      
        }           
    }
 
    IEnumerator UI_Buff_atkCoolDown(float cool)
    {
        float startTime = Time.time;
        while (Time.time < startTime + cool)
        {
            float timeLeft = startTime + cool - Time.time;
            UI_Buff_atk.fillAmount = timeLeft / cool;
            yield return null;
        }
        UI_Buff_atk.fillAmount = 1f;
    }

    IEnumerator Buff_atkCoolDown(float cool)
    {
       
        playerState = "Buff_atk";
        PlayerAnim("Buff_atk");
        playerAtkPower = playerAtkPower * 2;
        UnityEngine.Debug.Log("공버프 쿨타임");
        yield return new WaitForSeconds(cool);
        playerAtkPower = playerAtkPower % 2;
        UnityEngine.Debug.Log("공버프 쿨 초기화");
        yield return new WaitForSeconds(3f);
        isBuff_atk = false;
    }
    void Parry()
    {
        if(Input.GetKey("g")&&canParrying && canControl){
           
           // UnityEngine.Debug.Log("패링시작");
            playerState = "Parrying";
            PlayerAnim("Parrying");  
            /*
            if (checkHited == true)
            {
                canParrying = false;
                //UnityEngine.Debug.Log("패링");
                PlayerAnim("ParryingSuccess");
                //ConvertIdleAnim();
                StartCoroutine(ParryingCoolDown(3.0f));
                StartCoroutine(UI_ParryingCoolDown(3.0f));
            }
            */
        }
        else if (Input.GetKeyUp("g") && canParrying && canControl)
        {
            //패링 종료
            canParrying = false;
            //UnityEngine.Debug.Log("패링종료");
            ConvertIdleAnim();
            StartCoroutine(ParryingCoolDown(3.0f));
            StartCoroutine(UI_ParryingCoolDown(3.0f));
        }
    }
    void parryingSuccess()
    {

    }
    
    IEnumerator UI_ParryingCoolDown(float cool)
    {
        float startTime = Time.time;
        while (Time.time < startTime + cool)
        {
            float timeLeft = startTime + cool - Time.time;
            UI_Parry.fillAmount = timeLeft / cool;
            yield return null;
        }
        UI_Parry.fillAmount = 1f;
    }

    //패링 쿨타임을 체크하는 코루틴
    IEnumerator ParryingCoolDown(float cool)
    {
        //UnityEngine.Debug.Log("패링쿨타임");
        yield return new WaitForSeconds(cool);

       //UnityEngine.Debug.Log("패링쿨초기화");
        canParrying = true;
        checkHited = false;
    }
  

    IEnumerator PlayerJumpEnd()
    {


        rigid.velocity = new Vector2(0f, 0f);
        yield return new WaitForSeconds(.1f);
        playerState = "Idle";
        PlayerAnim("Idle");
        isGround = true;
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
        else if (playerDoAnim == "AirAttack1")
            nowAnimNum = 6;
        else if (playerDoAnim == "AirAttack2")
            nowAnimNum = 7;
        //점프
        else if (playerDoAnim == "JumpStart")
            nowAnimNum = 8;
        else if (playerDoAnim == "JumpMiddle")
            nowAnimNum = 9;
        else if (playerDoAnim == "JumpEnd")
            nowAnimNum = 10;
        else if (playerDoAnim == "Die")
            nowAnimNum = 11;
        else if (playerDoAnim == "Hited")
            nowAnimNum = 12;
        else if (playerDoAnim == "ShiftGo")
            nowAnimNum = 13;
        //패링
        else if (playerDoAnim == "Parrying")
            nowAnimNum = 14;
        else if (playerDoAnim == "ParryingSuccess")
            nowAnimNum = 15;
        else if (playerDoAnim == "Buff_atk")
            nowAnimNum = 16;
        else if (playerDoAnim == "Sit")
            nowAnimNum = 17;
        else if (playerDoAnim == "Rest")
            nowAnimNum = 18;

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
        {
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
        else if (playerAnimNum == 6)
            anim.SetTrigger("AirAttack1");
        else if (playerAnimNum == 7)
            anim.SetTrigger("AirAttack2");
        //점프
        else if (playerAnimNum == 8)
            anim.SetTrigger("JumpStart");
        else if (playerAnimNum == 9)
            anim.SetTrigger("JumpMiddle");
        else if (playerAnimNum == 10)
            anim.SetTrigger("JumpEnd");
        else if (playerAnimNum == 11)
            anim.SetTrigger("Die");
        else if (playerAnimNum == 12)
            anim.SetTrigger("Hit");
        else if (playerAnimNum == 13)
            anim.SetTrigger("FloorDash");
        //패링
        else if (playerAnimNum == 14)
            anim.SetTrigger("Parrying");
        else if (playerAnimNum == 15)
            anim.SetTrigger("ParryingSuccess");
        //공벞
        else if (playerAnimNum == 16)
            anim.SetTrigger("Buff_atk");

        else if (playerAnimNum == 17)
            anim.SetTrigger("Sit");
        else if (playerAnimNum == 18)
            anim.SetTrigger("Rest");

    }
    //피격 및 데미지 

    public void TakeDamage(float damage, Vector3 pos)
    {
        checkHited = true;
        if (playerState != "Parrying"&&!isDieAnim)
        {
            //UnityEngine.Debug.Log(playerHp);
            //공격 제한
            StartCoroutine(CanAtk());
            float hitAnimTime = 0.2f;
            float knockBackDirection = transform.position.x - pos.x;
            if (knockBackDirection < 0)
                knockBackDirection = 1;
            else
                knockBackDirection = -1;
            if (!isKnockBack)
                StartCoroutine(KnockBack(knockBackDirection));
            if (!invincibleMode)
                StartCoroutine(Hit(damage, hitAnimTime));
        }
        else if (playerState == "Parrying")
        {
            canParrying = false;
            //UnityEngine.Debug.Log("패링");
            PlayerAnim("ParryingSuccess");
            //ConvertIdleAnim();
            StartCoroutine(ParryingCoolDown(3.0f));
            StartCoroutine(UI_ParryingCoolDown(3.0f));
        }
    }
    private IEnumerator CanAtk()
    {
        canAtk = false;
        yield return new WaitForSeconds(0.2f);
        canAtk = true;
    }

    private IEnumerator KnockBack(float dir)
    {
        isKnockBack = true;
        //UnityEngine.Debug.Log("넉백");
        // 플레이어가 오른쪽을 보고 오른쪽에서 공격 받은 경우
        if (dir==1&& transform.localEulerAngles.y == 0)
        {
            transform.Translate(Vector2.left * playerSpeed *0.1f);
        }
        // 플레이어가 왼쪽을 보고 오른쪽에서 공격 받은 경우
        else if (dir == 1 && transform.localEulerAngles.y == 180)
        {
            transform.Translate(Vector2.right * playerSpeed * .1f);
        }
        // 플레이어가 왼쪽을 보고 왼쪽에서 공격 받은 경우
        else if (dir == -1 && transform.localEulerAngles.y == 0)
        {
            transform.Translate(Vector2.right * playerSpeed * .1f);
        }
        // 플레이어가 오른쪽을 보고 왼쪽에서 공격 받은 경우
        else if (dir==-1 && transform.localEulerAngles.y == 180)
        {
            transform.Translate(Vector2.left * playerSpeed * .1f );
        }
            

        yield return new WaitForSeconds(0.2f);

        isKnockBack = false;
    }

   

    private IEnumerator Hit(float damage, float AnimTime)
    {
       // Debug.Log("플레이어가 데미지를 입었습니다.");
        if (ishited == true)
        {
            playerHp -= damage;
            float invincibleTime = 0.6f;
            if (playerHp > 0)
                {
                HitedColor = true;
                spriteRenderer.color = new Color(1, 1, 1, 0.5f);
                //spriteRenderer.color = Color.red;
                playerState = "Hited";
                PlayerAnim("Hited");
                yield return new WaitForSeconds(AnimTime);
                spriteRenderer.color = new Color(1, 1, 1, 1);
                //spriteRenderer.color = Color.white;
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
    public void hpzero()
    {
        playerHp = 0;
        UnityEngine.Debug.Log("PlayerHp Zero.");
        die();
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
        isKnockBack = true;
        ishited = false;
        isDieAnim = true;
        playerState = "Die";
        PlayerAnim("Die");
        canControl = false;
        //UnityEngine.Debug.Log("플레이어가 죽었습니다.");
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;


        //Invoke("StopScript", 2f);
        //rigid.velocity = new Vector2(transform.position.x, 0); //죽으면 y좌표를 고정하게 하기
        //GetComponent<Player>().enabled = false; //플레이어 키입력 제한하기
      
    }
    void StopScript()
    {
        //플레이어 사망시 스크립트 멈추기
        enabled = false;
        
    }
    void MirrorImage()
    {
        // GameObject PlayerMirror = Instantiate();
    }
}