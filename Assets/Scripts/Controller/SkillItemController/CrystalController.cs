using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    Animator anim;
    CircleCollider2D circleCollider;
    bool canExplore;//是否能爆炸
    float growScale;//增长大小
    float growSpeed;//增长速度
    bool canMove;//是否可以向最近的敌人移动  技能树升级得到
    float moveSpeed;//移动速度
    float checkDistance;//检测与敌人之间的距离进行爆炸
    Transform enemy;//最近的敌人
    float timer;//持续存在时间计时器
    bool isExploring;//是否正在爆炸 
    bool exploreStart;
    void Start()
    {
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        //定时销毁 需要在自身当前位置进行爆炸
        if (timer < 0 && !exploreStart)
        {
            if (canExplore)
            {
                Grow();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        if (exploreStart)
        {
            Grow();
        }
        if (canMove)
        {
            if (enemy != null)
            {
                //向最近的敌人移动
                transform.position = Vector2.MoveTowards(transform.position, enemy.position, moveSpeed * Time.deltaTime);
                //到达指定距离后自爆
                if (Vector2.Distance(enemy.position, transform.position) < checkDistance)
                {
                    canMove = false;//自爆时不能移动
                    exploreStart = true;
                }
            }
        }
    }

    public void SetUpData(bool _canExplore, float _crystalDuring, float _growScale, float _growSpeed, bool _canMove, float _moveSpeed, float _checkDistance, Transform _enemy)
    {
        canExplore = _canExplore;
        timer = _crystalDuring;
        growScale = _growScale;
        growSpeed = _growSpeed;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        checkDistance = _checkDistance;
        enemy = _enemy;
    }

    //交换后需要在玩家之前的位置进行爆炸 且在爆炸时不能换位
    public bool Explorsion()
    {
        exploreStart = true;
        return isExploring;
    }

    void Grow()
    {
        isExploring = true;
        anim.SetBool("explore", true);
        transform.localScale = Vector3.Lerp(transform.localScale,
            new Vector3(growScale, growScale), growSpeed * Time.deltaTime);
    }

    //爆炸动画结束后执行对敌人的伤害
    private void ExplorsionEnd()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius * 1.5f);
        bool isEffect = false;//是否已经执行过特效
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.Instance.player.stats.DoMagicDamage(hit.GetComponent<CharacterStats>());
                if(isEffect == false)
                {
                    InventoryManager.instance.UseEquipmentEffect(EquipmentType.Amulet, hit.transform);
                    isEffect = true;
                }
            }
        }
    }

    private void AimationEnd()
    {
        Destroy(gameObject);
    }
}
