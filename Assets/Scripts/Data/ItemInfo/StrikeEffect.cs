using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StrikeEffect", menuName = "Item/ItemEffect/StrikeEffect")]
public class StrikeEffect : ItemEffect
{
    [SerializeField] private GameObject effectPrefab;//击中特效预制体
    [SerializeField] private float destroyTime;//特效销毁时间
    public override void Effect(Transform target)
    {
        GameObject strikeEffect = Instantiate(effectPrefab, target.position, Quaternion.identity);
        Destroy(strikeEffect, destroyTime);
    }
}
