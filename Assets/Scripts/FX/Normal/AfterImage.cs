using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AfterImage : MonoBehaviour
{

    public void SetUpData(Sprite image, float duringTime, float dir)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = image;
        transform.localScale = new Vector3(dir, 1, 1);
        sr.DOFade(0, duringTime).SetAutoKill(true).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
    void OnDestroy()
    {
        //在销毁物体前先将动画关闭
        DOTween.Kill(GetComponent<SpriteRenderer>());
    }
}
