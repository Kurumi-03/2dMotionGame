using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopTip : MonoBehaviour
{
    [SerializeField] GameObject tipPrefab;
    [SerializeField] float duringTime;
    [SerializeField] float showTime;
    [SerializeField] float moveSpeed;
    [SerializeField] float offsetX;
    [SerializeField] float offsetY;

    public void CreateTip(string text, Vector3 pos)
    {
        float randomOfesetX = Random.Range(-offsetX, offsetX);
        float randomOfesetY = Random.Range(-offsetY, offsetY);
        GameObject tip = Instantiate(tipPrefab, pos + new Vector3(randomOfesetX, randomOfesetY), Quaternion.identity,transform);
        tip.GetComponent<PopTipText>().SetUpData(text, showTime, duringTime, moveSpeed);
    }
}
