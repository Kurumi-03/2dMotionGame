using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//在次脚本中使用实践监听来控制ui血量显示
public class StateUIController : MonoBehaviour
{
    Entity entity;
    Slider slider;

    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        entity = GetComponentInParent<Entity>();
        if (entity == null)
        {
            entity = PlayerManager.Instance.player;
        }
        //事件监听添加
        entity.stats.updateHP += UpdateUI;
    }

    void Start()
    {
        //最开始时设置ui初始显示
        UpdateUI();
    }

    void Update()
    {
        //用以保持血量条的显示固定方向
        GetComponent<RectTransform>().rotation = Quaternion.identity;
    }

    //更新血量显示
    void UpdateUI()
    {
        slider.maxValue = entity.stats.GetMaxHP();
        slider.value = entity.stats.currentHp;
    }

    //事件监听移除
    void OnDestroy()
    {
        entity.stats.updateHP -= UpdateUI;
    }
}
