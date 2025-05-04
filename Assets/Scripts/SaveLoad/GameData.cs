using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//序列化数据
[Serializable]
public class GameData
{
    public int currency;//存储玩家钱币
    public DataDictionary<string, int> itemInventroy;//存储玩家持有的道具
    public DataDictionary<string, bool> skills;//存储一解锁的技能
    public List<string> equipments;//存储玩家身上的装备

    public DataDictionary<string,bool> checkPoints;//已激活的检查点
    public string checkPointID;//玩家到达的最远检查点  再次开始游戏时会在此重生

    public int lostCurrency;
    public float lostPosX;
    public float LostPosY;
    public GameData()
    {
        currency = 0;
        itemInventroy = new DataDictionary<string, int>();
        skills = new DataDictionary<string, bool>();
        equipments = new List<string>();

        checkPoints = new DataDictionary<string, bool>();
        checkPointID = "";
    }
}
