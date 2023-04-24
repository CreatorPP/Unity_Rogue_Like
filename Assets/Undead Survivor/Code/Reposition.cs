using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(!collision.CompareTag("Area"))   // 콜라이더랑 충돌한 아레아 태그 범위 벗어나면 
        {
            return;
        }

        Vector3 playerPos = GameManager.instance.player.transform.position;  // 플레이어 위치값 저장
        Vector3 myPos = transform.position; // 이 스크립트를 가진 위치값 저장
        

        switch (transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;


                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;

                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);


                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy":
                if(coll.enabled)
                {
                    Vector3 dist = playerPos - myPos; // 플레이어와 몬스터의 포지션값 빼기
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3),0);
                    transform.Translate(ran + dist * 2);
                }
                break;

        }
    }
}
