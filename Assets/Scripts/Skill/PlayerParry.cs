using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerParry : PlayerSkill
{
    [Header("Parry")]
    public SkillSlotController parrySlot;
    public bool parryUnlock { get; private set; }//反击技能是否解锁
    [Header("ParryHeal")]
    public SkillSlotController parryHealSlot;
    public bool parryHealUnlock { get; private set; }//反击治疗技能是否解锁
    [Range(0, 1)]
    public float parryHealValue;//反击治疗技能的治疗值  百分比
    [Header("ParryClone")]
    public SkillSlotController parryCloneSlot;
    [SerializeField] float cloneDelayTime;//反击成功后克隆体延迟创建时间
    [SerializeField] float cloneAttackOffset;//反击成功时克隆体的位置偏移
    public bool parryCloneUnlock { get; private set; }//反击制造克隆体技能是否解锁
    void OnEnable()
    {
        skillTreeSlots.Add(parrySlot);
        skillTreeSlots.Add(parryHealSlot);
        skillTreeSlots.Add(parryCloneSlot);
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (parryHealUnlock)
        {
            int healValue = Mathf.RoundToInt(PlayerManager.Instance.player.stats.GetMaxHP() * parryHealValue);
            PlayerManager.Instance.player.stats.Heal(healValue);
        }
    }

    protected override void CheckUnlock()
    {
        if (parrySlot.unlock)
        {
            parryUnlock = true;
        }
        if (parryHealSlot.unlock)
        {
            parryHealUnlock = true;
        }
        if (parryCloneSlot.unlock)
        {
            parryCloneUnlock = true;
        }
    }

    public void AddUnlockListener()
    {
        parrySlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (parrySlot.unlock)
            {
                parryUnlock = true;
            }
        });
        parryHealSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (parryHealSlot.unlock)
            {
                parryHealUnlock = true;
            }
        });
        parryCloneSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (parryCloneSlot.unlock)
            {
                parryCloneUnlock = true;
            }
        });
    }

    public override void SkillBelock()
    {
        base.SkillBelock();
        parryUnlock = false;
        parryHealUnlock = false;
        parryCloneUnlock = false;
    }


    public void ParryCreateClone(Transform enemy)
    {
        StartCoroutine(DelayCreate(enemy));
    }

    IEnumerator DelayCreate(Transform enemy)
    {
        yield return new WaitForSeconds(cloneDelayTime);
        SkillManager.Instance.clone.CreateClone(enemy, new Vector3(cloneAttackOffset * player.faceDir, 0));
    }
}
