using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISave
{
    void SaveData(ref GameData data);
    void LoadData(GameData data);
}
