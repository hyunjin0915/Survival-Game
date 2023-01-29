using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="New Item/item")]
public class Item : ScriptableObject
{
    public string itemName; //아이템이름
    public Sprite itemImage; //아이템이미지
    public GameObject itemPrefab;//아이템의프리팹.

    public string weaponType; //무기유형

    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        ETC
    }
}
