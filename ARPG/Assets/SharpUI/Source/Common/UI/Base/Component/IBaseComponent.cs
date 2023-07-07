using System;

namespace SharpUI.Source.Common.UI.Base.Component
{
    public interface IBaseComponent
    {
        void SetupComponent();

        void ShowScene(string sceneName, Action onSceneLoadComplete);

        void ShowSceneAdditive(string sceneName, Action onSceneLoadComplete);

        void HideScene(string sceneName, Action onSceneUnloadComplete);
    }
}
