using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour, ISave
{
    public static InventoryManager instance;
    public List<InventoryItem> startInventory = new List<InventoryItem>();//初始物品
    public List<InventoryItem> equipmentInventroy = new List<InventoryItem>();//装备仓库
    public List<InventoryItem> materialInventory = new List<InventoryItem>();//材料仓库
    public List<EquipmentData> equipmentList = new List<EquipmentData>();//身上装备列表
    public List<EquipmentData> craftList = new List<EquipmentData>();//可合成装备列表
    [SerializeField] Transform equipmentPanel;//装备仓库面板
    [SerializeField] Transform materialPanel;//仓库列表面板
    [SerializeField] Transform playerPanel;//玩家装备面板
    [SerializeField] Transform statsPanel;//玩家信息面板
    [SerializeField] Transform flaskPanel;//药剂面板
    [SerializeField] List<InventoryItem> loadItem;//从本地数据中得到的item
    [SerializeField] List<EquipmentData> loadEquipment;//从本地数据中得到的身上的装备
    StatsSlotController[] statsSlots;

    void Instance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        Instance();
        Init();
    }
    void Start()
    {
        InitEquipment();
    }

    public void Init()
    {
        statsSlots = statsPanel.GetComponentsInChildren<StatsSlotController>();
    }

    public void UpdateUI()
    {
        //刷新前需要将所有ui重置
        for (int i = 0; i < equipmentPanel.childCount; i++)
        {
            equipmentPanel.GetChild(i).GetComponent<ItemSlotController>().ClearItemSlot();
        }
        for (int i = 0; i < materialPanel.childCount; i++)
        {
            materialPanel.GetChild(i).GetComponent<ItemSlotController>().ClearItemSlot();
        }
        for (int i = 0; i < playerPanel.childCount; i++)
        {
            playerPanel.GetChild(i).GetComponent<ItemSlotController>().ClearItemSlot();
        }
        flaskPanel.GetComponent<ItemSlotController>().ClearItemSlot();

        //然后根据数据更新ui
        for (int i = 0; i < equipmentInventroy.Count; i++)
        {
            equipmentPanel.GetChild(i).GetComponent<ItemSlotController>().UpdateItemSlot(equipmentInventroy[i]);
        }
        for (int i = 0; i < materialInventory.Count; i++)
        {
            materialPanel.GetChild(i).GetComponent<ItemSlotController>().UpdateItemSlot(materialInventory[i]);
        }
        //根据装备类型来更新
        for (int i = 0; i < equipmentList.Count; i++)
        {
            for (int j = 0; j < playerPanel.childCount; j++)
            {
                if (equipmentList[i].equipmentType == playerPanel.GetChild(j).
                    GetComponent<EquipmentSlotController>().equipmentType)
                {
                    playerPanel.GetChild(j).GetComponent<ItemSlotController>().UpdateItemSlot(new InventoryItem(equipmentList[i]));
                }
                //刷新游戏内ui
                if(equipmentList[i].equipmentType == EquipmentType.Flask)
                {
                    flaskPanel.GetComponent<ItemSlotController>().UpdateItemSlot(new InventoryItem(equipmentList[i]));
                }
            }
        }
        UpdateStatsUI();
    }

    //每次添加物品时需要做判定避免溢出
    public bool CanAddEquip(ItemData data)
    {
        // Debug.Log("装备仓库:"+equipmentInventroy.Count+"  装备面板数量:"+equipmentPanel.childCount);
        if (equipmentInventroy.Count >= equipmentPanel.childCount)
        {
            for (int i = 0; i < equipmentInventroy.Count; i++)
            {
                //当检测到库存里有的装备时可以拾取
                if (equipmentInventroy[i].data == data)
                {
                    return true;
                }
            }
            return false;
        }
        return true;
    }

    public void AddItem(ItemData data)
    {
        if (data.itemType == ItemType.Equipment && CanAddEquip(data))
        {
            AddToEquipment(data);
        }
        else if (data.itemType == ItemType.Material)
        {
            AddToMaterial(data);
        }
    }

    public void AddToEquipment(ItemData data)
    {
        //在物品列表中查找是否已经存在该物品
        foreach (InventoryItem item in equipmentInventroy)
        {
            if (item.data == data)
            {
                item.AddItem();
                //每次添加时更新ui
                UpdateUI();
                return;
            }
        }
        //如果不存在则添加新物品
        InventoryItem newItem = new InventoryItem(data);
        equipmentInventroy.Add(newItem);
        UpdateUI();
    }

    public void AddToMaterial(ItemData data)
    {
        //在整备列表中查找是否已经存在该装备
        foreach (InventoryItem item in materialInventory)
        {
            if (item.data == data)
            {
                item.AddItem();
                UpdateUI();
                return;
            }
        }
        //如果不存在则添加新装备
        InventoryItem newItem = new InventoryItem(data);
        materialInventory.Add(newItem);
        UpdateUI();
    }

    public void RemoveItem(ItemData data)
    {
        if (data.itemType == ItemType.Equipment)
        {
            RemoveToEquipment(data);
        }
        else if (data.itemType == ItemType.Material)
        {
            RemoveToMaterial(data);
        }
        UpdateUI();
    }

    public void RemoveToEquipment(ItemData data)
    {
        //在物品列表中查找是否已经存在该物品
        foreach (InventoryItem item in equipmentInventroy)
        {
            if (item.data == data)
            {
                item.RemoveItem();
                if (item.amount <= 0)
                {
                    equipmentInventroy.Remove(item);
                }
                //每次添加时更新ui
                UpdateUI();
                return;
            }
        }
    }

    public void RemoveToMaterial(ItemData data)
    {
        //在整备列表中查找是否已经存在该装备
        foreach (InventoryItem item in materialInventory)
        {
            if (item.data == data)
            {
                item.RemoveItem();
                if (item.amount <= 0)
                {
                    materialInventory.Remove(item);
                }
                UpdateUI();
                return;
            }
        }
    }

    //装备道具
    public void EquipItem(ItemData item)
    {
        EquipmentData newEquip = item as EquipmentData;
        //排查是否有同类型的装备
        EquipmentData oldEquip = null;
        foreach (EquipmentData equip in equipmentList)
        {
            //如果有则将新的装备替换进装备列表  并需要将原来的装备放回仓库
            if (equip.equipmentType == newEquip.equipmentType)
            {
                oldEquip = equip;
                RemoveToEquipment(newEquip);
                equipmentList.Add(newEquip);
                newEquip.AddModifier();
                UnEquipItem(oldEquip);
                AddToEquipment(oldEquip);
                return;
            }
        }
        //如果没有则直接添加新装备
        equipmentList.Add(newEquip);
        RemoveToEquipment(newEquip);
        //更新数据
        newEquip.AddModifier();
        UpdateUI();
    }

    //卸下装备
    public void UnEquipItem(EquipmentData item)
    {
        equipmentList.Remove(item);
        item.RemoveModifier();
        UpdateUI();
    }

    //是否能够合成物品
    public bool CanCraftItem(EquipmentData equip)
    {
        List<InventoryItem> remove = new List<InventoryItem>();
        foreach (InventoryItem item in equip.materials)
        {
            foreach (InventoryItem material in materialInventory)
            {
                //具有材料并且材料数足够
                if (item.data == material.data && item.amount <= material.amount)
                {
                    remove.Add(material);
                }
            }
        }
        if (remove.Count != equip.materials.Count)
        {
            GameManager.Instance.gameTip.ShowTip(GameManager.Instance.craftTips[0], GameManager.Instance.craftTips[1]);
            return false;
        }
        //按对应材料的消耗
        foreach (InventoryItem item in remove)
        {
            for (int i = 0; i < item.amount; i++)
            {
                RemoveToMaterial(item.data);
            }
        }
        AddToEquipment(equip);
        return true;
    }

    //初始装备
    public void InitEquipment()
    {
        if (loadEquipment.Count > 0)
        {
            foreach (EquipmentData eqip in loadEquipment)
            {
                EquipItem(eqip);
            }
        }
        //加载保存在本地的装备
        if (loadItem.Count > 0)
        {
            foreach (InventoryItem item in loadItem)
            {
                //还要加上数量
                for (int i = 0; i < item.amount; i++)
                {
                    AddItem(item.data);
                }
            }
            //有装备就不能再加初始装备了
            return;
        }
        //只有第一次进入游戏才会加入基础装备
        if (SaveManager.Instance.firstGame == true)
        {
            //初始的默认装备
            foreach (InventoryItem item in startInventory)
            {
                //还要加上数量
                for (int i = 0; i < item.amount; i++)
                {
                    AddItem(item.data);
                }
            }
        }
    }

    public void UpdateStatsUI()
    {
        //刷新玩家状态显示
        for (int i = 0; i < statsSlots.Length; i++)
        {
            statsSlots[i].GetComponent<StatsSlotController>().UpdateSlotValue();
        }
    }

    //根据装备类型使用没有cd效果的装备
    public void UseEquipmentEffect(EquipmentType type, Transform target)
    {
        //当未装备时返回
        if (equipmentList.Count == 0)
        {
            return;
        }
        foreach (EquipmentData equip in equipmentList)
        {
            if (equip.equipmentType == type)
            {
                equip.UseEffect(target);
            }
        }
    }

    //有cd的效果需要单独写
    float armorLastTime;
    public void ArmorEffect()
    {
        EquipmentData equipment = null;
        foreach (EquipmentData equip in equipmentList)
        {
            if (equip.equipmentType == EquipmentType.Armor)
            {
                equipment = equip;
            }
        }
        if (Time.time > armorLastTime && equipment != null)
        {
            armorLastTime = Time.time + equipment.effectCoolDown;
            equipment.UseEffect(PlayerManager.Instance.player.transform);
        }
    }

    //游戏数据保存
    public void SaveData(ref GameData data)
    {
        //使用前先清空 避免重复添加
        data.itemInventroy.Clear();
        data.equipments.Clear();
        //保存仓库内的装备
        for (int i = 0; i < equipmentInventroy.Count; i++)
        {
            data.itemInventroy.Add(equipmentInventroy[i].data.GetItemID(), equipmentInventroy[i].amount);
        }
        //保存仓库内的
        for (int i = 0; i < materialInventory.Count; i++)
        {
            data.itemInventroy.Add(materialInventory[i].data.GetItemID(), materialInventory[i].amount);
        }
        //保存身上的装备
        for (int i = 0; i < equipmentList.Count; i++)
        {
            data.equipments.Add(equipmentList[i].GetItemID());
        }
    }

    //游戏数据加载
    public void LoadData(GameData data)
    {
        if (data == null) return;
        //得到存储的所有数据文件id  item和equipment都在
        string[] allID = AssetDatabase.FindAssets("", new[] { SaveManager.Instance.assetDataPath });
        List<ItemData> items = new List<ItemData>();
        for (int i = 0; i < allID.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(allID[i]);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            items.Add(item);
            // Debug.Log("第" + i + "个道具为:" + items[i].name);
        }

        //得到数据文件中的ItemInventroy存储内容
        for (int i = 0; i < items.Count; i++)
        {
            //将文件id进行对比 
            foreach (KeyValuePair<string, int> item in data.itemInventroy)
            {
                //因为资源会识别到文件夹 所以有些数据会为空
                if (items[i] != null && items[i].GetItemID() == item.Key)
                {
                    // Debug.Log("itemName:" + items[i].name);
                    InventoryItem newItem = new InventoryItem(items[i]);
                    newItem.amount = item.Value;
                    loadItem.Add(newItem);
                }
            }
        }

        //得到数据文件中存储的equipment存储内容
        for (int i = 0; i < items.Count; i++)
        {
            foreach (string equip in data.equipments)
            {
                if (items[i] != null && items[i].GetItemID() == equip)
                {
                    loadEquipment.Add(items[i] as EquipmentData);
                }
            }
        }

    }
}
