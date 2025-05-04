using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Material
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;//物品类型
    public string itemName;//物品名字
    public string itemId { get; private set; }//用以唯一区别道具   从资源路径转得
    public Sprite itemIcon;//物品图标

    [Range(0, 100)]
    public float dropRate;//掉落概率

    protected StringBuilder des = new StringBuilder();//保存物品描述的字符串
    protected int defaultRaw = 5;//描述的默认行

    public virtual string GetDescription()
    {
        return des.ToString();
    }

    public string GetItemID()
    {
        return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(this));
    }
}
