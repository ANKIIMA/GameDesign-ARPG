using System.Collections.Generic;
using UnityEngine;

public enum SoundAssetsType
{
    hit,
    swordWave,
    hSwordWave
}

[CreateAssetMenu(fileName = "New SoundAssets",menuName = "CreataAssets/Sound")]
public class GameSoundAssetsSO : ScriptableObject
{

    public List<SoundAssets> assets = new List<SoundAssets>();
    private Dictionary<string, AudioClip[]> assetsDictionary = new Dictionary<string, AudioClip[]>();


    public void InitAssets()
    {
        for (int i = 0; i < assets.Count; i++)
        {
            if (!assetsDictionary.ContainsKey(assets[i].assetaName))
            {
                assetsDictionary.Add(assets[i].assetaName,assets[i].assetsClip);
                Debug.Log(assetsDictionary[assets[i].assetaName].Length);
            }
        }
    }
    
    public AudioClip GetClipAssets(SoundAssetsType soundAssetsType)
    {
        switch (soundAssetsType)
        {
            case SoundAssetsType.hit:
                return assetsDictionary["Hit"][Random.Range(0, assetsDictionary["Hit"].Length)];
            case SoundAssetsType.swordWave:
                return assetsDictionary["SwordWave"][Random.Range(0, assetsDictionary["Hit"].Length)];
            case SoundAssetsType.hSwordWave:
                return assetsDictionary["HSwordWave"][Random.Range(0, assetsDictionary["Hit"].Length)];
            default:
                Debug.Log("没找到");
                return null;
        }
    }

    
    
    
    [System.Serializable]
    public class SoundAssets
    {
        public string assetaName;
        public AudioClip[] assetsClip;
    }
}
