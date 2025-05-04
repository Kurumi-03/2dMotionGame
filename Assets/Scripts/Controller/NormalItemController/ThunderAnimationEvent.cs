using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAnimationEvent : MonoBehaviour
{
    ThunderController controller;
    void Start()
    {
        controller = GetComponentInParent<ThunderController>();
    }

    void Update()
    {
        
    }

    void EndDestroy(){
        controller.EndDestroy();
    }

    void Damage(){
        controller.Damage();
    }
}
