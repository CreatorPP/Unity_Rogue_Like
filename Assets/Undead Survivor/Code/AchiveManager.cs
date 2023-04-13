using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//업적 관리 코드
public class AchiveManager : MonoBehaviour
{

  

    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achive { UnlockPotato, UnlockBean }
    Achive[] achives;

    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(4);

        if (!PlayerPrefs.HasKey("MyData")) // MyData가 없으면 초기화 해달라
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);


        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int index=0; index < lockCharacter.Length; index++)
        {
            string achiveName = achives[index].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacter[index].SetActive(!isUnlock);
            unlockCharacter[index].SetActive(isUnlock);

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        foreach(Achive achive in achives)
        {
            CheckAchive(achive);
        }
    }

    void CheckAchive(Achive achive) //업적달성 함수
    {
        bool isAchive = false;

        switch(achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.kill >= 10;  //게임매니저에 있는 kill이 10 이상이면 
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime; // 끝까지 생존할시 해금 코드
                break;

        }

        if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achive.ToString(), 1);

            for(int index=0; index < uiNotice.transform.childCount; index++)
            {

                bool isActive = index == (int)achive; 
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }
    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        yield return wait;   //4초뒤에 false로 변경

        uiNotice.SetActive(false);
    }
}
