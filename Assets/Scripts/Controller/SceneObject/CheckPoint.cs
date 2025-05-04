using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator animator;
    public string id;
    public bool active;//是否被激活
    [Range(1, 100)]
    public int sort;//检查点顺序  需要在Inspector面板赋予
    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        //当有玩家靠近时点亮
        if (collision.GetComponent<Player>() != null)
        {
            Active();
        }
    }

    public void Active()
    {
        animator.SetBool("active", true);
        active = true;
    }

    [ContextMenu("Create Guid")]
    public void GetGuid()
    {
        id = Guid.NewGuid().ToString();
    }
}
