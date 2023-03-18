using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePort : MonoBehaviour
{
    public GameObject teleportPoint;
    public bool moveNextMap, movePreivousMep;

    private void OnTriggerEnter2D(Collider2D col)
    {
        //�÷��̾�� �浹�ϸ� ����� �ڷ���Ʈ ��ġ�� �÷��̾ �̵�
        if (col.tag == "Player" && !Map.moveMap)
        {
            StartCoroutine(PlayerMoveMap(col));
        }
    }

    IEnumerator PlayerMoveMap(Collider2D col)
    {
        Map.moveMap = true;
        if (moveNextMap)
        {
            //Debug.Log("MoveNext");
            Vector2 goalPoint = new Vector2(teleportPoint.transform.position.x + 1.5f, teleportPoint.transform.position.y);
            Map.mapNum += 1;
            col.transform.position = goalPoint;
        }
        if (movePreivousMep)
        {
            //Debug.Log("MovePre");
            Vector2 goalPoint = new Vector2(teleportPoint.transform.position.x - 1.5f, teleportPoint.transform.position.y);
            Map.mapNum -= 1;
            col.transform.position = goalPoint;
        }
        yield return new WaitForSeconds(.33f);
        Map.moveMap = false;
    }


}
