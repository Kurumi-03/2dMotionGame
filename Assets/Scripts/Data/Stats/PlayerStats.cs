using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Player player;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void DoDamage(CharacterStats target, float damagePercent = 0)
    {
        base.DoDamage(target, damagePercent);
        //设置攻击时暴击和普通攻击的效果
        if (CheckCritChance())
        {
            player.fx.CritAttackEffect(target.transform, player.faceDir);
        }
        else
        {
            player.fx.NormalAttackEffect(target.transform);
        }
    }

    //将数据和效果分开
    public override void TakeDamage(int _damage, Transform target)
    {
        //在受到伤害前执行护甲的效果
        InventoryManager.instance.ArmorEffect();
        base.TakeDamage(_damage, target);
        //设置击退
        // player.SetHitBackDir(target);
        // player.StartCoroutine(player.HitBack());
        //设置受击特效
        player.fx.DamageEffect();
        //设置受击伤害显示
        GameFX.Instance.popTip.CreateTip(_damage.ToString(), player.transform.position);
    }

    protected override void Evasion(CharacterStats stats)
    {
        SkillManager.Instance.dodge.CreateEvasionClone(stats.transform, stats.GetComponent<Entity>().faceDir);
    }

    public override void Die()
    {
        base.Die();
        player.Die();
        GameManager.Instance.lostCurrency = PlayerManager.Instance.currency;
        PlayerManager.Instance.currency = 0;
        GetComponent<PlayerDropItem>().Drop();
    }
}
