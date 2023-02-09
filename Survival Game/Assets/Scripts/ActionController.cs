﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{

    [SerializeField]
    private float range; //습득가능최대거리설정

    private bool pickupActivated = false;//습득가능할시true

    private RaycastHit hitInfo;//충돌체정보저장

    [SerializeField]
    private LayerMask layerMask; //특정레이어에대해서만반응하도록

    //필요한컴포넌트
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;


    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }
    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }
    private void CanPickUp()
    {
        if(pickupActivated)
        {
            if(hitInfo.transform!=null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
            }
        }
    }
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
            InfoDisappear();
    }

    private void ItemInfoAppear()
    {
        pickupActivated = true;//주울수있음
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>";
    }
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);

    }
}
