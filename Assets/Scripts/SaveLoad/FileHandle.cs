using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileHandle
{
    //初始为空字符串而不是null
    string fileName = "";
    string filePath = "";
    string codeWord;//加密使用的基准词
    bool encryption;//是否加密

    public FileHandle(string _filePath,string _fileName,bool _encryption,string _codeWord)
    {
        filePath = _filePath;
        fileName = _fileName;
        encryption = _encryption;
        codeWord = _codeWord;
    }

    public void Save(GameData data)
    {
        //存储路径和文件名合一才是完整的文件路径
        string fullPath = Path.Combine(filePath, fileName);
        try
        {
            //确保路径存在
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string jsonData = JsonUtility.ToJson(data, true);
            //加密
            if (encryption)
            {
                jsonData = EncryptDecrypt(jsonData);
            }
            //使用using即可使用完后就释放资源
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonData);
                }
            }
        }
        catch (System.Exception e)
        {
            GameManager.Instance.gameTip.ShowTip(GameManager.Instance.dataTips[0], GameManager.Instance.dataTips[1]);
            Debug.Log(e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(filePath, fileName);
        //首先判断路径是否存在
        GameData data = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string jsonData = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonData = reader.ReadToEnd();
                    }
                }
                //加密
                if (encryption)
                {
                    jsonData = EncryptDecrypt(jsonData);
                }
                data = JsonUtility.FromJson<GameData>(jsonData);
            }
            catch (System.Exception e)
            {
                GameManager.Instance.gameTip.ShowTip(GameManager.Instance.dataTips[0], GameManager.Instance.dataTips[1]);
                Debug.Log(e);
            }
        }
        return data;
    }

    public void Delete()
    {
        string path = Path.Combine(filePath, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.Log("删除文件出错，或文件不存在");
        }
    }

    //加密和解密使用
    public string EncryptDecrypt(string s)
    {
        string result = "";
        for (int i = 0; i < s.Length; i++)
        {
            //此处要转为字符  不然会显示为字符的ascll码
            result += (char)(s[i] ^ codeWord[i % codeWord.Length]);
        }
        return result;
    }
}
