using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISave
{
    public static GameManager Instance;
    [Header("Base")]
    public bool firstGame;//是否第一次进入游戏
    [Header("CheckPoint")]
    public CheckPoint[] checkPoints;//游戏场景内所有保存点
    [SerializeField] float bornYOffset;//出生点y偏移

    [Header("Lost")]
    public int lostCurrency;//本轮游戏应该掉落的钱币 
    [SerializeField] GameObject lostPrefab;//玩家上一轮丢失钱币
    [Header("Setting")]
    public bool isShowPlayerHealth;//是否显示玩家血条
    [Header("Tip")]
    public GameTip gameTip;//游戏提示
    public List<string> dropTips;//掉落提示
    public List<string> currencyTips;//钱币提示
    public List<string> saveTips;//技能提示
    public List<string> craftTips;//技能提示
    public List<string> dataTips;//数据提示
    public List<string> skillTips;//技能提示
    void GetInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
        }
    }
    
    void Awake()
    {
        GetInstance();
        GetAllCheckPoint();
    }
    

    void Start()
    {
        gameTip.gameObject.SetActive(false);
    }

    void GetAllCheckPoint()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();
    }

    //重新开始加载游戏场景即重新开始游戏
    public void RestartGame()
    {
        //在重新加载场景时需要保存数据
        SaveManager.Instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //游戏是否暂停
    public void GamePause(bool isPause)
    {
        if(isPause){
            Time.timeScale = 0;
        }
        else{
            Time.timeScale = 1;
        }
    }

    //游戏退出
    public void GameExit()
    {
        //在退出游戏时需要保存数据
        SaveManager.Instance.SaveGame();
        Application.Quit();
    }

    public void SaveData(ref GameData data)
    {
        //使用前清空
        data.checkPoints.Clear();
        int maxSort = 0;
        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (checkPoints[i].active)
            {
                //存储所以激活的检查点
                data.checkPoints.Add(checkPoints[i].id, true);
                //得到玩家激活的最远的检查点
                if (checkPoints[i].sort > maxSort)
                {
                    maxSort = checkPoints[i].sort;
                    data.checkPointID = checkPoints[i].id;
                }
            }
        }
        //存储玩家钱币
        data.lostCurrency = lostCurrency;
        data.lostPosX = PlayerManager.Instance.player.transform.position.x;
        data.LostPosY = PlayerManager.Instance.player.transform.position.y;
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < checkPoints.Length; i++)
        {
            //激活所有已经激活的检查点
            foreach (var checkPoint in data.checkPoints)
            {
                if (checkPoint.Key == checkPoints[i].id && checkPoint.Value == true)
                {
                    checkPoints[i].Active();
                }
            }
            //加载玩家激活的最远的检查点
            if (checkPoints[i].id == data.checkPointID)
            {
                PlayerManager.Instance.player.transform.position =
                    new Vector3(checkPoints[i].transform.position.x, checkPoints[i].transform.position.y + bornYOffset);
            }
        }
        //加载玩家丢失钱币的位置
        if (data.lostCurrency > 0)
        {
            GameObject lost = Instantiate(lostPrefab, new Vector3(data.lostPosX, data.LostPosY), Quaternion.identity);
            lost.GetComponent<LostCurrency>().lost = data.lostCurrency;
        }
    }
}
