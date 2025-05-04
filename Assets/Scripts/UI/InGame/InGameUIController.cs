using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    [SerializeField] Image parryTimer;
    [SerializeField] Image dashTimer;
    [SerializeField] Image crystalTimer;
    [SerializeField] Image swordTimer;
    [SerializeField] Image blackHoleTimer;
    [SerializeField] TextMeshProUGUI currencyText;
    [SerializeField] float changeSpeed;//钱币变化速度
    int lostAmount;

    void Start()
    {
        PlayerManager.Instance.updateCurrency += UpdateCurrencyUI;
    }

    void Update()
    {
        UpdateAllUI();
        UpdateCurrencyUI();
    }

    //游戏金币变化效果
    void UpdateCurrencyUI()
    {
        if (lostAmount < PlayerManager.Instance.currency)
        {
            int change = Mathf.RoundToInt(Time.deltaTime * changeSpeed);
            lostAmount += change;
            currencyText.text = lostAmount.ToString("N0");
        }
        else
        {
            currencyText.text = PlayerManager.Instance.currency.ToString("N0");
        }
    }

    void UpdateAllUI()
    {
        if (SkillManager.Instance.parry.parryUnlock && Input.GetKeyDown(KeyCode.F))
        {
            SkillCD(parryTimer);
        }
        if (SkillManager.Instance.dash.dashUnlock && Input.GetMouseButtonDown(1))
        {
            SkillCD(dashTimer);
        }
        if (SkillManager.Instance.crystal.crystalUnlock && Input.GetKeyDown(KeyCode.C))
        {
            SkillCD(crystalTimer);
        }
        if (SkillManager.Instance.sword.swordUnlock && Input.GetKeyDown(KeyCode.Q))
        {
            SkillCD(swordTimer);
        }
        if (SkillManager.Instance.blackHole.blackHoleUnlock && Input.GetKeyDown(KeyCode.Space))
        {
            SkillCD(blackHoleTimer);
        }
        UpdateTimer(parryTimer, SkillManager.Instance.parry.GetCD());
        UpdateTimer(dashTimer, SkillManager.Instance.dash.GetCD());
        UpdateTimer(crystalTimer, SkillManager.Instance.crystal.GetCD());
        UpdateTimer(swordTimer, SkillManager.Instance.sword.GetCD());
        UpdateTimer(blackHoleTimer, SkillManager.Instance.blackHole.GetCD());
    }

    //刷新ui
    void UpdateTimer(Image image, float cd)
    {
        if (image.fillAmount > 0)
        {
            image.fillAmount -= Time.deltaTime / cd;
        }
    }

    void SkillCD(Image image)
    {
        if (image.fillAmount > 0)
        {
            return;
        }
        image.fillAmount = 1;
    }

    //根据技能解锁情况显示ui
    void InitTimer()
    {
        if(!SkillManager.Instance.parry.parryUnlock){
            
        }
    }
}
