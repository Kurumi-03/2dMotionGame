using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] List<Tkey> ids = new List<Tkey>();
    [SerializeField] List<TValue> values = new List<TValue>();
    public void OnBeforeSerialize()
    {
        //先确保数据正确
        ids.Clear();
        values.Clear();
        foreach (KeyValuePair<Tkey, TValue> pair in this)
        {
            ids.Add(pair.Key);
            values.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        if (ids.Count != values.Count)
        {
            GameManager.Instance.gameTip.ShowTip(GameManager.Instance.dataTips[0], GameManager.Instance.dataTips[1]);
            Debug.LogError("DataDictionary序列化出错");
        }
        for (int i = 0; i < ids.Count; i++)
        {
            this.Add(ids[i], values[i]);
        }
    }

}
