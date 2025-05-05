using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [SerializeField] string fileName;
    public string assetDataPath;//存储数据文件的路径 Asset起
    [SerializeField] bool encryption;
    [SerializeField] string codeWord;
    List<ISave> saves = new List<ISave>();
    GameData data;
    FileHandle handle;

    void Awake()
    {
        GetInstance();
    }

    void Start()
    {
        handle = new FileHandle(Application.persistentDataPath, fileName, encryption, codeWord);
        saves = FindAllSaves();
        LoadGame();
    }

    void GetInstance()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void SaveGame()
    {
        // Debug.Log("SaveGame");
        for (int i = 0; i < saves.Count; i++)
        {
            saves[i].SaveData(ref data);
        }
        handle.Save(data);
    }

    public void LoadGame()
    {
        //没有存储数据即第一次进入游戏需要创建数据
        data = handle.Load();
        if (data == null)
        {
            NewGame();
            return;
        }
        for (int i = 0; i < saves.Count; i++)
        {
            saves[i].LoadData(data);
        }
    }

    public void NewGame()
    {
        data = new GameData();
        GameManager.Instance.firstGame = true;
    }

    List<ISave> FindAllSaves()
    {
        IEnumerable<ISave> iSaves = FindObjectsOfType<MonoBehaviour>().OfType<ISave>();
        return new List<ISave>(iSaves);

    }

    [ContextMenu("Delete Data File")]
    //在unity编辑器内删除存储的数据文件
    public void DeleteDataFile()
    {
        FileHandle temp = new FileHandle(Application.persistentDataPath, fileName, encryption, codeWord);
        temp.Delete();
    }

    //判断是否有数据文件存在本地
    public bool HasDataFile()
    {
        return handle.Load() != null;
    }

    //关闭游戏进程时
    void OnApplicationQuit()
    {
        SaveGame();
    }
}
