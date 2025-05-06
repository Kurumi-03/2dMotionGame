using UnityEngine;

public class AudioManager : MonoBehaviour, ISave
{
    public AudioController playerAC;
    public AudioController BGMAC;
    public AudioController SystemAC;
    public static AudioManager Instance;

    [SerializeField] VolumeSlider[] volumeSettings;
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
    }

    void Start()
    {
        Init();
    }
    void Init()
    {
        //设置初始音量  初始进入游戏才需要
        if (GameManager.Instance.firstGame == true)
        {
            for (int i = 0; i < volumeSettings.Length; i++)
            {
                volumeSettings[i].LoadSliderValue(1);
            }
        }
        //初始需要播放bgm
        BGMAC.LoopAudioPlay(0);
    }

    public void SaveData(ref GameData data)
    {
        //存储前先清空
        data.volumeSettings.Clear();
        for (int i = 0; i < volumeSettings.Length; i++)
        {
            float temp = volumeSettings[i].slider.value;
            if (temp <= 0.001f)
            {
                temp = 0.001f;
            }
            data.volumeSettings.Add(volumeSettings[i].parameter, temp);
        }
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < volumeSettings.Length; i++)
        {
            if (data.volumeSettings.TryGetValue(volumeSettings[i].parameter, out float value))
            {
                volumeSettings[i].LoadSliderValue(value);
            }
        }
    }
}
