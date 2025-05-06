using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClone : PlayerSkill
{
    [Header("Clone")]
    public SkillSlotController cloneSlot;
    public bool cloneUnlock { get; private set; }//克隆体技能是否解锁
    [Range(0, 1)]
    [SerializeField] float defaultDamagePercent;//默认克隆体伤害百分比 
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuring;//克隆体显示时间
    [SerializeField] float cloneDisappearSpeed;//克隆体消失速度
    [SerializeField] float cloneAttackRadius;//克隆体攻击半径
    [SerializeField] float cloneCheckRadius;//克隆体检查半径
    [SerializeField] float cloneDelayTime;//克隆体延迟消失时间
    [SerializeField] bool canAttack;//暂时用以标记克隆体是否能够攻击

    [Header("CloneAggressive")]
    public SkillSlotController cloneAggressiveSlot;
    public bool cloneAggressiveUnlock { get; private set; }//克隆体攻击力技能是否解锁
    [Range(0, 1)]
    [SerializeField] float aggressivePercent;//克隆体攻击力百分比

    [Header("CloneEffect")]
    public SkillSlotController cloneEffectSlot;
    public bool cloneEffectUnlock { get; private set; }//克隆体技能效果是否解锁

    [Header("CloneDuplicate")]
    public SkillSlotController cloneDuplicateSlot;
    public bool cloneDuplicateUnlock { get; private set; }//克隆体复制技能是否解锁 
    [SerializeField] bool canDuplicate;//在克隆体攻击有概率再次创建克隆体攻击
    [Range(0, 100)]
    [SerializeField] float duplicateChance;//复制概率  取值：0&-100%

    float cloneDamagePercent;//克隆体伤害百分比

    void OnEnable()
    {
        skillTreeSlots.Add(cloneSlot);
        skillTreeSlots.Add(cloneAggressiveSlot);
        skillTreeSlots.Add(cloneEffectSlot);
        skillTreeSlots.Add(cloneDuplicateSlot);
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void CheckUnlock()
    {
        if (cloneSlot.unlock)
        {
            cloneUnlock = true;
            cloneDamagePercent = defaultDamagePercent;
        }
        if (cloneAggressiveSlot.unlock)
        {
            cloneAggressiveUnlock = true;
            cloneDamagePercent = aggressivePercent;
        }
        if (cloneEffectSlot.unlock)
        {
            cloneEffectUnlock = true;
            cloneDamagePercent = defaultDamagePercent;
        }
        if (cloneDuplicateSlot.unlock)
        {
            cloneDuplicateUnlock = true;
            canDuplicate = true;
            cloneDamagePercent = defaultDamagePercent;
        }
    }

    public void AddUnlockListener()
    {
        cloneSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (cloneSlot.unlock)
            {
                cloneUnlock = true;
                cloneDamagePercent = defaultDamagePercent;
            }
        });
        cloneAggressiveSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (cloneAggressiveSlot.unlock)
            {
                cloneAggressiveUnlock = true;
                cloneDamagePercent = aggressivePercent;
            }
        });
        cloneEffectSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (cloneEffectSlot.unlock)
            {
                cloneEffectUnlock = true;
                cloneDamagePercent = defaultDamagePercent;
            }
        });
        cloneDuplicateSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (cloneDuplicateSlot.unlock)
            {
                cloneDuplicateUnlock = true;
                canDuplicate = true;
                cloneDamagePercent = defaultDamagePercent;
            }
        });
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    //偏移量默认设置为0
    public void CreateClone(Transform target, Vector3 offset = new Vector3())
    {
        //此处创建克隆体时需要使用克隆体本身的朝向
        GameObject playerClone = Instantiate(clonePrefab, target.position + offset, Quaternion.identity);
        playerClone.GetComponent<CloneController>().SetUpData(cloneDuring, cloneDisappearSpeed, canAttack
            , cloneAttackRadius, cloneCheckRadius, canDuplicate, duplicateChance, cloneDamagePercent);
    }

    //克隆体效果
    public void CloneEffect(Transform target)
    {
        //携带装备效果
        InventoryManager.instance.UseEquipmentEffect(EquipmentType.Weapon, target);
    }

    public override void SkillBelock()
    {
        base.SkillBelock();
        cloneUnlock = false;
        cloneAggressiveUnlock = false;
        cloneEffectUnlock = false;
        cloneDuplicateUnlock = false;
    }
}
