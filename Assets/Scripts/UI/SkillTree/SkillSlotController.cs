using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlotController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool unlock;//是否解锁 默认为未解锁
    public int skillCost;//解锁所需货币
    [SerializeField] List<SkillSlotController> unlockList;//解锁时必须已经解锁的
    [SerializeField] List<SkillSlotController> belockList;//解锁是必须不能解锁的
    public string skillName;
    [TextArea]
    [SerializeField] string skillDes;
    [SerializeField] Color unLockColor;
    UIController controller;
    Image image;
    void Awake()
    {
        image = GetComponent<Image>();
        controller = GetComponentInParent<UIController>();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Unlock();
        });
    }

    void Start()
    {
        image.color = unLockColor;
        if(unlock){
            image.color = Color.white;
        }
    }

    public void Unlock()
    {
        //多次点击解锁无效
        if (unlock == true) return;
        //只要有一项条件未满足就退出
        for (int i = 0; i < unlockList.Count; i++)
        {
            if (unlockList[i].unlock != true)
            {
                GameManager.Instance.gameTip.ShowTip(GameManager.Instance.skillTips[0],unlockList[i].skillName + GameManager.Instance.skillTips[1]);
                Debug.Log("还有前置未解锁");
                return;
            }
        }
        for (int i = 0; i < belockList.Count; i++)
        {
            if (belockList[i].unlock != false)
            {
                GameManager.Instance.gameTip.ShowTip(GameManager.Instance.skillTips[0], GameManager.Instance.skillTips[2]);
                Debug.Log("已经选择另一条路线");
                return;
            }
        }
        //在前置条件判断完毕后才进行货币的判断
        if (PlayerManager.Instance.CheckCurrency(skillCost) == false)
        {
            Debug.Log("货币不足");
            GameManager.Instance.gameTip.ShowTip(GameManager.Instance.skillTips[0], GameManager.Instance.skillTips[3]);
            return;
        }
        unlock = true;
        image.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        controller.skillTip.ShowTip(skillName, skillDes, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        controller.skillTip.CloseTip();
    }
}
