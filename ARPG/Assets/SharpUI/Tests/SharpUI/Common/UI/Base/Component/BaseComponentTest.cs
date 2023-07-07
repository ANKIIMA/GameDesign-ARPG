using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Presenter;

namespace SharpUI.Tests.SharpUI.Common.UI.Base.Component
{
    public class BaseComponentTest
    {
        public class FakeComponent : BaseComponent<FakePresenter, FakeComponent>, IBaseComponent
        {
            public void SetupComponent() { }
            public void ShowScene(string sceneName, Action onSceneLoadComplete) { }
            public void ShowSceneAdditive(string sceneName, Action onSceneLoadComplete) { }
            public void HideScene(string sceneName, Action onSceneUnloadComplete) { }
        }

        public class FakePresenter : BasePresenter<FakeComponent> { }

        private BaseComponent<FakePresenter, FakeComponent> _baseComponent;
        private FakePresenter _fakePresenter;
        private FakeComponent _fakeComponent;

        [SetUp]
        public void SetUp()
        {
            _fakePresenter = Substitute.For<FakePresenter>();
            _fakeComponent = new FakeComponent();
            _baseComponent = new BaseComponent<FakePresenter, FakeComponent>(_fakePresenter);
        }

        [Test]
        public void GetPresenter_WillReturnCorrectPresenter()
        {
            var presenter = _baseComponent.GetPresenter();
            
            Assert.IsInstanceOf<FakePresenter>(presenter);
        }

        [Test]
        public void OnAwake_WillTakeComponent()
        {
            _baseComponent.OnAwake(_fakeComponent);
            
            _fakePresenter.Received().TakeComponent(_fakeComponent);
        }

        [Test]
        public void OnStart_WillNotifyPresenter()
        {
            _baseComponent.OnStart();
            
            _fakePresenter.Received().OnComponentStarted();
        }

        [Test]
        public void OnDisable_WillDropComponent()
        {
            _baseComponent.OnDisable();
            
            _fakePresenter.Received().DropComponent();
        }

        [Test]
        public void OnDestroy_WillInformPresenter()
        {
            _baseComponent.OnDestroy();
            
            _fakePresenter.Received().OnComponentDestroyed();
        }
    }
}
