using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] GameObject textPrefab;//按键文本提示预制体
    float lastScale;//最终大小
    float growSpeed;//增长速度
    float shrinkSpeed;//收缩速度
    float yOffset;//按键文本显示偏移
    float xOffset;//克隆体生成位置相对于敌人的偏移
    float attackCoolDown;//每个克隆体的攻击间隔

    List<KeyCode> keyCodes = new List<KeyCode>();//存储可以使用的按键
    [SerializeField] List<Transform> enemys = new List<Transform>();//存储探测到的敌人
    [SerializeField] List<Transform> sortEnemys = new List<Transform>();//存储根据按键按下顺序得到的敌人
    [SerializeField] List<GameObject> texts = new List<GameObject>();//存储创建的文本提示

    float attackTimer;//攻击计时器
    float skillTimer;//技能时间总计时器
    bool canAttack;//是否能够进行攻击
    bool canGrow = true;//是否能够增长  默认为true 即创建时开始增长
    bool canShink;//黑洞开始收缩
    int attackIndex;//攻击敌人顺序序号

    void Update()
    {
        ScaleChange();
        Attack();
        Check();
    }

    //设置基础数据
    public void SetUpData(float _lastScale, float _growSpeed, float _shrinkSpeed, float _yOffset
        , float _xOffset, float _attackCoolDown, List<KeyCode> _keyCodes, float _blackHoleDuring)
    {
        lastScale = _lastScale;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        yOffset = _yOffset;
        xOffset = _xOffset;
        attackCoolDown = _attackCoolDown;
        keyCodes.AddRange(_keyCodes);
        skillTimer = _blackHoleDuring;
    }

    void ScaleChange()
    {
        //增长
        if (canGrow && !canShink)
        {
            //先快后慢的增长
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(lastScale, lastScale), Time.deltaTime * growSpeed);
        }
        //收缩
        if (canShink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(-1, -1), Time.deltaTime * shrinkSpeed);
            if (transform.lossyScale.x <= 0)
            {
                canShink = false;
                //技能结束后 更改玩家状态并使玩家显形
                PlayerManager.Instance.player.fx.SetTransparent(false);
                PlayerManager.Instance.player.stateMachine.ChangeState(PlayerManager.Instance.player.idleState);
                Destroy(gameObject);
            }
        }
    }

    //此处可能还有问题
    void Attack()
    {
        attackTimer -= Time.deltaTime;
        //此处需要按键全部按完后或是技能时间到后才能进行攻击
        if ((canAttack || skillTimer < 0) && attackTimer < 0 && (enemys.Count <= 0 || skillTimer < 0))
        {
            // Debug.Log("进入一次判断");
            // Debug.Log("attackIndex:"+attackIndex+",sortEnemys.Count:"+sortEnemys.Count);
            if (attackIndex >= sortEnemys.Count)
            {
                // Debug.Log("进入二次判断");
                //攻击完成后 黑洞收缩，销毁文本提示
                canAttack = false;
                canShink = true;
                for (int i = 0; i < texts.Count; i++)
                {
                    Destroy(texts[i]);
                }
                return;
            }
            attackTimer = attackCoolDown;
            //开始使用技能攻击时玩家隐身
            PlayerManager.Instance.player.fx.SetTransparent(true);//可优化
            int dir = sortEnemys[attackIndex].GetComponent<Enemy>().faceDir;//敌人当前朝向
            //克隆体根据敌人当前朝向进行背刺攻击
            SkillManager.Instance.clone.CreateClone(sortEnemys[attackIndex], new Vector3(xOffset * -dir, 0, 0));
            attackIndex++;
        }
    }

    //当前使用的各种检测
    void Check()
    {
        skillTimer -= Time.deltaTime;
        //按键检测
        if (Input.GetKeyDown(KeyCode.R))
        {
            canAttack = true;
        }
    }

    public void CloneAttack(Transform enemy)
    {
        enemys.Remove(enemy);
        sortEnemys.Add(enemy);
    }

    //使用触发检测靠近的敌人
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            //当敌人数量超过上限或是正在进行攻击了时，不再进行后续判定
            if (keyCodes.Count <= 0 || canAttack || canShink)
            {
                return;
            }
            Transform enemy = collision.GetComponent<Enemy>().transform;
            enemy.GetComponent<Enemy>().FreezingTime(true);
            enemys.Add(enemy);
            GameObject text = Instantiate(textPrefab, new Vector3(enemy.position.x, enemy.position.y + yOffset), Quaternion.identity);
            BlackHoleTextController textController = text.GetComponent<BlackHoleTextController>();
            KeyCode tempCode = keyCodes[Random.Range(0, keyCodes.Count)];
            textController.SetKeyCode(tempCode, enemy, this);
            keyCodes.Remove(tempCode);//避免重复key
            texts.Add(text);
        }
    }

    //黑洞最后收缩时使离开范围的敌人能够重新活动
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezingTime(false);
        }
    }
}
