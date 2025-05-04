using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderController : MonoBehaviour
{
    Animator anim;
    [SerializeField] CharacterStats target;//目标状态
    [SerializeField] float moveSpeed;//闪电移动速度
    float checkDistance = 0.1f;//与目标之间的检测距离
    float checkRadius = 20;//范围检测的半径
    int damage;//闪电造成伤害
    Transform targetEnemy = null;

    bool isStrike;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        CheckDistance();
        Strike();
        Move();
    }

    void CheckDistance()
    {
        if (isStrike)
        {
            return;
        }
        //范围检测最近的敌人
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        float minDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //雷电会优先攻击未被攻击到的目标
                if (hit.gameObject == target.gameObject) continue;
                if (Vector3.Distance(hit.transform.position, transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(hit.transform.position, transform.position);
                    targetEnemy = hit.transform;
                }
            }
        }
        //当周围只有一个敌人时
        if (targetEnemy == null)
        {
            targetEnemy = target.transform;
        }
    }

    void Strike()
    {
        if (isStrike)
        {
            return;
        }
        //检测与目标之间距离
        if (Vector3.Distance(transform.position, targetEnemy.position) < checkDistance)
        {
            isStrike = true;
            //重置雷电方向
            transform.rotation = Quaternion.identity;
            transform.GetChild(0).rotation = Quaternion.identity;
            anim.SetTrigger("strike");
        }
    }

    void Move()
    {
        if (isStrike) return;
        //设置移动方向
        transform.right = targetEnemy.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetEnemy.position, moveSpeed * Time.deltaTime);
    }

    public void SetUpData(CharacterStats _stats, int _damage, float _moveSpeed)
    {
        target = _stats;
        damage = _damage;
        moveSpeed = _moveSpeed;
    }

    public void Damage()
    {
        targetEnemy.GetComponent<EnemyStats>().TakeDamage(damage,PlayerManager.Instance.player.transform);
    }

    public void EndDestroy()
    {
        Destroy(gameObject);
    }
}
