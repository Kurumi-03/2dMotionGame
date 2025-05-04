using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    Player player => GetComponentInParent<Player>();
    void AnimationEnd()
    {
        player.CallTrriger();
    }

    void AttackResult()
    {
        //攻击第三次重击时执行屏幕抖动的特效
        if(player.attackCombo == 2){
            GameFX.Instance.screenShake.Shake(new Vector2(player.faceDir, 1));
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius[player.attackCombo]);
        bool isEffect = false;//是否已经执行过特效
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                player.stats.DoDamage(enemy.GetComponent<EnemyStats>());
                //第三次重击时才会击退敌人
                if (player.attackCombo == 2)
                {
                    enemy.GetComponent<Enemy>().SetHitBackDir(player.transform);
                    enemy.StartCoroutine(enemy.HitBack());
                }
                //用以在攻击到多个敌人时只执行一次武器特效
                if (!isEffect)
                {
                    InventoryManager.instance.UseEquipmentEffect(EquipmentType.Weapon, enemy.transform);
                    isEffect = true;
                }
            }
        }
    }

    void ThrowSword()
    {
        SkillManager.Instance.sword.CreateSword();
    }
}
