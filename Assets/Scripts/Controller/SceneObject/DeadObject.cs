using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        //掉落的是实体
        if (collision.GetComponent<Entity>() != null)
        {
            collision.GetComponent<CharacterStats>().Die();
        }
        //掉落的是道具
        if (collision.GetComponent<ItemController>() != null)
        {
            Destroy(collision.gameObject);
        }
    }
}
