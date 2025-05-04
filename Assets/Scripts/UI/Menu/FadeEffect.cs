using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    [SerializeField] float loadTime;
    void Start()
    {
        //在每次场景载入时调用
        FadeIn(loadTime, () => { });
    }

    public void FadeIn(float time, Action action)
    {
        gameObject.SetActive(true);
        Image image = GetComponent<Image>();
        image.color = Color.black;
        image.DOFade(0, time).OnComplete(() =>
        {
            action();
        });

    }
    public void FadeOut(float time, Action action,float delay = 0)
    {
        gameObject.SetActive(true);
        Image image = GetComponent<Image>();
        image.color = Color.clear;
        image.DOFade(1, time).SetDelay(delay)
        .OnComplete(() =>
        {
            action();
        });
    }
}
