using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTipController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI desText;
    [SerializeField] TextMeshProUGUI costText;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowTip(string name, string des, int cost)
    {
        gameObject.SetActive(true);
        nameText.text = name;
        desText.text = des;
        costText.text = "Cost: " + cost.ToString();
    }

    public void CloseTip()
    {
        gameObject.SetActive(false);
    }
}
