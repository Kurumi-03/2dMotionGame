
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,//武器
    Armor,//盔甲
    Amulet,//护身符
    Flask,//药水
}
[CreateAssetMenu(fileName = "EquipmentData", menuName = "Item/EquipmentData")]
public class EquipmentData : ItemData
{
    public EquipmentType equipmentType;
    public List<ItemEffect> effects = new List<ItemEffect>();//装备效果列表
    public float effectCoolDown;//装备效果时间

    [Header("Major")]
    public int strength;//提升伤害加成和暴击伤害倍率
    public int agility;//闪避率和暴击率
    public int intelligence;//魔法攻击和魔法抗性
    public int vitality;//血量上限

    [Header("Offensive")]
    public int damage;//基础伤害
    public int critPower;//基础暴击伤害倍率
    public int critChance;//基础暴击率

    [Header("Defensive")]
    public int maxHp;//角色基础满状态血量
    public int armor;//基础护甲
    public int evasion;//基础闪避率
    public int magicResistance;//魔法抗性

    [Header("Magic Major")]
    public int fireDamage;//火焰伤害
    public int iceDamage;//冰冻伤害
    public int lightningDamage;//雷电伤害

    [Header("Craft")]
    public List<InventoryItem> materials = new List<InventoryItem>();//合成材料列表

    public void UseEffect(Transform target = null)
    {
        foreach (ItemEffect effect in effects)
        {
            effect.Effect(target);
        }
    }

    public void AddModifier()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
        playerStats.damage.AddModifier(damage);
        playerStats.critPower.AddModifier(critPower);
        playerStats.critChance.AddModifier(critChance);
        playerStats.maxHp.AddModifier(maxHp);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
    }

    public void RemoveModifier()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
        playerStats.damage.RemoveModifier(damage);
        playerStats.critPower.RemoveModifier(critPower);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.maxHp.RemoveModifier(maxHp);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);
        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
    }

    public override string GetDescription()
    {
        base.GetDescription();
        //初始将描述置空
        des.Length = 0;
        rawCount = 0;

        GetString(strength, StatsType.Strength.ToString());
        GetString(agility, StatsType.Agility.ToString());
        GetString(intelligence, StatsType.Intelligence.ToString());
        GetString(vitality, StatsType.Vitality.ToString());
        GetString(damage, StatsType.Damage.ToString());
        GetString(critPower, StatsType.CritPower.ToString());
        GetString(critChance, StatsType.CritChance.ToString());
        GetString(maxHp, StatsType.Health.ToString());
        GetString(armor, StatsType.Armor.ToString());
        GetString(evasion, StatsType.Evasion.ToString());
        GetString(magicResistance, StatsType.MagicResist.ToString());
        GetString(fireDamage, StatsType.FireDamage.ToString());
        GetString(iceDamage, StatsType.IceDamage.ToString());
        GetString(lightningDamage, StatsType.LightningDamage.ToString());

        //添加装备效果的描述
        if (effects.Count != 0)
        {
            des.AppendLine();//空行间隔
            for (int i = 0; i < effects.Count; i++)
            {
                des.AppendLine("Unqiue: " + effects[i].effectDescription);
                des.AppendLine();
            }
        }

        return des.ToString();
    }

    int rawCount;//描述的行数记录
    void GetString(int value, string name)
    {
        //只有数值不为0的属性才会显示
        if (value != 0)
        {
            des.AppendLine(" " + (value > 0 ? "+" : "-") + value.ToString() + " " + name);
            rawCount++;
        }
    }
}
