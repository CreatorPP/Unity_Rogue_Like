using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriter;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        if (!isLive)
            return;


        Vector2 dirVec = target.position - rigid.position;  //타겟의 방향이 나옴
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 부딪치면 안밀려나게 

    }


    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // 타겟의 위치가 자신의 x 값보다 작으면 
    }

    private void OnEnable() // 스크립트가 활성화 될 때 호출하는 함수
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // 게임매니저에 만들어 놓은 인스턴스를 호출해서 플레이어 자체 호출
        isLive = true;
        health = maxHealth;

    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) // 몬스터에 타겟된것이 불렛이 아니라면 끔
        {
            return;
        }
        health -= collision.GetComponent<Bullet>().damage;

        if(health>0)
        {
            // isLive, Hit Action
        }
        else
        {
            // Die
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
