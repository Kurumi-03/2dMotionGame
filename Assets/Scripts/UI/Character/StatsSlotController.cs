using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsSlotController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] StatsType type;

    public void UpdateSlotValue()
    {
        if (PlayerManager.Instance.player.stats == null) return;
        PlayerStats stats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
        int value = stats.GetStatsType(type).GetValue();
        //特殊数值需要单独处理
        if (type == StatsType.Health)
        {
            value = stats.GetMaxHP();
        }
        else if (type == StatsType.Damage)
        {
            value = stats.damage.GetValue() + stats.strength.GetValue();
        }
        else if (type == StatsType.CritPower)
        {
            value = stats.critPower.GetValue() + stats.strength.GetValue();
        }
        else if (type == StatsType.CritChance)
        {
            value = stats.critChance.GetValue() + stats.agility.GetValue();
        }
        else if (type == StatsType.Evasion)
        {
            value = stats.evasion.GetValue() + stats.agility.GetValue();
        }
        else if (type == StatsType.MagicResist)
        {
            value = stats.magicResistance.GetValue() + stats.intelligence.GetValue();
        }
        nameText.text = type.ToString();
        valueText.text = value.ToString();
    }
}
