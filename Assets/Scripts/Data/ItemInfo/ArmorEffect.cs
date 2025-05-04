using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArmorEffect", menuName = "Item/ItemEffect/ArmorEffect")]
public class ArmorEffect : ItemEffect
{
    [SerializeField] float during;//持续时间
    [SerializeField] float radius;//作用范围半径
    [Range(0, 1)]
    [SerializeField] float percent;//作用于玩家班百分比血量
    public override void Effect(Transform target)
    {
        if (PlayerManager.Instance.player.stats.currentHp > PlayerManager.Instance.player.stats.GetMaxHP() * percent)
        {
            //玩家血量低于一定比例才会触发效果
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.position, radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().SlowAction(during, 1);
            }
        }
    }
}
