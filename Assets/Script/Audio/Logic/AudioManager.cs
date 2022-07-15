using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [Header("获得道具的音效")]
    public AudioSource getItem;

    [Header("音乐数据库")]
    public SoundDetailsList_SO soundDetailsList;
    public SceneSoundList_SO sceneSoundList;

    [Header("Audio Source")]
    public AudioSource gameSource;
    public AudioSource effectSource;

    private Coroutine soundRoutine;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Snapsehots")]
    public AudioMixerSnapshot normalSnapShot;
    public AudioMixerSnapshot musicMute;
    public AudioMixerSnapshot masterMute;
    private float musicTransitionSecond = 2f;

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.FinishSceneLoadedEvent += OnFinishSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.FinishSceneLoadedEvent -= OnFinishSceneLoadedEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
         masterMute.TransitionTo(2);
    }

    private void OnFinishSceneLoadedEvent()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        SceneSoundItem sceneSound = sceneSoundList.GetSceneSoundItem(currentScene);
        if (sceneSound == null)
        {
            normalSnapShot.TransitionTo(0);
            return;
        }
        SoundDetails soundDetails = soundDetailsList.GetSoundDetails(sceneSound.music);
        // 场景音乐和当前播放音乐一致则跳过重新播放和恢复静音状态
        if(soundDetails.soundCilp == gameSource.clip)
        {
            normalSnapShot.TransitionTo(0);
            return;
        }
        if (soundRoutine != null)
            StopCoroutine(soundRoutine);
        
        soundRoutine = StartCoroutine(PlaySoundRoutine(soundDetails));

        
    }
    private IEnumerator PlaySoundRoutine(SoundDetails music)
    {   
        

        if (music != null)
        {
            yield return new WaitForSeconds(0);
            PlaySoundClip(music, musicTransitionSecond);
        }
        
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="soundDetails">音乐源</param>
    private void PlaySoundClip(SoundDetails soundDetails, float transitionTime)
    {
        audioMixer.SetFloat("MusicVolume", ConvertSoundVolume(soundDetails.soundVolume));
        gameSource.clip = soundDetails.soundCilp;
        gameSource.volume = soundDetails.soundVolume;
        if (gameSource.isActiveAndEnabled)
            gameSource.Play();

        normalSnapShot.TransitionTo(transitionTime);
    }

    
    //* 播放特殊音效
    public void PlayEffectSound(SoundName soundName)
    {
        SoundDetails soundDetails = soundDetailsList.GetSoundDetails(soundName);

        StartCoroutine(PlayEffectSoundRoutine(soundDetails));
    }
    private IEnumerator PlayEffectSoundRoutine(SoundDetails music)
    {
        if (music != null)
        {
            yield return new WaitForSeconds(0);
            PlayEffectSoundClip(music);
        }
    }
    /// <summary>
    /// 播放特效音乐
    /// </summary>
    /// <param name="soundDetails">音乐源</param>
    private void PlayEffectSoundClip(SoundDetails soundDetails)
    {
        audioMixer.SetFloat("EffectVolume", ConvertSoundVolume(soundDetails.soundVolume));
        effectSource.clip = soundDetails.soundCilp;
        effectSource.volume = soundDetails.soundVolume;
        if (effectSource.isActiveAndEnabled)
            effectSource.Play();
        normalSnapShot.TransitionTo(0);
    }



    /// <summary>
    /// 将Source中0~1的volume转换成在Mixer -80~20的值
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    private float ConvertSoundVolume(float amount)
    {
        return (amount * 100 - 80);
    }
}
