using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuffEffect", menuName = "Item/ItemEffect/BuffEffect")]
public class BuffEffect : ItemEffect
{
    [SerializeField] float during;//buff持续时间
    [SerializeField] int change;//buff想要修改的数值
    [SerializeField] StatsType type;//buff想要修改的值的类型
    public override void Effect(Transform target)
    {
        PlayerManager.Instance.player.stats.TempModifier(change, during, 
            PlayerManager.Instance.player.stats.GetStatsType(type));
    }

    
}
