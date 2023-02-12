using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//미완성 추상 class
public abstract class CloseWeaponController : MonoBehaviour
{

    //현재 장착된 Hand형 타입 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;

    protected bool isAttack = false;//공격중여부
    protected bool isSwing = false;//팔을휘두르고있는지

    protected RaycastHit hitInfo;//레이저에 닿은것의정보를저장



    protected void TryAttack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (Input.GetButton("Fire1")) //좌클릭이총알발사,세팅수정
            {
                if (!isAttack)
                {
                    StartCoroutine(AttackCoroutine());//딜레이때문에코루틴실행이효율적
                }
            }
        }
    }
    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("Attack");//트리거발동.실행

        yield return new WaitForSeconds(currentCloseWeapon.attackDealyA);
        isSwing = true;

        StartCoroutine(HitCoroutine());//공격활성화시점.

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDealyA - currentCloseWeapon.attackDelayB);

        isAttack = false;
    }

    //미완성-추상코루틴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    //완성함수이지만 추가편집이 가능한 virtual함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentCloseWeapon = _closeWeapon;
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
    }
}
