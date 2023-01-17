using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //현재활성화여부
    public static bool isActivate = true;

    [SerializeField]
    private Gun currentGun;

    private float currentFireRate; //총의FireRate값을가져와서1초에1씩깎음,0이되면발사가능,연사속도계산

    private bool isReload = false; //false일때만발사가되도록
    [HideInInspector]
    public bool isFineSightMode = false; //정조준상태여부

    private Vector3 originPos; //정조준하고나서 돌아올 원래 벡터값

    private AudioSource audioSource; //선언만.껍데기만만든것

    //레이저 충돌 정보 받아옴
    private RaycastHit hitInfo;

    //필요한컴포넌트
    [SerializeField]
    private Camera theCam; //게임화면이카메라시점이라
    private Crosshair theCrosshair;

    [SerializeField]
    private GameObject hit_effect_prefab; //피격effect

    private void Start()
    {
        originPos = Vector3.zero;
        audioSource = GetComponent<AudioSource>(); //넣어줘야함
        theCrosshair = FindObjectOfType<Crosshair>();

        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;
    }
    // Update is called once per frame
    void Update()
    {
        if(isActivate)
        {
            GunFireRateCalc();
            TryFire();
            TryReload();
            TryFineSight();
        }
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; // 1/60, update함수는1초에60프레임->1초에1씩감소하게됨
        
    }
    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate<=0 && !isReload)//누르고있는동안계속발사되게
        {
            Fire();
        }
    }
    private void Fire()
    {
        if(!isReload)
        {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
            {
                CancleFineSight();
                StartCoroutine(ReloadCoroutine());
            }
        }
    }
    private void Shoot()
    {
        theCrosshair.FireAnimation();
        currentGun.currentBulletCount--; //발사뒤에총알개수줄이기
        currentFireRate = currentGun.fireRate; //발사가이뤄진뒤에 연사속도재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Hit(); //발사하는족족맞는걸로

        StopAllCoroutines();
        StartCoroutine(RetroActionCoroutine());//총기반동코루틴실행
    }

    private void Hit()
    {
        //충돌한게있어서반환값이있으면t
        if (Physics.Raycast(theCam.transform.position, theCam.transform.forward + 
            new Vector3(Random.Range(-theCrosshair.GetAccuracy()-currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        Random.Range(-theCrosshair.GetAccuracy() - currentGun.accuracy, theCrosshair.GetAccuracy() + currentGun.accuracy),
                        0), 
                out hitInfo, currentGun.range))
        {
           var clone= Instantiate(hit_effect_prefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }
    private void TryReload() //수동재장전구현
    {
        if(Input.GetKeyDown(KeyCode.R)&& !isReload && currentGun.currentBulletCount < currentGun.reloadBulletCount)
        {
            CancleFineSight();
            StartCoroutine(ReloadCoroutine());
        }
    }
    public void CancelReload()
    {
        if(isReload)
        {
            StopAllCoroutines();
            isReload = false;
        }
    }
    IEnumerator ReloadCoroutine()
    {
        if(currentGun.carryBulletCount>0)
        {
            isReload = true;
            currentGun.anim.SetTrigger("Reload");

            currentGun.carryBulletCount += currentGun.currentBulletCount;//현재소유하고있는총알개수가날라가지않게
            currentGun.currentBulletCount = 0;

            yield return new WaitForSeconds(currentGun.reloadTime);

           if (currentGun.carryBulletCount >= currentGun.reloadBulletCount) //남은총알개수가재장전개수보다많으면
           {
               currentGun.currentBulletCount = currentGun.reloadBulletCount; //전부재장전
               currentGun.carryBulletCount -= currentGun.reloadBulletCount;
           }
           else
           {
               currentGun.currentBulletCount = currentGun.carryBulletCount;
               currentGun.carryBulletCount = 0;
           }
            isReload = false;
        }
        else
        {
            Debug.Log("소유한 총알이 없습니다.");
        }
    }
    private void TryFineSight()
    {
        if(Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }
    public void CancleFineSight()
    {
        if (isFineSightMode)
            FineSight();
    }
    private void FineSight()
    {
        isFineSightMode = !isFineSightMode;
        currentGun.anim.SetBool("FineSightMode", isFineSightMode);
        theCrosshair.FineSightAnimation(isFineSightMode);

        if(isFineSightMode)
        {
            StopAllCoroutines();
            StartCoroutine(FineSightActivateCoroutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(FineSightDeactivateCoroutine());
        }
    }

    IEnumerator FineSightActivateCoroutine()
    {
        while(currentGun.transform.localPosition != currentGun.fineSightOriginPos) //정조준위치가될때까지반복
        {
            //자식객체일때localPosition사용
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.2f);
            yield return null; //1프레임동안
        }
    }
    IEnumerator FineSightDeactivateCoroutine()
    {
        while (currentGun.transform.localPosition != originPos) //정조준위치가될때까지반복
        {
            //자식객체일때localPosition사용
            currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.2f);
            yield return null; //1프레임동안
        }
    }

    IEnumerator RetroActionCoroutine()
    {
        Vector3 recoilBack = new Vector3(currentGun.retroActionForce,originPos.y,originPos.z); //정조준안했을때최대반동
        Vector3 retroActionRecoilBack = new Vector3(currentGun.retroActionFineSightForce, currentGun.fineSightOriginPos.y,currentGun.fineSightOriginPos.z); //정조준했을때최대반동

        if(!isFineSightMode) //정조준상태가 아닐때
        {
            currentGun.transform.localPosition = originPos; //처음으로되돌린후에반동을주면 반동이 더 눈에 뛸수 있음

            //반동시작
            while(currentGun.transform.localPosition.x<=currentGun.retroActionForce-0.02f) //lerp사용했기 때문에 0.02빼줌
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, recoilBack, 0.4f);
                yield return null;
            }
            //원위치
            while(currentGun.transform.localPosition != originPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, originPos, 0.1f);
                yield return null;
            }
        }
        else//정조준상태일때
        {
            currentGun.transform.localPosition = currentGun.fineSightOriginPos; 

            //반동시작
            while (currentGun.transform.localPosition.x <= currentGun.retroActionFineSightForce - 0.02f) //lerp사용했기 때문에 0.02빼줌
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, retroActionRecoilBack, 0.4f);
                yield return null;
            }
            //원위치
            while (currentGun.transform.localPosition != currentGun.fineSightOriginPos)
            {
                currentGun.transform.localPosition = Vector3.Lerp(currentGun.transform.localPosition, currentGun.fineSightOriginPos, 0.1f);
                yield return null;
            }
        }
    }
    private void PlaySE(AudioClip _clip) //효과음재생
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
    public Gun GetGun()
    {
        return currentGun;
    }
    public bool GetFineSightMode()
    {
        return isFineSightMode;
    }
    public void GunChange(Gun _gun)
    {
        if(WeaponManager.currentWeapon!=null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        currentGun = _gun;
        WeaponManager.currentWeapon = currentGun.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentGun.anim;

        currentGun.transform.localPosition = Vector3.zero;
        currentGun.gameObject.SetActive(true);

        isActivate = true;
    }
}
