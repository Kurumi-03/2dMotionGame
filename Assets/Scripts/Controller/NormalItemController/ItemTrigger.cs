using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    private ItemController controller;
    void Start()
    {
        controller = GetComponentInParent<ItemController>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //死亡后不做检测  避免死亡后还能拾取物品
        if(PlayerManager.Instance.player.stats.isDead) return;
        if (collision.GetComponent<Player>() != null)
        {
            controller.PickUp();
        }
    }
}
