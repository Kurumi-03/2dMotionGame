using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Item/ItemEffect/HealEffect")]
public class HealEffect : ItemEffect
{
    [SerializeField] int healValue;//治疗值
    
    public override void Effect(Transform target)
    {
        PlayerManager.Instance.player.GetComponent<PlayerStats>().Heal(healValue);
    }

}
