using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchBossManager : MonoBehaviour
{
    //탄막스킬 사용시 중앙으로 가기 위해 맵의 위치를 받아오기 위한 오브젝트
    public GameObject center;

    //탄환
    public GameObject Bullet;

    //공격 스킬
    public GameObject ThunderSkill, BlackHoleSkill;

    [SerializeField]
    float hp;

    //발사할 탄막 숫자
    int count = 15;

    //조건에 따라 중앙이동, 플레이어 추적, 일정 위치로 이동하게 하는 변수
    bool goCenter, goPlayer, idleState, moveLeft, moveRight, movedowun;

    //lookPlayer가 true일 때 플레이어를 바라봄
    bool lookPlayer;

    //상하반전을 하게 하는 변수
    bool changeUpDown;
    
    //플레이어 위치를 받을 변수
    Vector2 playerPos;

    Vector3 thunderPosLeft, thenderPosRight;

    SpriteRenderer sprite;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        hp = 450;

        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        sprite.color = Color.white;

        //생각시작
        StartCoroutine(StageStartGoCenter());

        thunderPosLeft = new Vector3(-7.5f, 3, 0);
        thenderPosRight = new Vector3(7.5f, 3, 0);

        lookPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        //각 상태에 따라 이동
        if (goCenter)
            GoCenter();

        if (goPlayer)
            FollowPlayer();

        if (idleState)
            IdleMoving();

        if (moveLeft)
            MoveThunderPosLeft();

        if (moveRight)
            MoveThunderPosRight();

        if (movedowun)
            MoveDown();

        if(lookPlayer)
        {
            //플레이어보다 오른쪽에 위치해 있을 경우 좌우반전 X, 왼쪽에 위치해 있을 경우 좌우반전 실행
            if (GameObject.Find("Player").transform.position.x < transform.position.x)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
                transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        playerPos = GameObject.Find("Player").transform.position;
    }

    //중앙으로 이동
    void GoCenter()
    {
        transform.position = Vector3.MoveTowards(transform.position, center.transform.position, 4f * Time.deltaTime);
    }

    //플레이어 추격
    void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPos, 4f * Time.deltaTime);
    }

    void MoveThunderPosLeft()
    {
        transform.position = Vector3.MoveTowards(transform.position, thunderPosLeft, 6f * Time.deltaTime);
    }

    void MoveThunderPosRight()
    {
        transform.position = Vector3.MoveTowards(transform.position, thenderPosRight, 6f * Time.deltaTime);
    }

    //y축 위치를 낮추어 플레이어가 공격할 수 있게 함
    void IdleMoving()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 1.25f, transform.position.z), 4f * Time.deltaTime);
    }

    void MoveDown()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, .7f, transform.position.z), 3.5f * Time.deltaTime);
    }

    IEnumerator Thinking()
    {
        int randomNum;

        randomNum = Random.Range(1, 12);

        if(hp <= 0) //hp가 0 이하면 죽음
        {
            Die();
        }
        else if (hp < 300 && !changeUpDown && hp >= 150) //hp가 150이상, 300미만일 때, 상하반전 시행
        {
            changeUpDown = true;
            StartCoroutine(ChangeUpDown());
        }
        else if (hp < 150 && changeUpDown && hp > 0) //hp가 0이상, 150미만일 때, 상하반전 해제
        {
            changeUpDown = false;
            StartCoroutine(ChangeUpDown());
        }
        else //공격, idle상태 수행
        {
            switch (randomNum)
            {
                
                case 1:
                case 2:
                case 3:
                    StartCoroutine(Idle());
                    break;
                case 4:
                case 5:
                    StartCoroutine(ThunderLeft());
                    break;
                case 6:
                case 7:
                    StartCoroutine(ThundeRight());
                    break;
                case 8:
                case 9:
                    StartCoroutine(ShotBullet());
                    break;
                case 10:
                case 11:
                    StartCoroutine(BlackHole());
                    break;
                
                
                /*
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    StartCoroutine(ThunderLeft());
                    break;
                */
             
            }
        }
        yield return new WaitForSeconds(1f);
    }

    //탄막패턴
    IEnumerator ShotBullet()
    {
        goCenter = true;
        yield return new WaitForSeconds(2.5f);
        goCenter = false;

        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(Bullet, transform.position, Quaternion.identity);

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / count), Mathf.Sin(Mathf.PI * 2 * i / count));


            clone.GetComponent<Boss2Bullet>().MoveToPositoin(dirVec);
        }

        StartCoroutine(ShotBullet2());
    }

    IEnumerator ShotBullet2()
    {
        count = 20;
        goCenter = true;
        yield return new WaitForSeconds(2f);
        goCenter = false;

        for (int i = 0; i < count; i++)
        {
            GameObject clone = Instantiate(Bullet, transform.position, Quaternion.identity);

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / count), Mathf.Sin(Mathf.PI * 2 * i / count));


            clone.GetComponent<Boss2Bullet>().MoveToPositoin(dirVec);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(Thinking());
        count = 15;
    }

    //왼쪽 번개 스킬
    IEnumerator ThunderLeft()
    {
        lookPlayer = false;
        LookDirection("Left");
        moveLeft = true;
        yield return new WaitForSeconds(2.5f);
        lookPlayer = true;
        moveLeft = false;

        for(int i = 0; i < 6; i++)
        {
            Instantiate(ThunderSkill, new Vector3(transform.position.x + i * 1.5f, 1f, 0), Quaternion.identity);
            yield return new WaitForSeconds(.33f);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(Idle());
    }

    //오른쪽 번개 스킬
    IEnumerator ThundeRight()
    {
        lookPlayer = false;
        LookDirection("Right");
        moveRight = true;
        yield return new WaitForSeconds(2.5f);
        lookPlayer = true;
        moveRight = false;

        for (int i = 0; i < 6; i++)
        {
            Instantiate(ThunderSkill, new Vector3(transform.position.x - i * 1.5f, 1f, 0), Quaternion.identity);
            yield return new WaitForSeconds(.33f);
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(Idle());
    }

    //블랙홀 스킬
    IEnumerator BlackHole()
    {
        goCenter = true;
        yield return new WaitForSeconds(2.5f);
        goCenter = false;

        Instantiate(BlackHoleSkill, transform.position, Quaternion.identity);
        lookPlayer = false;

        yield return new WaitForSeconds(2f);

        lookPlayer = true;
        StartCoroutine(Thinking());
    }

    //화면 전환
    IEnumerator ChangeUpDown()
    {
        if (!changeUpDown)
            CameraMoving.mirrorMod = false;
        else
            CameraMoving.mirrorMod = true;

        yield return new WaitForSeconds(3.5f);

        StartCoroutine(Thinking());
    }

    //Idle상태
    IEnumerator Idle()
    {
        idleState = true;
        yield return new WaitForSeconds(3.5f);
        idleState = false;
        StartCoroutine(Thinking());
    }

    //스테이지 시작시 센터로 이동
    IEnumerator StageStartGoCenter()
    {
        goCenter = true;

        yield return new WaitForSeconds(3f);

        goCenter = false;

        yield return new WaitForSeconds(1f);
        StartCoroutine(Thinking());
    }

    IEnumerator Die()
    {
        movedowun = true;
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("Die");

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    public void GetDamage(float damage)
    {
        StartCoroutine(ChangeEnemyColor());
        hp -= damage;
    }

    IEnumerator ChangeEnemyColor()
    {
        sprite.color = new Color(1, 0.2f, 0.2f, 1);
        yield return new WaitForSeconds(.33f);
        sprite.color = new Color(1, 1, 1, 1);
    }

    void LookDirection(string direction)
    {
        if(direction == "Right")
            transform.localEulerAngles = new Vector3(0, 0, 0);
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);
    }

}
