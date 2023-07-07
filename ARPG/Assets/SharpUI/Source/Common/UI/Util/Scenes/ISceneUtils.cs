using System;
using System.Collections;

namespace SharpUI.Source.Common.UI.Util.Scenes
{
    public interface ISceneUtils
    {
        IEnumerator LoadSceneAsync(string sceneName, Action onCompleteAction = null);
        IEnumerator LoadSceneAdditiveAsync(string sceneName, Action onCompleteAction = null);
        IEnumerator UnloadSceneAsync(string sceneName, Action onCompleteAction = null);
    }
}