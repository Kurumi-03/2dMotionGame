using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public EntityFX fx { get; private set; }//特效显示脚本

    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform groundCheck = null;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform wallCheck = null;
    public Transform attackCheck = null;

    public float moveSpeed;
    public int faceDir { get; private set; } = 1;//1为右，-1为左
    public float[] attackRadius;//攻击半径  可以根据攻击段数设置不同攻击的攻击距离
    public int attackCombo;//攻击段数
    public bool isHitBack;//是否被击退
    public Vector2 hitBackPower;//击退后移动方向
    public float hitBackTime;//击退持续时间
    public int hitBackDir;//击退方向

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
    }

    protected virtual void Update()
    {

    }

    //设置物体速度
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        //被击退时不能移动
        if (isHitBack) return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        //当x方向速度不为0且速度方向与面朝方向不一致时，翻转
        if (rb.velocity.x != 0 && faceDir * rb.velocity.x < 0)
        {
            Filp();
        }
    }

    //翻转
    public virtual void Filp()
    {
        faceDir *= -1;
        transform.Rotate(0, 180, 0);
    }
    public virtual void SetHitBackDir(Transform target)
    {
        if (target.position.x > transform.position.x)
        {
            hitBackDir = -1;
        }
        else
        {
            hitBackDir = 1;
        }
    }

    

    public IEnumerator HitBack()
    {
        isHitBack = true;
        //乘以负方向是为了让被击退者向身后被击退
        // Debug.Log("HitBackDir:" + hitBackDir);
        rb.velocity = new Vector2(hitBackPower.x * hitBackDir, hitBackPower.y);
        yield return new WaitForSeconds(hitBackTime);
        isHitBack = false;
    }

    //冰冻效果显示
    public virtual void SlowAction(float slowTime, float slowPercent)
    {
        anim.speed *= 1 - slowPercent;
        //定时消除效果
        Invoke("RecoverAction", slowTime);
    }

    //解除冰冻效果
    public virtual void RecoverAction()
    {
        anim.speed = 1;
    }

    //死亡函数
    public virtual void Die()
    {

    }

    //检测
    public bool CheckIsGround()
    {
        return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    }
    public bool CheckIsWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, wallCheckDistance, groundLayer);
    }


    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * faceDir * wallCheckDistance);
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius[attackCombo]);
    }
}
