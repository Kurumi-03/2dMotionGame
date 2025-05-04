using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrency : MonoBehaviour
{
    public int lost;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>()!=null){
            PlayerManager.Instance.currency += lost;
            Destroy(gameObject);
        }
    }
}
