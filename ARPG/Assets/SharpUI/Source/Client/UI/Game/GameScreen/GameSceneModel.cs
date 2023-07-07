using System;
using UniRx;

namespace SharpUI.Source.Client.UI.Game.GameScreen
{
    public class GameSceneModel : IGameSceneModel
    {
        private const string SettingsSceneName = "Settings";
        private const string SkillTreeSceneName = "SkillTree";
        private const string VendorSceneName = "Vendor";

        public IObservable<string> GetSettingsScene() => Observable.Return(SettingsSceneName);
        
        public IObservable<string> GetSkillTreeScene() => Observable.Return(SkillTreeSceneName);

        public IObservable<string> GetVendorsScene() => Observable.Return(VendorSceneName);
    }
}