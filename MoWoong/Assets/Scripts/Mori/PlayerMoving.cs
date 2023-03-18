using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float playerSpeed = 5;
    //플레이어 상태(점프는 없음)
    [SerializeField]
    string playerState;  // Attack, SkillAttack, Idle, GoLeft, GoRight, Hited, Die
    //회피
    bool playerShiftOn;

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
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerState = "Idle";
        jumpTIme = 0;
        doJump = false;

        attBox = gameObject.AddComponent<BoxCollider2D>();
        attBox.size = new Vector2(2f, 1f);
        attBox.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

        PlayerMove();

        if (Input.GetKey("f") || Input.GetMouseButton(0) && playerState != "Attack")
        {
            StartCoroutine(PlayerAtt());
        }

        if (playerState == "GoLeft")
            this.transform.localEulerAngles = new Vector3(0, 0, 0);
        if (playerState == "GoRight")
            this.transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    void FixedUpdate()
    {

    }

    void PlayerMove()
    {
        Vector2 leftPos = new Vector2(transform.position.x - 1, transform.position.y);
        Vector2 rightPos = new Vector2(transform.position.x + 1, transform.position.y);


        if (Input.GetKey("left") || Input.GetKey("a") && playerState == "Idle")
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, leftPos, playerSpeed * Time.deltaTime);
            playerState = "GoLeft";
            if (Input.GetKeyDown("left shift") && playerState != "GoRight" && playerState != "GoDown" && !playerShiftOn)
            {
                StartCoroutine(ShiftGO());
            }
        }
        else if (Input.GetKey("right") || Input.GetKey("d") && playerState == "Idle")
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, rightPos, playerSpeed * Time.deltaTime);
            playerState = "GoRight";
            if (Input.GetKeyDown("left shift") && playerState != "GoLeft" && playerState != "GoDown" && !playerShiftOn)
            {
                StartCoroutine(ShiftGO());
            }
        }
        else if (playerState != "Attack" && playerState != "SkillAttack")
        {
            playerState = "Idle";
        }

        Jump();
    }

    IEnumerator ShiftGO()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        playerShiftOn = true;
        playerState = "ShiftGo";

        float speed = playerSpeed;
        playerSpeed = speed * 5;

        yield return new WaitForSeconds(.1f);

        playerSpeed = speed;
        playerState = "Idle";

        yield return new WaitForSeconds(.1f);
        rigid.constraints &= ~RigidbodyConstraints2D.FreezePositionY;

        yield return new WaitForSeconds(.8f);
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

    IEnumerator PlayerAtt()
    {
        playerState = "Attack";

        Collider2D[] EnemyCollider = Physics2D.OverlapBoxAll(attBoxObj.transform.position, attBox.size, 0f);

        foreach (Collider2D collider in EnemyCollider)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().GetDamage();

            }
        }

        yield return new WaitForSeconds(.66f);
        playerState = "Idle";
    }

}
