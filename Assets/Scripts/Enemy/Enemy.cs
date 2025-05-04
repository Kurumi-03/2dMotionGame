using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    protected EnemyStateMachine stateMachine { get; private set; }
    [SerializeField] protected LayerMask palyerLayer;
    [SerializeField] protected float playerDistance;
    [SerializeField] protected GameObject attackTip;
    protected bool canStun;//是否能够被眩晕
    private float defaultMoveSpeed;//默认移动速度  用以记录之前的移速便于恢复
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }
    protected override void Start()
    {
        base.Start();

        gameObject.layer = LayerMask.NameToLayer("Enemy");
        palyerLayer = LayerMask.GetMask("Player");
        attackTip = transform.Find("AttackTip").gameObject;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        // Debug.Log( stateMachine.currentState.ToString());
    }


    //在动画中检测眩晕状态是否能够开始
    public virtual void OpenAttackTip()
    {
        canStun = true;
        attackTip.SetActive(true);
    }

    //在动画中检测眩晕状态是否能够结束
    public virtual void CloseAttackTip()
    {
        canStun = false;
        attackTip.SetActive(false);
    }

    //检测当前是否在眩晕状态中
    public virtual bool IsStuning()
    {
        if (canStun)
        {
            CloseAttackTip();
            return true;
        }
        return false;
    }

    //使人物被停止行动和动画
    public virtual void FreezingTime(bool isFreeze)
    {
        if (isFreeze)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    //这个携程可用下面的SlowAction方法替换
    protected virtual IEnumerator StartFreezing(float time)
    {
        FreezingTime(true);
        yield return new WaitForSeconds(time);
        FreezingTime(false);
    }

    public override void SlowAction(float slowTime, float slowPercent)
    {
        base.SlowAction(slowTime, slowPercent);
        moveSpeed *= 1 - slowPercent;
    }

    public override void RecoverAction()
    {
        base.RecoverAction();
        moveSpeed = defaultMoveSpeed;
    }

    public RaycastHit2D CheckIsPlayer()
    {
        return Physics2D.Raycast(transform.position, Vector2.right * faceDir, playerDistance, palyerLayer);
    }

    public override void Die()
    {
        base.Die();
        GetComponent<DropSystem>().DropItem();
        Destroy(gameObject, 2);//死亡后两秒销毁
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + playerDistance * faceDir, transform.position.y));
    }

    public void CallTrigger()
    {
        stateMachine.currentState.OnAnimationEnd();
    }
}
