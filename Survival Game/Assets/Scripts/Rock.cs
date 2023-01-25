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

    //필요한사운드이름
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;

    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);

        var clone= Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, destroyTime);

        hp--;
        if(hp<=0)
            Destruction();
        
    }
    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);

        col.enabled = false;
        Destroy(go_rock);

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime);
    }
}
