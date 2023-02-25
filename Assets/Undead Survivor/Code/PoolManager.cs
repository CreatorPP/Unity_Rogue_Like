using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; // ������ ������ ����

    List<GameObject>[] pools; // Ǯ ����ϴ� ����Ʈ


    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; // ����Ʈ�� ũ��� �����տ� ���� ũ�⸸ŭ �迭 �ʱ�ȭ��

        for(int index= 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>(); // pools�� �ִ� ����Ʈ�� �ϳ��ϳ� �ʱ�ȭ��
        }

        

    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���ӿ�����Ʈ ������
            

        foreach (GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                // ... �߰��ϸ� select ������ �Ҵ�

                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ...�� ã������?
            

        if(!select) //select �� null�̸�
        {
            // ... ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(prefabs[index], transform); //���� ������Ʈ�� �����ؼ� ��鿡 �����ϴ� �Լ�
            pools[index].Add(select);
        }


        return select;
    }
}
