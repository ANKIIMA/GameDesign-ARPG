using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.Frost
{
    public class LoadScene : MonoBehaviour
    {
        public void ChangeToScene(string sceneName)
        {
            Michsky.UI.Frost.LoadingScreen.LoadScene(sceneName);
        }
    }
}