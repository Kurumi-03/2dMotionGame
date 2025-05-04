using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buff
{
    None,//无buff状态
    Ice,//着火buff     持续伤害
    Fire,//冰冻伤害    破甲
    Lightning,//雷电伤害  减少命中率
}

public enum StatsType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,

    Damage,
    CritPower,
    CritChance,

    Health,
    Armor,
    Evasion,
    MagicResist,


    FireDamage,
    IceDamage,
    LightningDamage,
}

//角色数据统计
public class CharacterStats : MonoBehaviour
{
    [Header("Major")]
    public Stats strength;//提升伤害加成和暴击伤害倍率
    public Stats agility;//闪避率和暴击率
    public Stats intelligence;//魔法攻击和魔法抗性
    public Stats vitality;//血量上限

    [Header("Offensive")]
    public Stats damage;//基础伤害
    public Stats critPower;//基础暴击伤害倍率
    public Stats critChance;//基础暴击率

    [Header("Defensive")]
    public Stats maxHp;//角色基础满状态血量
    public Stats armor;//基础护甲
    public Stats evasion;//基础闪避率
    public Stats magicResistance;//魔法抗性

    [Header("Magic Major")]
    public Buff buff = Buff.None;

    public Stats fireDamage;//火焰伤害
    public Stats iceDamage;//冰冻伤害
    public Stats lightningDamage;//雷电伤害

    public float fireTime;//持续时间
    public float iceTime;
    public float lightningTime;

    [Header("Magic Effect")]
    public float fireCoolDown;//持续伤害的间隔
    public float firePercent;//持续伤害相对于火焰伤害的比率
    public float iceArmor;//破甲比率
    public float icePercent;//冰冻降低速度的比率
    public float lightningChance;//为敌人增加的闪避率
    public GameObject thunderPrefab;//雷电预制体
    public float lightningPercent;//生成雷电相对于雷电伤害的比率
    public float thunderMoveSpeed;//生成雷电移动速度

    public List<float> buffValue = new List<float>();//临时存储buff的数据
    [Header("Base")]
    public int currentHp;//当前血量  不能直接被修改
    public bool isVulnerable;//是否处于脆弱状态
    public float vulnerablePercent;//脆弱状态下受到的伤害倍率
    public Action updateHP;//更新血量的事件

    public bool isDead { get; private set; }
    EntityFX fx;
    float buffTimer;
    float fireDamageTimer;

    protected virtual void Start()
    {
        currentHp = GetMaxHP();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        buffTimer -= Time.deltaTime;
        fireDamageTimer -= Time.deltaTime;
        if (buffTimer < 0)
        {
            buff = Buff.None;
            buffValue.Clear();
        }
        if (fireDamageTimer < 0 && buff == Buff.Fire)
        {
            CheckFireDamage(buffValue);
        }
    }

    //对目标造成物理伤害的计算
    public virtual void DoDamage(CharacterStats target, float damagePercent = 0)
    {
        //先计算闪避  
        if (CheckEvasion(target))
        {
            return;
        }
        int total = damage.GetValue() + strength.GetValue();
        //计算暴击
        if (CheckCritChance())
        {
            total = CheckCritPower(total);
        }
        //计算护甲
        total = CheckArmor(target, total);
        //计算伤害加成
        if (damagePercent > 0)
        {
            //如果有百分比伤害加成
            total = Mathf.RoundToInt(total * damagePercent);
        }
        //结算
        target.TakeDamage(total, transform);
    }

    //自身收到伤害的计算
    public virtual void TakeDamage(int damage, Transform target)
    {
        if (isVulnerable)
        {
            //脆弱状态下受到的伤害增加
            damage = Mathf.RoundToInt(damage * vulnerablePercent);
        }
        currentHp -= damage;
        if (updateHP != null)
            updateHP();
        if (currentHp <= 0 && !isDead)
        {
            Die();
        }
    }

    //闪避判断
    public bool CheckEvasion(CharacterStats stats)
    {
        int total = stats.agility.GetValue() + stats.evasion.GetValue();
        if (buff == Buff.Lightning)
        {
            //雷电状态会减少命中率  即增加对方的闪避率
            total += Mathf.RoundToInt(lightningChance * 100);
        }

        if (UnityEngine.Random.Range(0, 100) < total)
        {
            //在习得技能后闪避的效果
            stats.Evasion(this);
            return true;
        }
        return false;
    }

    //闪避效果函数
    protected virtual void Evasion(CharacterStats stats)
    {

    }

    //护甲计算
    int CheckArmor(CharacterStats stats, int total)
    {
        if (buff == Buff.Ice)
        {
            //冰冻状态需要削减护甲
            total -= Mathf.RoundToInt(stats.armor.GetValue() * iceArmor);
        }
        else
        {
            total -= stats.armor.GetValue();
        }
        //需要限制
        total = Mathf.Clamp(total, 0, int.MaxValue);
        return total;
    }

    //暴击判断
    public bool CheckCritChance()
    {
        int chance = critChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) < chance)
        {
            return true;
        }
        return false;
    }

    //暴击伤害
    int CheckCritPower(int total)
    {
        float chance = 1 + (critPower.GetValue() + strength.GetValue()) * 0.01f;//转为百分比
        float power = total * chance;
        return Mathf.RoundToInt(power);
    }

    //对目标造成魔法伤害的计算  魔法伤害取3个值中最高的
    public void DoMagicDamage(CharacterStats target)
    {
        //数值计算
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();
        int max = Mathf.Max(_fireDamage, _iceDamage, _lightningDamage);
        int total = max + intelligence.GetValue();
        total = CheckResistance(target, total);
        target.TakeDamage(total, transform);
        //状态判断  更改敌人状态
        Buff temp = target.buff;
        if (max > 0 && max == _fireDamage)
        {
            target.ApplyBuff(Buff.Fire, fireTime);
            target.buffValue.Add(fireDamage.GetValue());
            target.buffValue.Add(fireCoolDown);
            target.buffValue.Add(firePercent);
            //已经处于相同状态下时不再产生效果
            if (temp == Buff.Fire) return;
            target.fx.FireBlind(fireTime);
        }
        else if (max > 0 && max == _iceDamage)
        {

            target.ApplyBuff(Buff.Ice, iceTime);
            //已经处于相同状态下时不再产生效果
            if (temp == Buff.Ice) return;
            target.fx.IceBlind(iceTime, icePercent);
        }
        else if (max > 0 && max == _lightningDamage)
        {
            CheckLightning(target);
            target.ApplyBuff(Buff.Lightning, lightningTime);
            //已经处于相同状态下时不再产生效果
            if (temp == Buff.Lightning) return;
            target.fx.lightningBlind(lightningTime);
        }
    }

    int CheckResistance(CharacterStats stats, int total)
    {
        total -= stats.magicResistance.GetValue() + stats.intelligence.GetValue();
        total = Mathf.Clamp(total, 0, int.MaxValue);
        return total;
    }

    //time为状态持续时间 
    public void ApplyBuff(Buff _buff, float time)
    {
        //只会同时处于一种buff下
        if (buff != Buff.None) return;
        buff = _buff;
        buffTimer = time;
    }

    //每一种buff可在此处单独进行处理
    void CheckFireDamage(List<float> _value)
    {
        fireDamageTimer = _value[1];
        currentHp -= Mathf.RoundToInt(_value[0] * _value[2]);
        if (updateHP != null)
            updateHP();
        if (currentHp <= 0 && !isDead)
        {
            Die();
        }
    }

    //雷电状态下效果
    void CheckLightning(CharacterStats target)
    {
        //效果对玩家不生效 且 第一次进状态并不生效
        if (target.GetComponent<Player>() != null || target.buff == Buff.None) return;
        //从目标位置生成
        GameObject thunder = Instantiate(thunderPrefab, target.transform.position, Quaternion.identity);
        thunder.GetComponent<ThunderController>().SetUpData(target, Mathf.RoundToInt(lightningDamage.GetValue()
            * lightningPercent), thunderMoveSpeed);
    }

    //对角色的治疗
    public void Heal(int value)
    {
        currentHp += value;
        currentHp = Mathf.Clamp(currentHp, 0, GetMaxHP());
        if (updateHP != null)
            updateHP();
    }

    //暂时修改数值的buff效果
    public void TempModifier(int change, float during, Stats stats)
    {
        if (stats == null) return;
        StartCoroutine(Modifier(change, during, stats));
    }

    IEnumerator Modifier(int change, float during, Stats stats)
    {
        stats.AddModifier(change);
        yield return new WaitForSeconds(during);
        stats.RemoveModifier(change);
    }

    //脆弱状态 时间和脆弱倍率
    public void SetVulnerable(float time, float percent)
    {
        vulnerablePercent = percent;
        StartCoroutine(Vulnerable(time));
    }
    IEnumerator Vulnerable(float time)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(time);
        isVulnerable = false;
        vulnerablePercent = 1;
    }

    //根据值类型得到值
    public Stats GetStatsType(StatsType _type)
    {
        if (_type == StatsType.Strength)
        {
            return strength;
        }
        else if (_type == StatsType.Agility)
        {
            return agility;
        }
        else if (_type == StatsType.Intelligence)
        {
            return intelligence;
        }
        else if (_type == StatsType.Vitality)
        {
            return vitality;
        }
        else if (_type == StatsType.Damage)
        {
            return damage;
        }
        else if (_type == StatsType.CritPower)
        {
            return critPower;
        }
        else if (_type == StatsType.CritChance)
        {
            return critChance;
        }
        else if (_type == StatsType.Health)
        {
            return maxHp;
        }
        else if (_type == StatsType.Armor)
        {
            return armor;
        }
        else if (_type == StatsType.Evasion)
        {
            return evasion;
        }
        else if (_type == StatsType.MagicResist)
        {
            return magicResistance;
        }
        else if (_type == StatsType.FireDamage)
        {
            return fireDamage;
        }
        else if (_type == StatsType.IceDamage)
        {
            return iceDamage;
        }
        else if (_type == StatsType.LightningDamage)
        {
            return lightningDamage;
        }

        return null;
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public int GetMaxHP()
    {
        return maxHp.GetValue() + vitality.GetValue();
    }
}
