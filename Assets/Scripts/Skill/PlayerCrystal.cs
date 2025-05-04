using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCrystal : PlayerSkill
{
    [Header("Base")]
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDruing;//水晶持续存在时间
    public SkillSlotController crystalSlot;
    public bool crystalUnlock { get; private set; }//水晶技能是否解锁
    [Header("Explorsion")]
    public bool canExplore;//是否可以爆炸   技能树升级得到
    [SerializeField] float growScale;//爆炸时增长大小
    [SerializeField] float growSpeed;//增长速度
    public SkillSlotController crystalExplorsionSlot;
    [Header("Move")]
    public bool canMove;//是否可以向最近的敌人移动  技能树升级得到
    [SerializeField] float moveSpeed;//移动速度
    [SerializeField] float checkRadius;//检测敌人时的半径
    [SerializeField] float checkDistance;//靠近敌人的距离
    public SkillSlotController crystalMoveSlot;
    [Header("Multiple")]
    public bool canMultiple;//是否能够创建多个水晶   技能树升级得到  从水晶移动升级
    [SerializeField] float crystalNum;//可以使用的水晶数量
    [SerializeField] float multipleCoolDown;//在用完水晶后填充多个水晶的时间
    [SerializeField] float multipleWindow;//在使用一个水晶但没有使用完所有水晶后回复全部水晶所需时间  这个值要比cooldown小
    [SerializeField] List<GameObject> crystalList = new List<GameObject>();//存储多个水晶
    public SkillSlotController crystalMultipleSlot;
    [Header("Clone")]
    public bool canCreateClone;//在交换位置后在玩家原地创建一个克隆体  技能树升级得到
    public SkillSlotController crystalCloneSlot;
    GameObject currentCrystal;//当前创建的水晶
    Vector3 playerPos;//临时记录玩家位置
    Vector3 crystalPos;//临时记录水晶位置
    float multipleTimer;
    float multipleTime;
    void OnEnable()
    {
        skillTreeSlots.Add(crystalSlot);
        skillTreeSlots.Add(crystalExplorsionSlot);
        skillTreeSlots.Add(crystalMoveSlot);
        skillTreeSlots.Add(crystalMultipleSlot);
        skillTreeSlots.Add(crystalCloneSlot);
    }

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();
        multipleTimer -= Time.deltaTime;
    }

    public override void UseSkill()
    {
        //创建多个水晶使用另外的逻辑
        if (CanMultipleCrystal()) return;
        //创建单个水晶的逻辑  只在单个水晶中使用cd  移动暂时没有cd
        if (currentCrystal == null)
        {
            GameObject crystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            currentCrystal = crystal;
            crystal.GetComponent<CrystalController>().SetUpData(canExplore, crystalDruing, growScale,
                growSpeed, canMove, moveSpeed, checkDistance, CheckEnemy(currentCrystal.transform, checkRadius));
        }
        else
        {
            //移动时不可以爆炸
            if (canMove) return;
            //得到交换时的位置
            playerPos = player.transform.position;
            crystalPos = currentCrystal.transform.position;
            if (canExplore)
            {
                Explorsion();
                base.UseSkill();
            }
            else
            {
                //将玩家传送到水晶位置
                base.UseSkill();
                CreateClone();
                player.transform.position = crystalPos;
                Destroy(currentCrystal);
            }
        }
    }

    //水晶的爆炸能力
    void Explorsion()
    {
        //正在爆炸时不能交换
        if (currentCrystal.GetComponent<CrystalController>().Explorsion()) return;
        player.transform.position = crystalPos;
        currentCrystal.transform.position = playerPos;
    }

    void CreateClone()
    {
        //在玩家位置创建克隆体进行攻击
        if (canCreateClone)
        {
            SkillManager.Instance.clone.CreateClone(player.transform);
        }
    }

    //单纯使用预制体当占位符使用
    bool CanMultipleCrystal()
    {
        if (canMultiple)
        {
            if (crystalList.Count > 0)
            {
                UseCrystal();
                //标记用完水晶的时间
                if (crystalList.Count <= 0)
                {
                    multipleTime = Time.time + multipleCoolDown;
                }
                //使用了水晶但并未用完时
                else
                {
                    if (multipleTimer < 0)
                    {
                        multipleTimer = multipleWindow;
                        //需要装填已使用的水晶数量
                        for (int i = 0; i < crystalNum - crystalList.Count - 1; i++)
                        {
                            crystalList.Add(crystalPrefab);
                        }
                    }
                }
            }
            else
            {
                //用完crystalNum个水晶后经过CoolDown秒后再重新生成 
                if (Time.time > multipleTime)
                {
                    //填充列表
                    for (int i = 0; i < crystalNum; i++)
                    {
                        crystalList.Add(crystalPrefab);
                    }
                    //装填后开始计时
                    multipleTimer = multipleWindow;
                    UseCrystal();
                }
            }
            return true;
        }
        return false;
    }

    void UseCrystal()
    {
        //使用水晶
        GameObject prefab = crystalList[crystalList.Count - 1];
        GameObject crystalClone = Instantiate(prefab, player.transform.position, Quaternion.identity);
        crystalClone.GetComponent<CrystalController>().SetUpData(canExplore, crystalDruing, growScale,
            growSpeed, canMove, moveSpeed, checkDistance, CheckEnemy(crystalClone.transform, checkRadius));
        //计数使用
        crystalList.Remove(prefab);
    }

    //解锁技能

    protected override void CheckUnlock()
    {
        if (crystalSlot.unlock)
        {
            crystalUnlock = true;
        }
        if (crystalExplorsionSlot.unlock)
        {
            canExplore = true;
        }
        if (crystalMoveSlot.unlock)
        {
            canMove = true;
        }
        if (crystalMultipleSlot.unlock)
        {
            canMultiple = true;
        }
        if (crystalCloneSlot.unlock)
        {
            canCreateClone = true;
        }
    }
    public void AddUnlockListener()
    {
        crystalSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (crystalSlot.unlock)
            {
                crystalUnlock = true;
            }
        });
        crystalExplorsionSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (crystalExplorsionSlot.unlock)
            {
                canExplore = true;
            }
        });
        crystalMoveSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (crystalMoveSlot.unlock)
            {
                canMove = true;
            }
        });
        crystalMultipleSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (crystalMultipleSlot.unlock)
            {
                canMultiple = true;
            }
        });
        crystalCloneSlot.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (crystalCloneSlot.unlock)
            {
                canCreateClone = true;
            }
        });
    }
}
