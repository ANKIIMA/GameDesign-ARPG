using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Presenter;
using UniRx;

namespace SharpUI.Tests.SharpUI.Common.UI.Base.Presenter
{
    public class BasePresenterTests
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private IBaseComponent _component;
        private FakePresenter _presenter;

        private const string SceneName = "SomeScene";

        private class FakePresenter : BasePresenter<IBaseComponent>
        {
            public FakePresenter(CompositeDisposable disposable) : base(disposable) { }

            public void ShowSomeScene()
            {
                ShowScene(SceneName);
            }

            public void HideSomeScene()
            {
                HideScene(SceneName);
            }

            public void ShowSceneOnComponent()
            {
                OnComponent(component => component.ShowScene(SceneName, null));
            }
        }

        [SetUp]
        public void SetUp()
        {
            _presenter = new FakePresenter(_disposables);
            _component = Substitute.For<IBaseComponent>();
        }
        
        [Test]
        public void OnComponentStarted_WillSetupComponent()
        {
            _presenter.TakeComponent(_component);
            _presenter.OnComponentStarted();
            
            _component.Received().SetupComponent();
        }

        [Test]
        public void OnComponentDestroyed_WillDisposeAll()
        {
            _presenter.OnComponentDestroyed();
            
            Assert.IsTrue(_disposables.IsDisposed);
        }

        [Test]
        public void DropComponent_WillNotSetupComponent()
        {
            _presenter.TakeComponent(_component);
            
            _presenter.DropComponent();
            _presenter.OnComponentStarted();
            
            _component.DidNotReceive().SetupComponent();
        }

        [Test]
        public void HideScene_WillHideScene()
        {
            var fakePresenter = new FakePresenter(_disposables);
            fakePresenter.TakeComponent(_component);
            
            fakePresenter.HideSomeScene();

            _component.Received().HideScene(SceneName, null);
        }

        [Test]
        public void ShowScene_WillShowScene()
        {
            var fakePresenter = new FakePresenter(_disposables);
            fakePresenter.TakeComponent(_component);
            
            fakePresenter.ShowSomeScene();

            _component.Received().ShowScene(SceneName, null);
        }

        [Test]
        public void OnComponent_WillWorkCorrectly()
        {
            var fakePresenter = new FakePresenter(_disposables);
            fakePresenter.TakeComponent(_component);
            
            fakePresenter.ShowSceneOnComponent();

            _component.Received().ShowScene(SceneName, Arg.Any<Action>());
        }
    }
}