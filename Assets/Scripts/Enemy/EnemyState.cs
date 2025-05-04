using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{

    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;

    protected float timer;//所有状态共用的计时器
    protected bool animationTrriger = false;
    protected float lastAttackTime;
    string stateBoolName;

    public EnemyState(EnemyStateMachine _stateMachine, Enemy _enemy, string _boolname)
    {
        stateMachine = _stateMachine;
        enemy = _enemy;
        stateBoolName = _boolname;
    }

    public virtual void Enter()
    {
        animationTrriger = false;
        enemy.anim.SetBool(stateBoolName, true);
    }

    public virtual void Update()
    {
        timer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(stateBoolName, false);
    }

    public virtual void OnAnimationEnd()
    {
        animationTrriger = true;
    }
}
