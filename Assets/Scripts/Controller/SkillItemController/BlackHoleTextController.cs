using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleTextController : MonoBehaviour
{
    TextMeshProUGUI textMesh;
    SpriteRenderer sr;
    public KeyCode keyCode;//对应的按键
    BlackHoleController controller;//控制器脚本
    Transform enemy;//按键对应敌人
    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            //使按键提示不显示
            textMesh.color = Color.clear;
            sr.color = Color.clear;
            //在对应的敌人位置创建玩家的克隆体进行攻击
            controller.CloneAttack(enemy);
        }
    }
    public void SetKeyCode(KeyCode code, Transform _enemy, BlackHoleController _controller)
    {
        textMesh.text = code.ToString();
        keyCode = code;
        controller = _controller;
        enemy = _enemy;
    }

}
