using UnityEngine;

namespace Michsky.UI.Frost
{
    public class LoadingStyle : MonoBehaviour
    {
        public void SetStyle(string prefabToLoad)
        {
            Michsky.UI.Frost.LoadingScreen.prefabName = prefabToLoad;
        }
    }
}