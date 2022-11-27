using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    public string handName; //손이름 너클이나맨손구분
    public float range;//공격범위
    public int damage; //공격력
    public float workSpped; //작업속도
    public float attackDelay; //공격딜레이
    public float attackDealyA; //주먹을 완전히휘둘러야공격들어가게 공격활성화 시점
    public float attackDelayB;//공격비활성화 시점


    public Animator anim; //만들어둔animatorcontroller넣기
    
}
