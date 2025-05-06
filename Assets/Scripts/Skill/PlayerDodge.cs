using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDodge : PlayerSkill
{
    [Header("Dodge")]
    public SkillSlotController dodgeSlot;
    public bool dodgeUnlock { get; private set; }//闪避技能是否解锁
    [SerializeField] int dodgeChance;//增加的闪避率  
    [Header("DodgeClone")]
    public SkillSlotController dodgeCloneSlot;
    public bool dodgeCloneUnlock { get; private set; }//闪避技能是否解锁
    [SerializeField] float dodgeOffsetX;//闪避克隆的偏移量

    void OnEnable()
    {
        skillTreeSlots.Add(dodgeSlot);
        skillTreeSlots.Add(dodgeCloneSlot);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void CheckUnlock()
    {
        if (dodgeSlot.unlock && dodgeUnlock == false)
        {
            dodgeUnlock = true;
            PlayerManager.Instance.player.stats.evasion.AddModifier(dodgeChance);
            InventoryManager.instance.UpdateStatsUI();
        }
        if (dodgeCloneSlot.unlock)
        {
            dodgeCloneUnlock = true;
        }
    }

    public void AddUnlockListener()
    {
        dodgeSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (dodgeSlot.unlock && dodgeUnlock == false)
            {
                dodgeUnlock = true;
                PlayerManager.Instance.player.stats.evasion.AddModifier(dodgeChance);
                InventoryManager.instance.UpdateStatsUI();
            }
        });
        dodgeCloneSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (dodgeCloneSlot.unlock)
            {
                dodgeCloneUnlock = true;
            }
        });
    }

    public override void SkillBelock()
    {
        base.SkillBelock();
        dodgeUnlock = false;
        dodgeCloneUnlock = false;
    }

    public void CreateEvasionClone(Transform target, int faceDir)
    {
        if (dodgeCloneUnlock)
        {
            //在敌人背后进行克隆体攻击
            SkillManager.Instance.clone.CreateClone(target, new Vector3(dodgeOffsetX * -faceDir, 0));
        }
    }
}
