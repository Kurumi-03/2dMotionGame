using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftSlotController : ItemSlotController
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;
    EquipmentData equipmentData;

    void Start()
    {
        // controller.craftInfo.SetUpUI(controller.GetComponentInChildren<CraftListController>().GetDefaultEquip());
    }

    //后期可改
    public void SetUpSlot(ItemData data)
    {
        if (data.itemType == ItemType.Equipment)
        {
            equipmentData = data as EquipmentData;
            icon.sprite = equipmentData.itemIcon;
            nameText.text = equipmentData.itemName;
        }
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(equipmentData == null) return;
        controller.craftInfo.SetUpUI(equipmentData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
