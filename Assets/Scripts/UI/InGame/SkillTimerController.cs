using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TimerType
{
    Parry,
    Dash,
    Crystal,
    Sword,
    BlackHole
}
public class SkillTimerController : MonoBehaviour
{
    [SerializeField] Image timerImage;
    [SerializeField] List<Sprite> skillSprites = new List<Sprite>();
    [SerializeField] TimerType timerType;

    void Update()
    {
        InitTimer();
    }

    void InitTimer()
    {
        if(timerType == TimerType.Parry)
        {
            UnlockParry();
        }
        else if (timerType == TimerType.Dash)
        {
            UnlockDash();
        }
        else if (timerType == TimerType.Crystal)
        {
            UnlockCrystal();
        }
        else if (timerType == TimerType.Sword)
        {
            UnlockSword();
        }
        else if (timerType == TimerType.BlackHole)
        {
            UnlockBlackHole();
        }
    }

    void UnlockParry(){
        if(SkillManager.Instance.parry.parryCloneUnlock){
            UpdateTimer(skillSprites[3]);
        }
        else if(SkillManager.Instance.parry.parryHealUnlock){
            UpdateTimer(skillSprites[2]);
        }
        else if(SkillManager.Instance.parry.parryUnlock){
            UpdateTimer(skillSprites[1]);
        }
        else{
            UpdateTimer(skillSprites[0]);
        }
    }

    void UnlockDash()
    {
        if (SkillManager.Instance.dash.dashEndUnlock)
        {
            UpdateTimer(skillSprites[3]);
        }
        else if (SkillManager.Instance.dash.dashStartUnlock)
        {
            UpdateTimer(skillSprites[2]);
        }
        else if (SkillManager.Instance.dash.dashUnlock)
        {
            UpdateTimer(skillSprites[1]);
        }
        else
        {
            UpdateTimer(skillSprites[0]);
        }
    }

    void UnlockCrystal()
    {
        if (SkillManager.Instance.crystal.canCreateClone)
        {
            UpdateTimer(skillSprites[4]);
        }
        else if(SkillManager.Instance.crystal.canMultiple){
            UpdateTimer(skillSprites[3]);
        }
        else if (SkillManager.Instance.crystal.canMove)
        {
            UpdateTimer(skillSprites[2]);
        }
        else if (SkillManager.Instance.crystal.canExplore)
        {
            UpdateTimer(skillSprites[1]);
        }
        else
        {
            UpdateTimer(skillSprites[0]);
        }
    }

    void UnlockSword()
    {
        if (SkillManager.Instance.sword.sowrdType == SwordSkill.Spin)
        {
            UpdateTimer(skillSprites[4]);
        }
        else if (SkillManager.Instance.sword.sowrdType == SwordSkill.Pierce)
        {
            UpdateTimer(skillSprites[3]);
        }
        else if (SkillManager.Instance.sword.sowrdType == SwordSkill.Bounce)
        {
            UpdateTimer(skillSprites[2]);
        }
        else if (SkillManager.Instance.sword.sowrdType == SwordSkill.Regular)
        {
            UpdateTimer(skillSprites[1]);
        }
        else
        {
            UpdateTimer(skillSprites[0]);
        }
    }

    void UnlockBlackHole()
    {
        if (SkillManager.Instance.blackHole.blackHoleUnlock)
        {
            UpdateTimer(skillSprites[1]);
        }
        else
        {
            UpdateTimer(skillSprites[0]);
        }
    }


    void UpdateTimer(Sprite sprite){
        //避免多次刷新
        if(timerImage.sprite == sprite){
            return;
        }
        timerImage.sprite = sprite;
    }
}
