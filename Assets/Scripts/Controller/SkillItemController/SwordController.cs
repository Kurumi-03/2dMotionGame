using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public bool canRotate;//设置剑不旋转时的飞行方向
    Animator anim;
    CircleCollider2D cd;
    Rigidbody2D rb;

    bool isReturning;
    float freezeTime;//剑能够冻结敌人时间
    float swordSpeed;//返回速度
    float checkDistance;//返回时与玩家的检测距离

    bool isBouncing;//是否可以反弹   需要点技能树
    float bounceSpeed;//反弹速度
    float bounceCheckRadius;//剑对敌人的检测半径
    float enemyDistance;//剑与敌人之间的检测距离
    int bounceNum;//反弹次数

    bool isPiercing;//是否可以穿刺   需要点技能树
    float pierceGravity;//穿刺时重力
    int pieceNum;//穿刺次数

    bool isSpining;//是否可以自旋   需要点技能树
    float moveDiatance;//剑能够离开的最大距离
    float hitCoolDown;//剑旋转时的攻击cd
    float spinTime;//剑旋转持续时间
    float damageRadius;//伤害范围

    List<Transform> enemys = new List<Transform>();//获取到范围内所有敌人
    int enemyIndex = 0;//范围内敌人序号
    float spinTimer;//旋转计时器
    float hitTimer;//伤害计时器
    bool spinStart;//是否开始自旋
    bool isTouchGround;//触碰到地面

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        cd = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //使剑的右方向一直为速度方向
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
        //剑的返回移动
        if (isReturning)
        {
            anim.SetBool("flip", true);
            transform.position = Vector2.MoveTowards(transform.position,
                PlayerManager.Instance.player.transform.position, swordSpeed * Time.deltaTime);
            if (Vector2.Distance(PlayerManager.Instance.player.transform.position, transform.position) < checkDistance)
            {
                SkillManager.Instance.sword.playerSwords.Remove(gameObject);
                //玩家接受剑时需要翻转
                if (transform.position.x > PlayerManager.Instance.player.transform.position.x
                    && PlayerManager.Instance.player.faceDir == -1)
                {
                    PlayerManager.Instance.player.Filp();
                }
                else if (transform.position.x < PlayerManager.Instance.player.transform.position.x
                    && PlayerManager.Instance.player.faceDir == 1)
                {
                    PlayerManager.Instance.player.Filp();
                }
                //接受到剑后需要切换状态
                PlayerManager.Instance.player.stateMachine.ChangeState(PlayerManager.Instance.player.catchSwordState);
                anim.SetBool("flip", false);
                Destroy(gameObject);
            }
        }
        //剑的反弹移动
        if (isBouncing && enemys.Count > 1 && bounceNum > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemys[enemyIndex].position,
                bounceSpeed * Time.deltaTime);
            //检测剑与敌人之间的距离
            if (Vector2.Distance(transform.position, enemys[enemyIndex].position) < enemyDistance)
            {
                Damage(enemys[enemyIndex].GetComponent<Enemy>());
                // PlayerManager.Instance.player.stats.DoDamage(enemys[enemyIndex].GetComponent<CharacterStats>());
                // InventoryManager.instance.UseEquipmentEffect(EquipmentType.Amulet, enemys[enemyIndex]);
                // enemys[enemyIndex].GetComponent<Enemy>().StartCoroutine("StartFreezing", freezeTime);
                //靠近后攻击下一个敌人
                enemyIndex++;
                if (enemyIndex >= enemys.Count)
                {
                    enemyIndex = 0;
                }
                bounceNum--;
                if (bounceNum <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
            }
        }
        //剑的穿刺移动
        if (isPiercing && pieceNum > 0)
        {
            rb.gravityScale = pierceGravity;
        }
        //剑的自旋技能
        if (isSpining)
        {
            //碰到地面或是到达最大距离 且 并未开始旋转时
            if (Vector2.Distance(transform.position,
                PlayerManager.Instance.player.transform.position) >= moveDiatance && !spinStart)
            {
                spinStart = true;
                spinTimer = spinTime;
            }
            if (spinStart)
            {
                spinTimer -= Time.deltaTime;
                hitTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isSpining = false;
                    isReturning = true;
                }
                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            PlayerManager.Instance.player.stats.DoDamage(hit.GetComponent<CharacterStats>());
                            InventoryManager.instance.UseEquipmentEffect(EquipmentType.Amulet, hit.transform);
                        }
                    }
                }
            }
        }
    }

    public void SetUpSword(Vector2 _velocity, float _gravity, float _speed, float _distance, float _freezeTime)
    {
        rb.velocity = _velocity;
        rb.gravityScale = _gravity;
        swordSpeed = _speed;
        checkDistance = _distance;
        freezeTime = _freezeTime;
        anim.SetBool("flip", true);
    }

    public void SetUpBoundData(bool _isBouncing, float _bounceSpeed, float _checkRadius, float _enemyDistance, int _bounceNum)
    {
        isBouncing = _isBouncing;
        bounceSpeed = _bounceSpeed;
        bounceCheckRadius = _checkRadius;
        enemyDistance = _enemyDistance;
        bounceNum = _bounceNum;
    }

    public void SetUpPierceData(bool _isPiercing, float _pierceGravity, int _pieceNum)
    {
        isPiercing = _isPiercing;
        pierceGravity = _pierceGravity;
        pieceNum = _pieceNum;
    }

    public void SetUpSpinData(bool _isSpining, float _moveDiatance, float _hitCoolDown, float _spinTime, float _damageRadius)
    {
        isSpining = _isSpining;
        moveDiatance = _moveDiatance;
        hitCoolDown = _hitCoolDown;
        spinTime = _spinTime;
        damageRadius = _damageRadius;
    }


    //使用触发检测将剑赋给被碰撞的物体，使剑能保持速度展现出卡在被碰撞物体上的效果
    void OnTriggerEnter2D(Collider2D collision)
    {
        //检测是否碰到地面
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isTouchGround = true;
        }
        //剑返回时不做检测
        if (isReturning) return;
        //碰撞时对敌人的伤害
        if (collision.GetComponent<Enemy>() != null)
        {
            Damage(collision.GetComponent<Enemy>());
            // PlayerManager.Instance.player.stats.DoDamage(collision.GetComponent<CharacterStats>());
            // InventoryManager.instance.UseEquipmentEffect(EquipmentType.Amulet, collision.transform);
            // if (SkillManager.Instance.sowrd.freezeUnlock)
            // {
            //     collision.GetComponent<Enemy>().StartCoroutine("StartFreezing", freezeTime);
            // }
        }

        //剑的穿刺
        if (isPiercing && pieceNum > 0)
        {
            //穿刺到地面时就不能再进行穿刺
            if (isTouchGround)
            {
                pieceNum = 0;
            }
            else
            {
                pieceNum--;
                return;
            }
        }

        //剑的自旋
        if (isSpining)
        {
            //触碰到第一个物体时停止
            spinStart = true;
            spinTimer = spinTime;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }

        canRotate = false;//避免被触发时速度为0使剑的方向被更改
        rb.bodyType = RigidbodyType2D.Kinematic;//将剑设为静态，不再进行移动
        cd.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;


        //剑的反弹
        //只有剑第一次撞到敌人后续才可以反弹
        if (collision.GetComponent<Enemy>() != null && isBouncing)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bounceCheckRadius);
            foreach (var hit in colliders)
            {
                if (hit.GetComponent<Enemy>() != null)
                {
                    enemys.Add(hit.transform);
                }
            }
            //只有具有两个以上敌人时才能进行反弹
            if (enemys.Count > 1) return;
        }

        anim.SetBool("flip", false);
        transform.parent = collision.transform;
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;//避免rigidbody影响移动
        transform.parent = null;
        isReturning = true;
    }

    void Damage(Enemy enemy)
    {
        PlayerManager.Instance.player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        InventoryManager.instance.UseEquipmentEffect(EquipmentType.Amulet, enemy.transform);
        if (SkillManager.Instance.sword.swordFreezeUnlock)
        {
            enemy.StartCoroutine("StartFreezing", freezeTime);
        }
        if (SkillManager.Instance.sword.swordVulnerabilityUnlock)
        {
            SkillManager.Instance.sword.SetVulnerabilityEffect(enemy.GetComponent<CharacterStats>());
        }
    }
}
