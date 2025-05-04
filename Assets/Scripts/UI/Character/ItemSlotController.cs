using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public InventoryItem item;
    Image itemIcon;
    TextMeshProUGUI itemAmount;
    protected UIController controller;
    void Awake()
    {
        itemIcon = GetComponent<Image>();
        itemAmount = GetComponentInChildren<TextMeshProUGUI>();
        controller = GetComponentInParent<UIController>();
        item = null;
    }


    //根据库存的道具数量进行更新
    public void UpdateItemSlot(InventoryItem itemData)
    {
        //避免ui未被唤醒时调用
        if (itemIcon == null) return;
        item = itemData;
        itemIcon.sprite = itemData.data.itemIcon;
        itemIcon.color = Color.white;
        if (itemData.amount > 1)
        {
            itemAmount.text = itemData.amount.ToString();
        }
        else
        {
            itemAmount.text = "";
        }
    }

    //使图片和文字透明
    public void ClearItemSlot()
    {
        //避免ui未被唤醒时调用
        if (itemIcon == null) return;
        item = null;
        itemIcon.sprite = null;
        itemIcon.color = Color.clear;
        itemAmount.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //当没有物品时返回  避免误触
        if (item == null)
        {
            return;
        }
        //在点击物品时按下ctrl键可以扔掉物品
        if (Input.GetKey(KeyCode.LeftControl))
        {
            InventoryManager.instance.RemoveItem(item.data);
            controller.itemTip.CloseTip();
            return;
        }
        if (item.data.itemType == ItemType.Equipment)
        {
            InventoryManager.instance.EquipItem(item.data);
        }
        //点击后需要将信息提示关闭
        controller.itemTip.CloseTip();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //当没有物品时返回  避免误触
        if (item == null)
        {
            return;
        }
        controller.itemTip.ShowTip(item.data);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //当没有物品时返回  避免误触
        if (item == null)
        {
            return;
        }
        controller.itemTip.CloseTip();
    }
}
