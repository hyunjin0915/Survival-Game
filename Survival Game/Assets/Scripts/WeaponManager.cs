using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(GunController))]
public class WeaponManager : MonoBehaviour
{
    //무기중복교체실행방지용
    public static bool isChangeWeapon = false;

    //현재무기와현재무기의애니메이션
    public static Transform currentWeapon;//현재무기setActive로껐다키는역할만있음
    public static Animator currentWeaponAnim;

    [SerializeField]
    private float changeWeaponDelayTime; //무기교체딜레이타임,무기교체완전히끝난시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기종류들전부관리
    [SerializeField]
    private Gun[] guns; 
    [SerializeField]
    private Hand[] hands;

    //관리차원에서쉽게무기접근이가능하도록만듦
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Hand> handDictionary = new Dictionary<string, Hand>();

    //필요한컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;

    //현재무기의타입. 총/도끼...
    [SerializeField]
    private string currentWeaponType;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].handName, hands[i]);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isChangeWeapon)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                //무기교체실행(맨손)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //무기교체
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));

            }
        }
    }

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN":
                theGunController.CancleFineSight();//정조준상태취소하기
                theGunController.CancelReload(); //장전중일때무기변경안되게재장전취소시키기
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
        }
    }
    private void WeaponChange(string _type, string _name)
    {
        if(_type=="GUN")
            theGunController.GunChange(gunDictionary[_name]);
       
        else if(_type=="HAND")
            theHandController.HandChange(handDictionary[_name]);
    }
}
