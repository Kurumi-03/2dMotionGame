using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,ISave
{
    public Player player;
    public int currency;//玩家货币
    public Action updateCurrency;//货币更新事件
    public static PlayerManager Instance;

    //单例模式
    private void GetInstance()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    void Awake()
    {
        GetInstance();
    }

    public bool CheckCurrency(int amount)
    {
        if (amount > currency)
        {
            GameManager.Instance.gameTip.ShowTip(GameManager.Instance.currencyTips[0], GameManager.Instance.currencyTips[1]);
            return false;
        }
        else
        {
            currency -= amount;
            if (updateCurrency != null)
            {
                updateCurrency();
            }
            return true;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.currency = currency;
    }

    public void LoadData(GameData data)
    {
        currency = data.currency;
    }
}
