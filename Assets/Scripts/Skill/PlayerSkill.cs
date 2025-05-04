using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] protected float cd;
    protected float cdTimer;
    protected Player player;
    public List<SkillSlotController> skillTreeSlots = new List<SkillSlotController>();

    protected virtual void Start()
    {
        player = PlayerManager.Instance.player;
        StartCoroutine(DelayUnlcok());
    }

    //只有继承update函数才能开始计时
    protected virtual void Update()
    {
        cdTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cdTimer < 0)
        {
            UseSkill();
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void UseSkill()
    {
        cdTimer = cd;
    }

    //查找当前最近的敌人位置
    public virtual Transform CheckEnemy(Transform check, float radius)
    {
        //设置一个大值用以检测
        Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, radius);
        float minDistance = Mathf.Infinity;
        Transform enemy = null;//最近的敌人
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (minDistance > Vector2.Distance(hit.transform.position, check.position))
                {
                    minDistance = Vector2.Distance(hit.transform.position, check.position);
                    enemy = hit.transform;
                }
            }
        }
        return enemy;
    }

    //延迟一帧执行解锁避免出错
    IEnumerator DelayUnlcok()
    {
        yield return new WaitForSeconds(0);
        CheckUnlock();
    }

    protected virtual void CheckUnlock()
    {

    }

    public float GetCD()
    {
        return cd;
    }
}
