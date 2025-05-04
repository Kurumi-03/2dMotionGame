using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;

    protected float xInput;//水平输入
    protected float yInput;//垂直输入
    protected float timer;//所有状态共用的计时器
    protected bool animationTrriger = false;//动画结束标记
    public float combo;//普通攻击段数
    string stateBoolName;//状态转换bool值

    protected bool isJumping = false;
    protected bool isAttacking = false;
    protected bool isCounterAttack = false;
    protected bool isBlackHole = false;
    protected bool isAniming = false;
    protected bool isAnimEnd = false;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string stateBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.stateBoolName = stateBoolName;
    }

    //进入状态
    public virtual void Enter()
    {
        player.anim.SetBool(stateBoolName, true);
        // Debug.Log(stateBoolName+" Enter");
        animationTrriger = false;//每次进入一个新的动画都需要重置
    }

    //状态内部逻辑
    public virtual void Update()
    {
        //当游戏暂停后需要停止状态输入
        if(Time.timeScale == 0) return;
        // Debug.Log(stateBoolName+" Update");
        // Debug.Log("xVelocity: " + player.rb.velocity.x + "   yVelocity: " + player.rb.velocity.y);
        timer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", player.rb.velocity.y);
        player.anim.SetFloat("combo", combo);
        if (Input.GetKeyDown(KeyCode.F) && SkillManager.Instance.parry.parryUnlock && SkillManager.Instance.parry.CanUseSkill())
        {
            isCounterAttack = true;
        }
        if (Input.GetMouseButtonDown(1) && SkillManager.Instance.dash.CanUseSkill() && SkillManager.Instance.dash.dashUnlock)
        {
            stateMachine.ChangeState(player.dashState);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
        }

        if (!player.CheckIsGround() && player.CheckIsWall())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            isJumping = true;
        }
        if (Input.GetKey(KeyCode.Q) && SkillManager.Instance.sword.swordUnlock && SkillManager.Instance.sword.canThrow && player.CheckIsGround() && SkillManager.Instance.sword.CanUseSkill())
        {
            stateMachine.ChangeState(player.aimSwordState);
        }
        if (Input.GetKeyUp(KeyCode.Q) && SkillManager.Instance.sword.swordUnlock)
        {
            isAnimEnd = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && SkillManager.Instance.sword.swordUnlock)
        {
            SkillManager.Instance.sword.CanReturnSword();
        }
        if (Input.GetKeyDown(KeyCode.Space) && SkillManager.Instance.blackHole.CanUseSkill() && player.CheckIsGround() && SkillManager.Instance.blackHole.blackHoleUnlock)
        {
            stateMachine.ChangeState(player.blackHoleState);
        }
        if (Input.GetKeyDown(KeyCode.C) && SkillManager.Instance.crystal.crystalUnlock && SkillManager.Instance.crystal.CanUseSkill())
        {
            // SkillManager.Instance.crystal.UseSkill();
        }
    }

    //离开状态
    public virtual void Exit()
    {
        player.anim.SetBool(stateBoolName, false);
        // Debug.Log(stateBoolName+" Exit");
    }

    //标记每个动画的结束
    public virtual void OnAnimationEnd()
    {
        animationTrriger = true;
    }
}
