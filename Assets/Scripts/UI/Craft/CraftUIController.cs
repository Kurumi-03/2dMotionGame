using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUIController : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] ItemSlotController[] materials;
    [SerializeField] Button craft;

    public void SetUpUI(EquipmentData data)
    {
        icon.sprite = data.itemIcon;
        nameText.text = data.itemName;
        descriptionText.text = data.GetDescription();
        //每次刷新时先重置所有插槽
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].ClearItemSlot();
        }
        for (int i = 0; i < data.materials.Count; i++)
        {
            materials[i].UpdateItemSlot(data.materials[i]);
        }
        //先重置按钮的所有注册再重新注册
        craft.onClick.RemoveAllListeners();
        craft.onClick.AddListener(() =>
        {
            InventoryManager.instance.CanCraftItem(data);
        });
    }
}
