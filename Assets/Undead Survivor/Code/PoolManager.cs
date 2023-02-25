using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; // 프리팹 보관할 변수

    List<GameObject>[] pools; // 풀 담당하는 리스트


    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length]; // 리스트의 크기는 프리팹에 넣은 크기만큼 배열 초기화함

        for(int index= 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>(); // pools에 있는 리스트도 하나하나 초기화함
        }

        

    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고(비활성화 된) 있는 게임오브젝트 접근함
            

        foreach (GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                // ... 발견하면 select 변수에 할당

                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ...못 찾았으면?
            

        if(!select) //select 가 null이면
        {
            // ... 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform); //원본 오브젝트를 복제해서 장면에 생성하는 함수
            pools[index].Add(select);
        }


        return select;
    }
}
