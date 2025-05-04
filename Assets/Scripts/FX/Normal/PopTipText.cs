using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopTipText : MonoBehaviour
{
    float moveSpeed;
    float timer;
    float duringTime;
    float showTime;
    bool isFade;
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > duringTime - showTime)
        {
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
        }
        else if (timer <= duringTime - showTime && isFade == false)
        {
            isFade = true;
            GetComponent<TextMeshPro>().DOFade(0, duringTime - showTime).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
    public void SetUpData(string text, float _showTime, float _duringTime, float _moveSpeed)
    {
        showTime = _showTime;
        duringTime = _duringTime;
        moveSpeed = _moveSpeed;
        GetComponent<TextMeshPro>().text = text;
        timer = duringTime;
    }

    void OnDestroy()
    {
        DOTween.Kill(GetComponent<TextMeshPro>());
    }
}
