using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] List<AudioClip> oneceAudios = new List<AudioClip>();//只播放一次的音效
    [SerializeField] List<AudioClip> loopAudios = new List<AudioClip>();//循环播放的音效

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OneceAudioPlay(int index,bool isRandom = false)
    {
        if(isRandom){
            index = Random.Range(0, oneceAudios.Count);
        }
        audioSource.clip = oneceAudios[index];
        audioSource.loop = false;
        audioSource.Play();
    }

    public void LoopAudioPlay(int index,bool isRandom = false)
    {
        if(isRandom){
            index = Random.Range(0, loopAudios.Count);
        }
        audioSource.clip = loopAudios[index];
        audioSource.loop = true;
        audioSource.Play();
    }

    public void AudioStop()
    {
        audioSource.Stop();
    }
}
