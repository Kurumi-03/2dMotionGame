using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] TextMeshProUGUI tip;

    public void ShowTip(string headerText, string tipText)
    {
        gameObject.SetActive(true);
        header.text = headerText;
        tip.text = tipText;
    }

    public void HideTip()
    {
        gameObject.SetActive(false);
    }

}
