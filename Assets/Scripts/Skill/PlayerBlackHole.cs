using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlackHole : PlayerSkill
{
    public SkillSlotController blackHoleSlot;
    public bool blackHoleUnlock { get; private set; }//黑洞技能是否解锁
    [SerializeField] GameObject blackHolePrefab;//黑洞技能的预制体
    [SerializeField] float blackHoleDuring;//技能持续时间
    [SerializeField] float lastScale;//最终大小
    [SerializeField] float growSpeed;//增长速度
    [SerializeField] float shrinkSpeed;//收缩速度
    [SerializeField] float yOffset;//按键文本显示偏移
    [SerializeField] float xOffset;//克隆体生成位置相对于敌人的偏移
    [SerializeField] float attackCoolDown;//每个克隆体的攻击间隔
    [SerializeField] List<KeyCode> keyCodes = new List<KeyCode>();//存储可以使用的按键

    [SerializeField] float flyTime;//玩家飞起时间
    [SerializeField] float flyForce;//飞起的力度
    [SerializeField] float fallForce;//下降力度
    void OnEnable()
    {
        skillTreeSlots.Add(blackHoleSlot);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void CheckUnlock()
    {
        if (blackHoleSlot.unlock)
        {
            blackHoleUnlock = true;
        }
    }

    public void AddUnlockListener()
    {
        blackHoleSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (blackHoleSlot.unlock)
            {
                blackHoleUnlock = true;
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
        GameObject blackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        blackHole.GetComponent<BlackHoleController>().SetUpData(lastScale, growSpeed, shrinkSpeed,
            yOffset, xOffset, attackCoolDown, keyCodes, blackHoleDuring);
    }

    public override bool CanUseSkill()
    {
        if (cdTimer < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SkillData()
    {
        player.blackHoleState.SetUpData(flyTime, flyForce, fallForce);
    }

    public override void SkillBelock()
    {
        base.SkillBelock();
        blackHoleUnlock = false;
    }
}
