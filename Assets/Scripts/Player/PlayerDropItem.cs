using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropItem : DropSystem
{
    public void Drop()
    {
        List<InventoryItem> drops = new List<InventoryItem>();
        foreach(EquipmentData euip in InventoryManager.instance.equipmentList)
        {
            drops.Add(new InventoryItem(euip));
        }

        foreach(InventoryItem drop in drops)
        {
            CreateItem(drop.data); 
            InventoryManager.instance.UnEquipItem(drop.data as EquipmentData);
        }
    }
}
