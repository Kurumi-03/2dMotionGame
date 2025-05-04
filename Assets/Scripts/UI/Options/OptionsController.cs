using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Toggle playerHealth;//选项面板

    public void ShowPlayerHealth()
    {
        GameManager.Instance.isShowPlayerHealth = playerHealth.isOn;
        PlayerManager.Instance.player.hpUI.SetActive(playerHealth.isOn);
    }
}
