using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //총마다 다른 값을 가지도록 변수들을 선언함
    public string gunName; //총의 이름 여러개의 총 구분역할
    public float range; //총의 사정거리
    public float accuracy; //총의 정확도
    public float fireRate; //연사속도
    public float reloadTime; //재장전속도

    public int damage; //총의 데미지
    
    public int reloadBulletCount; //총알 재장전 개수
    public int currentBulletCount; //현재 남은 총알 개수
    public int maxBulletCount; //최대 소유 가능 총알개수
    public int carryBulletCount; //현재 소유 총알 개수

    public float retroActionForce; //반동세기
    public float retroActionFineSightForce; //정조준시의 반동세기

    public Vector3 fineSightOriginPos;
    public Animator anim;
    public ParticleSystem muzzleFlash;//화염등 이팩트관리

    public AudioClip fire_Sound;//총마다 발사 사운드

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
