using System;
using System.Collections;
using System.Collections.Generic;
using MyUnitTools;
using UnityEngine;

public class GameAssets : SingletonBase<GameAssets>
{
    [SerializeField,Header("资源")] private GameSoundAssetsSO soundAssets;


    private void Awake()
    {
        soundAssets.InitAssets();
    }


    #region 音效或者音乐
    
    /// <summary>
    /// 播放音乐或者音效
    /// </summary>
    /// <param name="audioSource">音源</param>
    /// <param name="soundAssetsType">音效类型</param>
    public void PlaySoundEffect(AudioSource audioSource,SoundAssetsType soundAssetsType)
    {
        audioSource.clip = soundAssets.GetClipAssets(soundAssetsType);
        audioSource.Play();
    }

    
    #endregion

    
    
    
}
