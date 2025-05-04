using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    [SerializeField] FadeEffect fadeEffect;
    [SerializeField] float loadTime;
    [SerializeField] string gameScene;
    [SerializeField] GameObject continueBtn;

    void Start()
    {
        //继续游戏按钮的显示与隐藏
        continueBtn.SetActive(SaveManager.Instance.HasDataFile());
    }

    public void NewGame()
    {
        fadeEffect.FadeOut(loadTime, () =>
        {
            SaveManager.Instance.DeleteDataFile();
            SaveManager.Instance.NewGame();
            SceneManager.LoadScene(gameScene);
        });
    }

    public void Continue()
    {
        fadeEffect.FadeOut(loadTime, () =>
        {
            SceneManager.LoadScene(gameScene);
        });
    }

    public void ExitGame()
    {
        Debug.Log("退出游戏");
    }
}
