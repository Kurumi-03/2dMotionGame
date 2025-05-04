using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : PlayerSkill
{
    [Header("Dash")]
    public SkillSlotController dashSlot;
    public bool dashUnlock { get; private set; }//冲刺技能是否解锁
    [Header("DashCloneOnStart")]
    public SkillSlotController dashOnStartSlot;
    public bool dashStartUnlock { get; private set; }//在冲刺技能开始时创建    
    [Header("DashCloneOnEnd")]
    public SkillSlotController dashOnEndSlot;
    public bool dashEndUnlock { get; private set; }//在冲刺技能结束时创建
    void OnEnable()
    {
        skillTreeSlots.Add(dashSlot);
        skillTreeSlots.Add(dashOnStartSlot);
        skillTreeSlots.Add(dashOnEndSlot);
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void CheckUnlock()
    {
        if (dashSlot.unlock)
        {
            dashUnlock = true;
        }
        if (dashOnStartSlot.unlock)
        {
            dashStartUnlock = true;
        }
        if (dashOnEndSlot.unlock)
        {
            dashEndUnlock = true;
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    public void AddUnlockListener()
    {
        dashSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (dashSlot.unlock)
            {
                dashUnlock = true;
            }
        });
        dashOnStartSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (dashOnStartSlot.unlock)
            {
                dashStartUnlock = true;
            }
        });
        dashOnEndSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (dashOnEndSlot.unlock)
            {
                dashEndUnlock = true;
            }
        });

    }

    public void CanCreateCloneStart()
    {
        if (dashStartUnlock)
        {
            SkillManager.Instance.clone.CreateClone(player.transform);
        }
    }

    public void CanCreateCloneEnd()
    {
        if (dashEndUnlock)
        {
            SkillManager.Instance.clone.CreateClone(player.transform);
        }
    }
}
