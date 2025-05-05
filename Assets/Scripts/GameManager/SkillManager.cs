using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour, ISave
{
    public static SkillManager Instance;
    public PlayerDash dash { get; private set; }
    public PlayerClone clone { get; private set; }
    public PlayerSowrd sword { get; private set; }
    public PlayerBlackHole blackHole { get; private set; }
    public PlayerCrystal crystal { get; private set; }
    public PlayerParry parry { get; private set; }
    public PlayerDodge dodge { get; private set; }

    public List<SkillSlotController> skillSlots = new List<SkillSlotController>();
    public List<PlayerSkill> skills = new List<PlayerSkill>();

    //单例模式
    private void GetInstance()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    void Awake()
    {
        GetInstance();
        dash = GetComponent<PlayerDash>();
        clone = GetComponent<PlayerClone>();
        sword = GetComponent<PlayerSowrd>();
        blackHole = GetComponent<PlayerBlackHole>();
        crystal = GetComponent<PlayerCrystal>();
        parry = GetComponent<PlayerParry>();
        dodge = GetComponent<PlayerDodge>();
    }

    void OnEnable()
    {
        skills.Add(dash);
        skills.Add(clone);
        skills.Add(sword);
        skills.Add(blackHole);
        skills.Add(crystal);
        skills.Add(parry);
        skills.Add(dodge);
        for (int i = 0; i < skills.Count; i++)
        {
            for (int j = 0; j < skills[i].skillTreeSlots.Count; j++)
            {
                skillSlots.Add(skills[i].skillTreeSlots[j]);
            }
        }
    }

    public void AddAllListener()
    {
        dash.AddUnlockListener();
        clone.AddUnlockListener();
        sword.AddUnlockListener();
        blackHole.AddUnlockListener();
        crystal.AddUnlockListener();
        parry.AddUnlockListener();
        dodge.AddUnlockListener();
    }

    public void ClearAllUnlock()
    {

    }


    public void SaveData(ref GameData data)
    {
        data.skills.Clear();
        for (int i = 0; i < skills.Count; i++)
        {
            for (int j = 0; j < skills[i].skillTreeSlots.Count; j++)
            {
                data.skills.Add(skills[i].skillTreeSlots[j].skillName, skills[i].skillTreeSlots[j].unlock);
            }
        }
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            for (int j = 0; j < skills[i].skillTreeSlots.Count; j++)
            {
                //对比skillname进行赋值
                if (data.skills.TryGetValue(skills[i].skillTreeSlots[j].skillName, out bool unlockData))
                {
                    skills[i].skillTreeSlots[j].unlock = unlockData;
                }
                else
                {
                    GameManager.Instance.gameTip.ShowTip(GameManager.Instance.saveTips[0], GameManager.Instance.saveTips[1]);
                }
            }
        }
    }
}
