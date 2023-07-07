using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace SharpUI.Source.Common.UI.Util.Scenes
{
    public class SceneUtils : ISceneUtils
    {
        public IEnumerator LoadSceneAsync(string sceneName, Action onCompleteAction = null)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.completed += operation => onCompleteAction?.Invoke();

            while (!asyncLoad.isDone)
                yield return null;
        }

        public IEnumerator LoadSceneAdditiveAsync(string sceneName, Action onCompleteAction = null)
        {
            var newSceneAsyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            newSceneAsyncLoad.completed += operation => onCompleteAction?.Invoke();
            
            while (!newSceneAsyncLoad.isDone)
                yield return null;
        }

        public IEnumerator UnloadSceneAsync(string sceneName, Action onCompleteAction = null)
        {
            var asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
            asyncUnload.completed += operation => onCompleteAction?.Invoke();
            
            while (!asyncUnload.isDone)
                yield return null;
        }
    }
}