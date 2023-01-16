using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    //현재활성화여부
    public static bool isActivate = false;

    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    private bool isAttack = false;//공격중여부
    private bool isSwing = false;//팔을휘두르고있는지

    private RaycastHit hitInfo;//레이저에 닿은것의정보를저장


    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            TryAttack();
        }
    }

    private void TryAttack()
    {
        if(Input.GetButton("Fire1")) //좌클릭이총알발사,세팅수정
        {
            if(!isAttack)
            {
                StartCoroutine(AttackCoroutine());//딜레이때문에코루틴실행이효율적
            }
        }
    }
    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");//트리거발동.실행

        yield return new WaitForSeconds(currentHand.attackDealyA);
        isSwing = true;

        StartCoroutine(HitCoroutine());//공격활성화시점.

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDealyA - currentHand.attackDelayB);

        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while(isSwing) //delayA와 dealyB사이에서 계속공격성공여부체크
        {
            if(CheckObject()) //충돌했으면
            {
                isSwing = false; //한번적중했으면다시실행되지않도록
                Debug.Log(hitInfo.transform.name);//충돌한것의hierarchy상의이름가져옴 
            }
           
            yield return null;//whild문도는동안1프레임대기
        }
    }
    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward,out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    public void HandChange(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentHand = _hand;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }

}
