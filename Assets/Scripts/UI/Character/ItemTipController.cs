using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTipController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI typeText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] float xOffset;//显示位置x的偏移倍率
    [SerializeField] float yOffset;//显示位置y的偏移倍率
    float defaultX,defaultY;
    void Awake()
    {
        defaultX = xOffset;
        defaultY = yOffset;
    }

    public void ShowTip(ItemData item)
    {
        if(item == null) return;
        gameObject.SetActive(true);
        //设置提示显示位置
        if (item.itemType == ItemType.Equipment)
        {
            EquipmentTip(item as EquipmentData);
        }
        else if (item.itemType == ItemType.Material)
        {

        }


    }

    public void CloseTip()
    {
        gameObject.SetActive(false);
        xOffset = defaultX;
        yOffset = defaultY;
    }

    void EquipmentTip(EquipmentData equipment)
    {   
        nameText.text = equipment.name;
        typeText.text = equipment.equipmentType.ToString();
        descriptionText.text = equipment.GetDescription();
        //设置显示位置
        Vector2 mousePos = Input.mousePosition;
        if (mousePos.x > Screen.width / 2)
        {
            xOffset *= -1;
        }
        if (mousePos.y > Screen.height / 2)
        {
            yOffset *= -1;
        }
        transform.position = new Vector2(mousePos.x + Screen.width * xOffset, mousePos.y + Screen.height * yOffset);
    }


}
