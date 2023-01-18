using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseWeapon : MonoBehaviour
{

    public string closeWeaponName; //근접무기이름

    //weapon 유형들
    public bool isHand;
    public bool isAxe;
    public bool isPickaxe;

    public float range;//공격범위
    public int damage; //공격력
    public float workSpped; //작업속도
    public float attackDelay; //공격딜레이
    public float attackDealyA; //주먹을 완전히휘둘러야공격들어가게 공격활성화 시점
    public float attackDelayB;//공격비활성화 시점


    public Animator anim; //만들어둔animatorcontroller넣기
    
}
