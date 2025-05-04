using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Transform equipParent;
    [SerializeField] EquipmentType type;
    [SerializeField] GameObject equipSlotPrefab;
    [SerializeField] CraftUIController craftInfo;
    public List<EquipmentData> datas;

    void Start()
    {
        SetDefaultSlot();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CreateCraftSlot();
    }

    public void DestroyList()
    {
        datas.Clear();
        for (int i = 0; i < equipParent.childCount; i++)
        {
            Destroy(equipParent.GetChild(i).gameObject);
        }
    }

    public void CreateCraftSlot()
    {
        //每次显示新的slot之前需要将之前的清空
        DestroyList();
        List<EquipmentData> allEquip = InventoryManager.instance.craftList;
        for (int i = 0; i < allEquip.Count; i++)
        {
            if (allEquip[i].equipmentType == type)
            {
                datas.Add(allEquip[i]);
                GameObject slot = Instantiate(equipSlotPrefab, equipParent);
                slot.GetComponent<CraftSlotController>().SetUpSlot(allEquip[i]);
            }
        }
    }

    public void SetDefaultSlot()
    {
        transform.parent.GetChild(0).GetComponent<CraftListController>().CreateCraftSlot();
        craftInfo.SetUpUI(GetDefaultEquip());
    }

    public EquipmentData GetDefaultEquip()
    {
        return transform.parent.GetChild(0).GetComponent<CraftListController>().datas[0];
    }
}
