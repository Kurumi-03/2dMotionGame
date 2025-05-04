using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Path;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    [SerializeField] Transform attackCheck;
    SpriteRenderer sr;
    Animator anim;
    float attackRadius;//攻击半径
    float checkRadius;//敌人检测半径
    float dispearSpeed;//克隆体消失速度
    float cloneDuring;//克隆体显示时间
    bool canAttack;//克隆体是否能够攻击
    bool canDuplicate;//在克隆体攻击有概率再次创建克隆体攻击
    float duplicateChance;//复制概率
    float damagePercent;//克隆体伤害百分比
    float timer;
    int faceDir = 1;//标识当前克隆体的方向  默认保持预制体向右的方向


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - (Time.deltaTime * dispearSpeed));
            //渐隐完毕后销毁
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetUpData(float _during, float _speed, bool _canAttack, float _attackRadius, float _checkRadius,
        bool _canDuplicate, float _duplicateChance, float _damagePercent)
    {
        cloneDuring = _during;
        dispearSpeed = _speed;
        canAttack = _canAttack;
        attackRadius = _attackRadius;
        checkRadius = _checkRadius;
        canDuplicate = _canDuplicate;
        duplicateChance = _duplicateChance;
        damagePercent = _damagePercent;

        timer = cloneDuring;
        if (canAttack)
        {
            anim.SetInteger("AttackNum", Random.Range(1, 4));
        }
        CheckEnemy();
    }

    //不能删除
    void AnimationEnd()
    {

    }


    void AttackResult()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                PlayerManager.Instance.player.stats.DoDamage(hit.GetComponent<CharacterStats>(), damagePercent);
                //克隆体是否应用武器效果
                if (SkillManager.Instance.clone.cloneEffectUnlock)
                {
                    //克隆体技能效果
                    SkillManager.Instance.clone.CloneEffect(hit.transform);
                }
                //攻击后判定
                if (canDuplicate && Random.Range(0, 100) < duplicateChance)
                {
                    SkillManager.Instance.clone.CreateClone(hit.transform, new Vector3(faceDir * attackRadius, 0));
                }
            }
        }
    }

    //寻找最近的敌人并转向 
    void CheckEnemy()
    {
        //设置一个大值用以检测
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        float minDistance = Mathf.Infinity;
        Transform enemy = null;//最近的敌人
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (minDistance > Vector2.Distance(hit.transform.position, transform.position))
                {
                    minDistance = Vector2.Distance(hit.transform.position, transform.position);
                    enemy = hit.transform;
                }
            }
        }
        //因为克隆体默认朝向为右，所以当敌人在克隆体左边时改变朝向即可
        if (enemy != null && enemy.position.x < transform.position.x)
        {
            faceDir = -1;
            transform.Rotate(0, 180, 0);
        }

    }
}
