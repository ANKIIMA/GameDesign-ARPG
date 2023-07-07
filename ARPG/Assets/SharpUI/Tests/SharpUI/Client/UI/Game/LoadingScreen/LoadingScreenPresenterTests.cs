using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.Game.LoadingScreen;
using SharpUI.Source.Common.Util.Reactive;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.Game.LoadingScreen
{
    public class LoadingScreenPresenterTests
    {
        private const string CharacterSelectSceneName = "CharacterSelectSceneName";
        private const string GamePlaygroundSceneName = "GamePlaygroundSceneName";
        
        private ILoadingScreenModel _model;
        private LoadingScreenPresenter _presenter;
        private ILoadingScreenComponent _component;
        private IDelayObserver _delayObserver;

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<ILoadingScreenModel>();
            _model.GetCharacterSelectScene().Returns(Observable.Return(CharacterSelectSceneName));
            _model.GetGamePlaygroundScene().Returns(Observable.Return(GamePlaygroundSceneName));
         
            _delayObserver = Substitute.For<IDelayObserver>();
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .Returns(Observable.Return((long) 1));
            
            _component = Substitute.For<ILoadingScreenComponent>();
            _presenter = new LoadingScreenPresenter(_model, _delayObserver);
            _presenter.TakeComponent(_component);
        }

        [Test]
        public void LoadingScreenPresenter_CanCreateEmptyConstructor()
        {
            _presenter = new LoadingScreenPresenter();
        }
        
        [Test]
        public void SimulateLoadingBar_WillUpdateProgress()
        {
            _presenter.SimulateLoadingBar();
            
            _component.Received().UpdateLoadingPercentage(Arg.Any<float>());
        }

        [Test]
        public void SimulateLoadingBar_WillShowGamePlaygroundScene()
        {
            _presenter.SimulateLoadingBar();
            
            _component.Received().ShowScene(GamePlaygroundSceneName, Arg.Any<Action>());
        }

        [Test]
        public void OnBack_WillShowCharacterSelectScene()
        {
            _presenter.OnBack();

            _component.Received().ShowScene(CharacterSelectSceneName, Arg.Any<Action>());
        }
    }
}