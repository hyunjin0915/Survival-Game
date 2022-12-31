using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate; //총의FireRate값을가져와서1초에1씩깎음>0이되면발사가능

    private AudioSource audioSource; //선언만.껍데기만만든것

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); //넣어줘야함
    }
    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        TryFire();
    }

    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime; // 1/60, update함수는1초에60프레임->1초에1씩감소하게됨
        
    }
    private void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate<=0)//누르고있는동안계속발사되게
        {
            Fire();
        }
    }
    private void Fire()
    {
        currentFireRate = currentGun.fireRate; //연사속도조절
        Shoot();
    }
    private void Shoot()
    {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("총알 발사함");
    }
    private void PlaySE(AudioClip _clip) //효과음재생
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
