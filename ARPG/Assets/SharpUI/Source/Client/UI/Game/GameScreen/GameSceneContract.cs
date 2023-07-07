using System;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;

namespace SharpUI.Source.Client.UI.Game.GameScreen
{
    public interface IGameSceneComponent : IBaseComponent
    {
    }

    public interface IGameScenePresenter
    {
        void OnSettingsClicked();
        void OnVendorClicked();
        void OnSkillsClicked();
    }

    public interface IGameSceneModel : IBaseModel
    {
        IObservable<string> GetSkillTreeScene();
        IObservable<string> GetSettingsScene();
        IObservable<string> GetVendorsScene();
    }
}