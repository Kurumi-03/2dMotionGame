using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeController : MonoBehaviour
{
    void Start()
    {
        GetComponentInChildren<Animator>().SetTrigger("hit");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerManager.Instance.player.stats.DoMagicDamage(collision.GetComponent<EnemyStats>());
        }
    }
}
