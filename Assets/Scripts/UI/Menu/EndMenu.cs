using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Button btn;
    [SerializeField] FadeEffect fadeEffect;

    void Start()
    {
        fadeEffect.gameObject.SetActive(true);
        btn.onClick.AddListener(() =>
        {
            GameManager.Instance.RestartGame();
        });
        btn.gameObject.SetActive(false);
        txt.gameObject.SetActive(false);
        txt.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0);
    }

    public void GameEnd()
    {
        txt.gameObject.SetActive(true);
        txt.DOFade(1, 2).OnComplete(() =>
        {
            btn.gameObject.SetActive(true);
        });
    }
}
