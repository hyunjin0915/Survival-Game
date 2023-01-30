﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; //획득한아이템
    public int itemCount; //획득한개수
    public Image itemImage;//아이탬의이미지

    //필요한컴포넌트
    [SerializeField]
    private Text text_Count;
    [SerializeField]
    private GameObject go_CountImage;

    //이미지의투명도조절
    private void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
    //아이템획득
    public void AddItem(Item _item, int _count=1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage; //itemImgae로하면안되고.sprite붙여줘야함

        if (item.ItemType == Item.ItemType.Equipment)
        {
            text_Count.text = itemCount.ToString();
            go_CountImage.SetActive(false);
        }
        else
        {
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
        }
        SetColor(1);
    }
    //아이템개수조정
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        text_Count.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }
    //슬롯초기화
    private void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        go_CountImage.SetActive(false);
        text_Count.text = "0";
    }
}