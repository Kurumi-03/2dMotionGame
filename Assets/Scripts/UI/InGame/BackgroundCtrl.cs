using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{
    public Transform mainCamera = null;
    [SerializeField] float offset;
    float length;
    float xPos;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        xPos = mainCamera.transform.position.x;
        length = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        //制作无尽背景
        float distance = mainCamera.position.x * (1 - offset);//获取摄像机与背景中心的距离
        if (distance > length + xPos)
        {
            xPos += length;
        }
        else if (distance < xPos - length)
        {
            xPos -= length;
        }
        //使不同背景直接有不同的移动效果
        transform.position = new Vector3(xPos + mainCamera.position.x * offset, transform.position.y, transform.position.z);
    }
}
