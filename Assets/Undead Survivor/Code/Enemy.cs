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


        Vector2 dirVec = target.position - rigid.position;  //Ÿ���� ������ ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // �ε�ġ�� �ȹз����� 

    }


    private void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        spriter.flipX = target.position.x < rigid.position.x; // Ÿ���� ��ġ�� �ڽ��� x ������ ������ 
    }

    private void OnEnable() // ��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ���ϴ� �Լ�
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // ���ӸŴ����� ����� ���� �ν��Ͻ��� ȣ���ؼ� �÷��̾� ��ü ȣ��
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true; //������ �ùķ��̼� �Ұ��� �� �Ұ���
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
        if (!collision.CompareTag("Bullet") || !isLive) // ���Ϳ� Ÿ�ٵȰ��� �ҷ��� �ƴ϶�� �� , ������� ���� 
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
            rigid.simulated = false; //������ �ùķ��̼� �Ұ��� �� �Ұ���
            spriter.sortingOrder = 1; // ��������Ʈ �켱���� ����
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            if(GameManager.instance.isLive)
            {
               AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }

           

        }
    }

    IEnumerator KnockBack() //�ڷ�ƾ
    {
        yield return wait; // ���� �ϳ��� ���� ������ ������
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos; // �÷��̾��� �ݴ�������� �з����� ��
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
       
       //yield return new WaitForSeconds(2f); //2�� ����
    }


    void Dead()
    {
        gameObject.SetActive(false);
    }
}
