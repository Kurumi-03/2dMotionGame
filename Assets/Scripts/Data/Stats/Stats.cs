using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//封装一个属性类   用以标识血量和伤害的类型
public class Stats
{
    [SerializeField] int baseValue;//属性的基础值
    [SerializeField] List<int> modifierList = new List<int>();//装备 buff等增减或减少属性值的列表

    //通过方法得到属性的真正值
    public int GetValue()
    {
        int value = baseValue;
        foreach (int i in modifierList)
        {
            value += i;
        }
        return value;
    }

    public int GetBaseValue()
    {
        return baseValue;
    }

    //通过方法设置基础属性值
    public void SetBaseValue(int value)
    {
        baseValue = value;
    }

    //修改值
    public void AddModifier(int _modifier)
    {
        modifierList.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifierList.Remove(_modifier);
    }
}
