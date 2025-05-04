using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IceAndFireEffect", menuName = "Item/ItemEffect/IceAndFireEffect")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject effectPrefab;//击中特效预制体
    [SerializeField] private float xVelocity;//水平速度
    [SerializeField] private float destroyTime;//特效销毁时间
    public override void Effect(Transform target)
    {
        //普通攻击到第三段时才可以触发效果
        Player player = PlayerManager.Instance.player;
        if (player.primaryAttackState.combo == 2)
        {
            GameObject strikeEffect = Instantiate(effectPrefab, player.transform.position, player.transform.rotation);
            //设置特效的速度
            strikeEffect.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.faceDir, 0);
            Destroy(strikeEffect, destroyTime);
        }

    }
}
