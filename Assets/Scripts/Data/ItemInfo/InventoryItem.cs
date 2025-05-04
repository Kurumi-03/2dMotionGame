using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;//物品数据
    public int amount;//数量

    public InventoryItem(ItemData data)
    {
        this.data = data;
        amount = 1;//新添加时默认数量为1
    }

    public void AddItem()
    {
        amount++;
    }

    public void RemoveItem()
    {
        amount--;
    }
}
