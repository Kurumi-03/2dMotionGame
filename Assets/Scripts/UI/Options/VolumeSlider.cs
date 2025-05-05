using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;//音频混合器参数名称
    [SerializeField] AudioMixer audioMixer;//音频混合器
    [SerializeField] float multiplier;//音量倍数

    public void SetVolume(float value)
    {
        //设置音量  log10(value) * multiplier  0-1之间的值转换为音频混合器需要的值
        audioMixer.SetFloat(parameter, Mathf.Log10(value) * multiplier);
    }

    public void LoadSliderValue(float value)
    {
        float temp = value;
        if (temp <= 0.001f)
        {
            temp = 0.001f;
        }
        slider.value = temp;
        SetVolume(temp);
    }
}
