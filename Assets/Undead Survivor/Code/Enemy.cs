using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;

    SpriteRenderer spriter;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        if (!isLive)
            return;


        Vector2 dirVec = target.position - rigid.position;  //Ÿ���� ������ ����
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero; // �ε�ġ�� �ȹз����� 

    }


    private void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x; // Ÿ���� ��ġ�� �ڽ��� x ������ ������ 
    }

    private void OnEnable() // ��ũ��Ʈ�� Ȱ��ȭ �� �� ȣ���ϴ� �Լ�
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>(); // ���ӸŴ����� ����� ���� �ν��Ͻ��� ȣ���ؼ� �÷��̾� ��ü ȣ��
    }
}
