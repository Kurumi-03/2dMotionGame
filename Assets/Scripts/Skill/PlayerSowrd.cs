using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SwordSkill
{
    None,
    Regular,//常规扔出
    Bounce,//反弹
    Pierce,//刺穿
    Spin,//自旋
}

public class PlayerSowrd : PlayerSkill
{
    public List<GameObject> playerSwords = new List<GameObject>();//存储玩家发射出的剑
    public SwordSkill sowrdType = SwordSkill.None;
    public bool canThrow = false;
    [Header("SwordRegular")]
    public SkillSlotController swordSlot;//技能槽
    public bool swordUnlock { get; private set; }//是否解锁剑技能
    [SerializeField] GameObject sowrdPrefab;//剑的预制体
    [SerializeField] float swordTime;//剑能够存在的时间
    [SerializeField] Vector2 throwForce;//投掷力度大小
    [SerializeField] float gravity;//重力大小
    [SerializeField] float freezeTime;//剑能够冻结敌人时间
    [SerializeField] int swordNum;//玩家可以使用的剑数量
    [SerializeField] float offset;//生成剑与玩家之间的距离偏移
    [SerializeField] float checkDistance;//检测剑与玩家之间距离
    [SerializeField] float swordSpeed;//剑飞回的速度
    [SerializeField] float returnForce;//剑飞回时对玩家的作用力力度

    [Header("AimDots")]
    [SerializeField] GameObject dotPrefab;//轨迹点预制体
    [SerializeField] Transform dotParent;//轨迹点的父物体
    [SerializeField] int dotNum;//轨迹点数量
    [SerializeField] float dotSpace;//轨迹点之间的间隔

    [Header("Bounce")]
    public SkillSlotController swordBounceSlot;//技能槽
    [SerializeField] float bounceSpeed;//反弹速度
    [SerializeField] float checkRadius;//剑对敌人的检测圈半径
    [SerializeField] int bounceNum;//反弹次数
    [SerializeField] float enemyDistance;//剑与敌人之间的检测距离

    [Header("Pierce")]
    public SkillSlotController swordPierceSlot;//技能槽
    [SerializeField] int pieceNum;//穿刺次数
    [SerializeField] float pierceGravity;//穿刺时重力

    [Header("Spin")]
    public SkillSlotController swordSpinSlot;//技能槽
    [SerializeField] float moveDiatance;//剑能够离开的最大距离
    [SerializeField] float spinTime;//自旋能够持续的时间
    [SerializeField] float hitCoolDown;//剑旋转时的攻击cd
    [SerializeField] float damageRadius;//伤害范围
    [SerializeField] float spinGravity;//自旋技能的重力

    [Header("SwordEffect")]
    public SkillSlotController swordFreezeSlot;//技能槽
    public bool swordFreezeUnlock { get; private set; }//是否解锁冻结技能
    public SkillSlotController sowrdVulnerabilitySlot;
    public bool swordVulnerabilityUnlock { get; private set; }//是否解锁脆弱技能
    [SerializeField] float vulnerabilityTime;//脆弱技能持续时间
    [Range(1, 10)]
    [SerializeField] float vulnerabilityPercent;//脆弱倍率

    public bool isBouncing;//是否可以反弹   需要点技能树
    public bool isPiercing;//是否可以穿刺   需要点技能树
    public bool isSpining;//是否可以自旋   需要点技能树
    private GameObject[] dots;//存储轨迹点
    float tempGravity;

    void OnEnable()
    {
        skillTreeSlots.Add(swordSlot);
        skillTreeSlots.Add(swordBounceSlot);
        skillTreeSlots.Add(swordPierceSlot);
        skillTreeSlots.Add(swordSpinSlot);
        skillTreeSlots.Add(swordFreezeSlot);
        skillTreeSlots.Add(sowrdVulnerabilitySlot);
    }
    protected override void Start()
    {
        base.Start();
        //初始需要创建轨迹点
        CreateDots();
    }

    protected override void CheckUnlock()
    {
        if (swordSlot.unlock)
        {
            swordUnlock = true;
            canThrow = true;
            sowrdType = SwordSkill.Regular;
        }
        if (swordBounceSlot.unlock)
        {
            sowrdType = SwordSkill.Bounce;
        }
        if (swordPierceSlot.unlock)
        {
            sowrdType = SwordSkill.Pierce;
        }
        if (swordSpinSlot.unlock)
        {
            sowrdType = SwordSkill.Spin;
        }
        if (swordFreezeSlot.unlock)
        {
            swordFreezeUnlock = true;
        }
        if (sowrdVulnerabilitySlot.unlock)
        {
            swordVulnerabilityUnlock = true;
        }
    }

    public void AddUnlockListener()
    {
        swordSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (swordSlot.unlock)
            {
                swordUnlock = true;
                canThrow = true;
                sowrdType = SwordSkill.Regular;
            }
        });
        swordBounceSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (swordBounceSlot.unlock)
            {
                sowrdType = SwordSkill.Bounce;
            }
        });
        swordPierceSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (swordPierceSlot.unlock)
            {
                sowrdType = SwordSkill.Pierce;
            }
        });
        swordSpinSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (swordSpinSlot.unlock)
            {
                sowrdType = SwordSkill.Spin;
            }
        });
        swordFreezeSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (swordFreezeSlot.unlock)
            {
                swordFreezeUnlock = true;
            }
        });
        sowrdVulnerabilitySlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (sowrdVulnerabilitySlot.unlock)
            {
                swordVulnerabilityUnlock = true;
            }
        });

    }

    protected override void Update()
    {
        base.Update();
        //技能切换
        switch (sowrdType)
        {
            case SwordSkill.Regular:
                isBouncing = false;
                isPiercing = false;
                isSpining = false;
                tempGravity = gravity;
                break;
            case SwordSkill.Bounce:
                isBouncing = true;
                isPiercing = false;
                isSpining = false;
                tempGravity = gravity;
                break;
            case SwordSkill.Pierce:
                isBouncing = false;
                isPiercing = true;
                isSpining = false;
                tempGravity = pierceGravity;
                break;
            case SwordSkill.Spin:
                isBouncing = false;
                isPiercing = false;
                isSpining = true;
                tempGravity = gravity;
                break;
        }
    }

    public void CreateSword()
    {
        GameObject sword = Instantiate(sowrdPrefab);
        //生成位置向面朝方向进行偏移 避免剑与玩家碰撞
        sword.transform.position = new Vector2(player.transform.position.x + player.faceDir * offset, player.transform.position.y);
        //投掷速度为方向归一化乘以力度
        Vector2 throwVelocity = new Vector2(SetUpDir().normalized.x * throwForce.x, SetUpDir().normalized.y * throwForce.y);

        //设置剑的常态数据
        sword.GetComponent<SwordController>().SetUpSword(throwVelocity, gravity, swordSpeed, checkDistance, freezeTime);
        //设置剑的反弹数据
        sword.GetComponent<SwordController>().SetUpBoundData(isBouncing, bounceSpeed, checkRadius, enemyDistance, bounceNum);
        //设置剑的穿刺数据
        sword.GetComponent<SwordController>().SetUpPierceData(isPiercing, pierceGravity, pieceNum);
        //设置剑的自旋数据
        sword.GetComponent<SwordController>().SetUpSpinData(isSpining, moveDiatance, hitCoolDown, spinTime, damageRadius);
        playerSwords.Add(sword);
        //根据当前剑的数量确认是否可以扔剑
        if (playerSwords.Count == swordNum)
        {
            canThrow = false;
        }
        //避免剑飞的太远
        Invoke("DestroySword", swordTime);
    }

    private Vector2 SetUpDir()
    {
        //将鼠标的屏幕位置转为世界坐标获取
        Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerDir = player.transform.position;
        Vector2 targetDir = mouseDir - playerDir;
        return targetDir;
    }

    public void SetDotVisible(bool isVisible)
    {
        foreach (GameObject item in dots)
        {
            item.SetActive(isVisible);
        }
    }

    private void CreateDots()
    {
        dots = new GameObject[dotNum];
        for (int i = 0; i < dotNum; i++)
        {
            Vector2 pos = new Vector2(player.transform.position.x + player.faceDir * offset, player.transform.position.y);
            GameObject dot = Instantiate(dotPrefab, pos, Quaternion.identity, dotParent);
            dots[i] = dot;
        }
        SetDotVisible(false);
    }

    //设置轨迹线中所有点的位置
    public void SetPos()
    {
        for (int i = 0; i < dotNum; i++)
        {
            //此处的space相当于时间间隔
            float space = i * dotSpace;
            //此处为抛物线速度公式 x = v * t + 1/2 * a * t * t;
            Vector2 targetPos = new Vector2(SetUpDir().normalized.x * throwForce.x,
                SetUpDir().normalized.y * throwForce.y) * space + 0.5f * (Physics2D.gravity * tempGravity) * (space * space);
            //轨迹线需要和玩家统一朝向
            targetPos.x *= player.faceDir;
            dots[i].transform.localPosition = targetPos;
        }
    }

    public bool CanReturnSword()
    {
        if (playerSwords.Count == 0)
        {
            return false;
        }
        playerSwords[0].GetComponent<SwordController>().ReturnSword();
        //返回时不销毁剑
        CancelInvoke("DestroySword");
        return false;
    }

    private void DestroySword()
    {
        if (playerSwords.Count != 0)
        {
            Destroy(playerSwords[0].gameObject);
            playerSwords.RemoveAt(0);
            canThrow = true;
        }
    }

    public float SetReturnForce()
    {
        return returnForce;
    }

    //剑的脆弱效果
    public void SetVulnerabilityEffect(CharacterStats enemy)
    {
        enemy.SetVulnerable(vulnerabilityTime, vulnerabilityPercent);
    }

    public override void SkillBelock()
    {
        base.SkillBelock();
        swordUnlock = false;
        canThrow = false;
        sowrdType = SwordSkill.None;
        swordFreezeUnlock = false;
        swordVulnerabilityUnlock = false;
    }
}
