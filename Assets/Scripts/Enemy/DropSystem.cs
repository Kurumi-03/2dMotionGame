using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSystem : MonoBehaviour
{
    [SerializeField] private List<ItemData> dropItems;//可能掉落的物品
    [SerializeField] protected GameObject itemPrefab;//物品预制体
    [SerializeField] private int amount;//掉落数量

    public void DropItem()
    {
        int dropCount = 0;
        FindDrop(dropCount);
    }

    //使用递归查找掉落物品  
    void FindDrop(int dropCount)
    {
        for (int i = 0; i < dropItems.Count; i++)
        {
            //到达掉落数量则返回
            if(dropCount >= amount)
            {
                return;
            }
            //随机数小于掉率则掉落
            if (Random.Range(0, 100) < dropItems[i].dropRate)
            {
                CreateItem(dropItems[i]);
                dropCount++;
            }
        }
        //如果掉落数量小于指定数量则继续查找
        if (dropCount < amount)
        {
            FindDrop(dropCount);
        }
        if (dropCount >= 10)
        {
            GameManager.Instance.gameTip.ShowTip(GameManager.Instance.dropTips[0],GameManager.Instance.dropTips[1]);
            return;
        }
    }

    protected void CreateItem(ItemData data)
    {
        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        item.GetComponent<ItemController>().SetUpData(data, new Vector2(Random.Range(-5f, 5f), Random.Range(5f, 10f)));
    }
}
