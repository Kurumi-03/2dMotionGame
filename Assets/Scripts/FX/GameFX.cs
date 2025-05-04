using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFX : MonoBehaviour
{
    public static GameFX Instance;
    public ScreenShake screenShake;
    public PopTip popTip;
    void GetInstance(){
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        GetInstance();
        screenShake = GetComponent<ScreenShake>();
        popTip = GetComponent<PopTip>();
    }
}
