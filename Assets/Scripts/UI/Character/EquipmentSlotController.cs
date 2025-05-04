using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotController : ItemSlotController
{
    //用以标记对应的装备类型
    public EquipmentType equipmentType;
    [SerializeField] bool isOut;//用以判断是否是游戏内ui还是主菜单ui

    void Update()
    {
        UseFlask();
    }

    //点击装备栏时将装备卸下
    public override void OnPointerDown(PointerEventData eventData)
    {
        //当没有物品时返回
        if (item == null || isOut == false)
        {
            return;
        }
        //如果是装备栏是药剂则点击使用
        // if ((item.data as EquipmentData).equipmentType == EquipmentType.Flask)
        // {
        //     (item.data as EquipmentData).UseEffect(null);
        //     InventoryManager.instance.UnEquipItem(item.data as EquipmentData);
        //     return;
        // }
        controller.itemTip.CloseTip();
        ItemData temp = item.data;
        InventoryManager.instance.UnEquipItem(temp as EquipmentData);
        InventoryManager.instance.AddToEquipment(temp);
    }

    void UseFlask()
    {
        if(item == null) return;
        if (Input.GetKeyDown(KeyCode.T) && isOut == false)
        {
            (item.data as EquipmentData).UseEffect();
            InventoryManager.instance.UnEquipItem(item.data as EquipmentData);
        }
    }
}
