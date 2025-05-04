using UnityEngine;

public class ItemController : MonoBehaviour
{
    SpriteRenderer sr;
    Rigidbody2D rb;
    public ItemData itemData;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetUpData(ItemData item, Vector2 velocity)
    {
        itemData = item;
        sr.sprite = itemData.itemIcon;
        rb.velocity = velocity;
    }

    public void PickUp()
    {
        //装备仓库满后不能拾取装备
        if (!InventoryManager.instance.CanAddEquip(itemData) && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }
        InventoryManager.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
