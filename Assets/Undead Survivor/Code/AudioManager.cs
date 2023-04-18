using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;


    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    private void Awake()
    {
        instance = this;
        Init();
    }

    public enum Sfx { Dead, Hit, LevelUp=3, Lose, Melee, Range=7, Select, Win }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;  //배경음을 담당하는 자식 오브젝트 생성
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;

        bgmPlayer.clip = bgmClip;



        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;  //효과음을 담당하는 자식 오브젝트 생성
        sfxPlayers = new AudioSource[channels];

        for (int index=0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].volume = sfxVolume;
        }

    }

    public void PlaySfx(Sfx sfx) 
    {

        for(int index=0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if(sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx ==Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }


    }

}
