using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{

    [SerializeField]
    private int hp; //바위의체력

    [SerializeField]
    private float destroyTime;//파편제거시간

    [SerializeField]
    private SphereCollider col;//구체 콜라이더. 파괴된이후에비활성화시킬때사용

    //필요한게임오브젝트
    [SerializeField]
    private GameObject go_rock;//일반바위
    [SerializeField]
    private GameObject go_debris;//깨진바위
    [SerializeField]
    private GameObject go_effect_prefabs;// 채굴이펙트적용

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effect_sound;
    [SerializeField]
    private AudioClip effect_sound2;


    public void Mining()
    {
        audioSource.clip = effect_sound;
        audioSource.Play();
        var clone= Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if(hp<=0)
            Destruction();
        
    }
    private void Destruction()
    {
        audioSource.clip = effect_sound2;
        audioSource.Play();

        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
