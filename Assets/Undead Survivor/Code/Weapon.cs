using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }


    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);  //-150과 forward 해야 시계방향 150 back 하면 시계 반대방향
                break;
            default:
                timer += Time.deltaTime;

                if(timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;

        }

        // test
        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(10, 1);
        }
    }


    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }


    public void Init(ItemData data)
    {

        //Basic Set
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;// 지역 위치인 localPos 을 원점으로 변경


        // Property Set
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;


        // 스크립트블 오브젝트의 독립성을 위해서 인덱스가 아닌 프리펩으로 설정
        for(int index=0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150; // -는 시계 방향으로 돌게함   
                Batch();
                break;
            default:
                speed = 0.3f;
                break;

        }

        //Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void Batch()
    {
        for(int index=0; index < count; index++)
        {
            Transform bullet;
            if(index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform; // parent = 부모변경
            }

            

            bullet.localPosition = Vector3.zero; // 월드 위치값 초기화 플레이어한테 소환
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1은 무한으로 관통
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

    }

}
