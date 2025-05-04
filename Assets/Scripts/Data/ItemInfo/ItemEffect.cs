using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    [TextArea]   
    public string effectDescription;//效果描述
    public virtual void Effect(Transform target)
    {

    }
}
