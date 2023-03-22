using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
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
    
    float attackInputTime = 0f;
    int attackComboCount = 0;


    //점프
    Rigidbody2D rigid;
    float jumpForce = 27;
    [SerializeField]
    int jumpTIme;
    bool doJump;

    //기본공격 범위 설정
    public GameObject attBoxObj;
    BoxCollider2D attBox;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
       
    }
    void Start()
    {
        UnityEngine.Debug.Log("Debug message");

        rigid = GetComponent<Rigidbody2D>();
        playerState = "Idle";
        jumpTIme = 0;
        playerAnimNum = 0;
        doJump = false;

        attBox = gameObject.AddComponent<BoxCollider2D>();
        attBox.size = new Vector2(2f, 1f);
        attBox.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        PlayerMove();


        if ((Input.GetKey("f") || Input.GetMouseButton(0)) && playerState != "Attack") {

            float timeSinceLastAttack = Time.time - attackInputTime;
            if (timeSinceLastAttack <= 1.0f && attackComboCount < 3)
                attackComboCount++;
            else
                attackComboCount = 1;
            attackInputTime = Time.time;
            StartCoroutine(PlayerAtt(attackComboCount));
        }             

        if (playerState == "GoLeft")
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        if (playerState == "GoRight")
            this.transform.localEulerAngles = new Vector3(0, 180, 0);
    }


    void FixedUpdate()
    {
        //플레이어 이동
        Vector3 move = new Vector3(moveHorizontal, 0f, 0f);
        if (playerState != "Attack")
        transform.position += move * playerSpeed * Time.deltaTime;
    }

    void PlayerMove()
    {
       
        //플레이어 상태에 따른 이동 변경
        if (moveHorizontal < 0 && playerState != "GoRight" && playerState != "GoDown" && playerState != "Attack")
        {
            PlayerAnim("Move");
            playerState = "GoLeft";
            if (Input.GetKeyDown("left shift") && !playerShiftOn)
            {
                StartCoroutine(ShiftGO());
            }
        }
        else if (moveHorizontal > 0 && playerState != "GoLeft" && playerState != "GoDown"&& playerState != "Attack")
        {
            PlayerAnim("Move");
            playerState = "GoRight";
            if (Input.GetKeyDown("left shift") && !playerShiftOn)
            {
                StartCoroutine(ShiftGO());
            }
        }
        else if (playerState != "Attack" && playerState != "SkillAttack")
        {
            playerState = "Idle";
            PlayerAnim("Idle");
        }

        Jump();
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
        
        yield return new WaitForSeconds(2.8f); //재사용 대기시간
       
        playerShiftOn = false;

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpTIme < 2 && !doJump && playerState != "Die" && playerState != "Hited")
        {
            jumpTIme += 1;
            StartCoroutine(DoJump());
        }
    }

    IEnumerator DoJump()
    {
        doJump = true;

        if (jumpTIme == 0)
            rigid.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        else
            rigid.AddForce(new Vector2(0f, (jumpForce * .66f)), ForceMode2D.Impulse);

        yield return new WaitForSeconds(.33f);
        doJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Wall" && jumpTIme != 0)
        {
            jumpTIme = 0;
            //Debug.Log("chakji");
        }
    }

    IEnumerator PlayerAtt(int count)
    {
        playerState = "Attack";
        PlayerAnim("Attack");     
        UnityEngine.Debug.Log("Attack Combo "+count);

        Collider2D[] EnemyCollider = Physics2D.OverlapBoxAll(attBoxObj.transform.position, attBox.size, 0f);

        foreach (Collider2D collider in EnemyCollider)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().GetDamage();
                
            }
        }
        yield return new WaitForSeconds(.4f);
        playerState = "Idle";
    }
    
    
    void PlayerAnim(string playerDoAnim)
    {
          int nowAnimNum = 0;
          if (playerDoAnim == "Idle")
                nowAnimNum = 0;
          else if (playerDoAnim == "Move")
                nowAnimNum = 1;
          else if (playerDoAnim == "Jump")
                nowAnimNum = 2;
          else if (playerDoAnim == "Attack")
                nowAnimNum = 3;

          if (nowAnimNum != playerAnimNum)
          {
           playerAnimNum = nowAnimNum;
           PlayerAnimChange();
          }
    }
    void PlayerAnimChange()
    {   
        if (playerAnimNum == 0 )
            anim.SetTrigger("Idle");
        else if (playerAnimNum == 1)
            anim.SetTrigger("isRunning");
        else if (playerAnimNum == 3)
            anim.SetTrigger("isAttack");            
    }
}
