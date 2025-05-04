using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    Enemy enemy;

    [Header("Enemy Level Stats")]
    [SerializeField] int level;//通过提升等级来快速修改敌人属性
    [Range(0.0f, 1.0f)]
    [SerializeField] float percent;//等级提升后属性提升相对于基础值的百分比

    [Header("Currency")]
    public Stats dropCurrency;//敌人掉落金币
    [SerializeField] int baseCurrency;//基础掉落钱币
    protected override void Start()
    {
        dropCurrency.SetBaseValue(baseCurrency);

        Modifier(damage);
        Modifier(critPower);
        Modifier(critChance);

        Modifier(maxHp);
        Modifier(armor);
        Modifier(evasion);
        Modifier(magicResistance);

        Modifier(fireDamage);
        Modifier(iceDamage);
        Modifier(lightningDamage);

        Modifier(dropCurrency);

        base.Start();
        enemy = GetComponent<Enemy>();

    }

    protected override void Update()
    {
        base.Update();
    }

    void Modifier(Stats _stats)
    {
        for (int i = 1; i < level; i++)
        {
            _stats.AddModifier(Mathf.RoundToInt(_stats.GetBaseValue() * percent));
        }
    }

    //将数据和效果分开
    public override void TakeDamage(int _stats, Transform target)
    {
        base.TakeDamage(_stats, target);
        // //收到伤害击退
        // enemy.SetHitBackDir(target);
        // enemy.StartCoroutine(enemy.HitBack());
        //设置受击特效
        enemy.fx.DamageEffect();
        //设置受击伤害显示
        GameFX.Instance.popTip.CreateTip(_stats.ToString(), enemy.transform.position);
    }

    public override void Die()
    {
        base.Die();
        enemy.Die();
        PlayerManager.Instance.currency += dropCurrency.GetValue();
    }
}
