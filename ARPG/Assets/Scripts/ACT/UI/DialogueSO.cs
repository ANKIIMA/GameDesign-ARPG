using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueAssets
{
    public string DialogueName;
    public TextAsset text;
}

[System.Serializable]
public class AvatarAssets
{
    public string AvatarName;
    public Sprite AvatarImage;
}

[CreateAssetMenu(fileName = "New DialogueAssets", menuName = "CreataAssets/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public List<DialogueAssets> textFiles;
    public List<AvatarAssets> imageFiles;

    public TextAsset GetTextAsset(string dialogueName)
    {
        foreach(var asset in textFiles)
        {
            if(asset.DialogueName == dialogueName)
            {
                return asset.text;
            }
        }

        return null;
    }
}
