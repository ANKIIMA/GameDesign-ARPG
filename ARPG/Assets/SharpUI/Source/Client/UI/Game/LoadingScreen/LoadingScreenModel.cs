using System;
using UniRx;

namespace SharpUI.Source.Client.UI.Game.LoadingScreen
{
    public class LoadingScreenModel : ILoadingScreenModel
    {
        private const string CharacterSelectScene = "CharacterSelection";
        private const string GamePlaygroundScene = "GamePlayground";

        public IObservable<string> GetCharacterSelectScene() => Observable.Return(CharacterSelectScene);

        public IObservable<string> GetGamePlaygroundScene() => Observable.Return(GamePlaygroundScene);
    }
}