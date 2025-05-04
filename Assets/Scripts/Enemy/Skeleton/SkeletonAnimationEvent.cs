using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationEvent : MonoBehaviour
{
    EnemySkeleton skeleton => GetComponentInParent<EnemySkeleton>();
    void AnimationEnd(){
        skeleton.CallTrigger();
    }

    void AttackResult(){
        Collider2D[] colliders = Physics2D.OverlapCircleAll(skeleton.attackCheck.position,skeleton.attackRadius[0]);
        foreach(var hit in colliders){
            if(hit.GetComponent<Player>() != null){
                skeleton.stats.DoDamage(hit.GetComponent<PlayerStats>());
            }
        }
    }

    void CanStun(){
        skeleton.OpenAttackTip();
    }

    void CancelStun(){
        AttackResult();
        skeleton.CloseAttackTip();
    }
}
