using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject characterUI;//玩家面板
    [SerializeField] GameObject skillTreeUI;//技能树面板
    [SerializeField] GameObject craftUI;//合成面板
    [SerializeField] GameObject optionUI;//游戏设置面板
    [SerializeField] GameObject outGameUI;//主菜单面板
    [SerializeField] GameObject inGameUI;//游戏中UI面板
    [SerializeField] GameObject fadeEffect;//场景切换效果
    [SerializeField] GameObject endMenu;//最终显示菜单
    public ItemTipController itemTip;
    public SkillTipController skillTip;
    public CraftUIController craftInfo;
    void Awake()
    {
        //开启扫描未被激活的组件或物体
        itemTip = GetComponentInChildren<ItemTipController>(true);
        skillTip = GetComponentInChildren<SkillTipController>(true);
        craftInfo = GetComponentInChildren<CraftUIController>(true);
        //初始先唤醒一次让他执行awake函数注册按钮事件
        // ButtonSwitchPanel(skillTreeUI);
    }

    void Start()
    {
        //初始会关闭所有面板
        ButtonSwitchPanel(null);
        inGameUI.SetActive(true);
    }

    bool temp;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ButtonSwitchPanel(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ButtonSwitchPanel(skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ButtonSwitchPanel(craftUI);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ButtonSwitchPanel(optionUI);
        }
        //当玩家死亡后调用
        if (PlayerManager.Instance.player.stats.isDead)
        {
            if (temp == false)
            {
                fadeEffect.GetComponent<FadeEffect>().FadeOut(2, () =>
                {
                    endMenu.GetComponent<EndMenu>().GameEnd();
                }, 1);
                temp = true;
            }
        }
    }
    public void SwitchOutPanel(GameObject menu)
    {
        for (int i = 0; i < outGameUI.transform.childCount; i++)
        {
            outGameUI.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (menu != null)
        {
            GameManager.Instance.GamePause(true);
            menu.SetActive(true);
            inGameUI.SetActive(false);
            InventoryManager.instance.UpdateUI();
        }
        else
        {
            SkillTreeUI();
        }
    }

    public void ButtonSwitchPanel(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            inGameUI.SetActive(true);
            GameManager.Instance.GamePause(false);
            return;
        }
        SwitchOutPanel(menu);
    }

    //技能数面板有特殊处理
    bool isSkillTreeFirstOpen = true;
    void SkillTreeUI()
    {
        if (isSkillTreeFirstOpen)
        {
            //注册初始事件
            isSkillTreeFirstOpen = false;
            //需要解锁在判断之前  需要需要提前唤醒skill注册事件
            skillTreeUI.SetActive(true);
            skillTreeUI.SetActive(false);
            SkillManager.Instance.AddAllListener();
        }
    }
}
