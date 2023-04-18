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
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }


    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;


        Vector2 dirVec = target.position - rigid.position;  //타겟의 방향이 나옴
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // 부딪치면 안밀려나게 

    }


    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        spriter.flipX = target.position.x < rigid.position.x; // 타겟의 위치가 자신의 x 값보다 작으면 
    }

    private void OnEnable() // 스크립트가 활성화 될 때 호출하는 함수
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // 게임매니저에 만들어 놓은 인스턴스를 호출해서 플레이어 자체 호출
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true; //물리를 시뮬레이션 할건지 안 할건지
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);

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
        if (!collision.CompareTag("Bullet") || !isLive) // 몬스터에 타겟된것이 불렛이 아니라면 끔 , 살아있을 때만 
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack()); //StartCoroutine("KnockBack");

        if (health>0)
        {
            // isLive, Hit Action
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            // Die
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false; //물리를 시뮬레이션 할건지 안 할건지
            spriter.sortingOrder = 1; // 스프라이트 우선순위 낮춤
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if(GameManager.instance.isLive)
            {
               AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }

           

        }
    }

    IEnumerator KnockBack() //코루틴
    {
        yield return wait; // 다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; // 플레이어의 반대방향으로 밀려나게 뺌
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
       
       //yield return new WaitForSeconds(2f); //2초 쉬기
    }


    void Dead()
    {
        gameObject.SetActive(false);
    }
}
