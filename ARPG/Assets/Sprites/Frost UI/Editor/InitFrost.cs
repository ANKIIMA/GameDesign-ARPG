using UnityEngine;
using UnityEditor;

public class InitFrost : MonoBehaviour
{
    [InitializeOnLoad]
    public class InitOnLoad
    {
        static InitOnLoad()
        {
            if (!EditorPrefs.HasKey("FrostUI.Installed"))
            {
                EditorPrefs.SetInt("FrostUI.Installed", 1);
                EditorUtility.DisplayDialog("Hello there!", "Thank you for purchasing Frost UI.\r\rFirst of all, import TextMesh Pro from Package Manager if you haven't already.\r\rYou can check Documentation file for help, or contact me at isa.steam@outlook.com", "Got it!");
            }
        }
    }
}