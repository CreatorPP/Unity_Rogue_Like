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

    private void Start()
    {
        Init();
    }

    void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);  //-150과 forward 해야 시계방향 150 back 하면 시계 반대방향
                break;
            default:
                break;

        }

        // test
        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }


    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
    }


    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = -150; // -는 시계 방향으로 돌게함   
                Batch();
                break;
            default:
                break;

        }

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

            bullet.GetComponent<Bullet>().Init(damage, -1); // -1은 무한으로 관통
        }
    }
}
