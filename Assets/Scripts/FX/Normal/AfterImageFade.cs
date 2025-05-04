using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFade : MonoBehaviour
{
    [SerializeField] GameObject imagePrefab;
    Sprite imgSprite;
    int dir;
    Transform target;
    float fadeTime;
    float imageCd;
    float imageCdTimer;

    void Update()
    {
        Check();
    }

    void Check()
    {
        if (imageCdTimer > 0)
        {
            imageCdTimer -= Time.deltaTime;
        }
        else
        {
            imageCdTimer = imageCd;
            CreateImg();
        }
    }

    public void SetUpData(float _imageCd, float _duringTime, float _fadeTime, Sprite _sprite, int _dir, Transform _target)
    {
        imageCd = _imageCd;
        fadeTime = _fadeTime;
        imgSprite = _sprite;
        dir = _dir;
        target = _target;
        Destroy(gameObject, _duringTime);
    }

    void CreateImg()
    {
        GameObject image = Instantiate(imagePrefab, target.position, Quaternion.identity, transform);
        image.GetComponent<AfterImage>().SetUpData(imgSprite, fadeTime, dir);
    }
}
