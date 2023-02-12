using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    //자기자신을 인스턴스로 어디서든 대입할 수 있게 
    static public DragSlot instance;

    public Slot dragSlot;

    [SerializeField]
    private Image imageItem;

     void Start()
    {
        instance = this;
    }
    public void DragSetImage(Image _itemImgae)
    {
        imageItem.sprite = _itemImgae.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
