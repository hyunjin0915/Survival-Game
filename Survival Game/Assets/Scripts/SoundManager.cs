using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //클래스자체를직렬화시켜야인스펙터창에뜨게할수있으
public class Sound
{
    public string name; //곡의이름
    public AudioClip clip;//곡
}

public class SoundManager : MonoBehaviour
{
    static public SoundManager instance;

    //싱글턴화시키기.어느씬에있던같은매니저사용_1개를싱글턴화시키는방법
    #region singleton
    void Awake() //객체 생성시 최소 실행
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
            Destroy(this.gameObject);
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource audioSourceBgm;

    public string[] playSoundName;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    private void Start()
    {
        playSoundName = new string[audioSourceEffects.Length];
    }
    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if(_name == effectSounds[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[i].isPlaying)
                    {
                        playSoundName[j] = effectSounds[i].name;
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 가용 AudioSource가 사용중입니다");
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }
    public void StopSE(string _name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i]==_name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생중인" + _name + "사운드가 없습니다");
    }
}
