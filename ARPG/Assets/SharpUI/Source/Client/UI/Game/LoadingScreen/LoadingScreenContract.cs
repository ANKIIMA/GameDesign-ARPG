using System;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;

namespace SharpUI.Source.Client.UI.Game.LoadingScreen
{
    public interface ILoadingScreenComponent : IBaseComponent
    {
        void UpdateLoadingPercentage(float percentage);
    }

    public interface ILoadingScreenPresenter
    {
        void OnBack();
        void SimulateLoadingBar();
    }

    public interface ILoadingScreenModel : IBaseModel
    {
        IObservable<string> GetCharacterSelectScene();
        IObservable<string> GetGamePlaygroundScene();
    }
}