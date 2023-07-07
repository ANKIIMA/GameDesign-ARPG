using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.UI.Util.Scenes;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Base.Component
{
    public class MonoBehaviourComponentTests
    {
        private FakeComponent _component;
        private BaseComponent<FakePresenter, FakeComponent> _baseComponent;

        private const string SceneName = "SceneName";
        
        public class FakeComponent : MonoBehaviourComponent<FakeComponent, FakePresenter>, IBaseComponent
        {
            public void SetupComponent() { }
            
            protected override FakeComponent GetComponent() => this;

            public BasePresenter<FakeComponent> GetFakePresenter()
            {
                return GetPresenter();
            }
        }

        public class FakePresenter : BasePresenter<FakeComponent> { }

        [SetUp]
        public void SetUp()
        {
            _baseComponent = Substitute.For<BaseComponent<FakePresenter, FakeComponent>>();
            _component = new GameObject().AddComponent<FakeComponent>();
            _component.SetupComponent();
            _component.SetComponent(_baseComponent);
        }

        [Test]
        public void Awake_WillHaveCorrectPresenter()
        {
            _component.Awake();

            var presenter = _component.GetFakePresenter();
            
            Assert.IsInstanceOf<FakePresenter>(presenter);
        }

        [Test]
        public void Awake_WillAwakeBaseComponent()
        {
            _component.Awake();
            
            _baseComponent.Received().OnAwake(_component);
        }
        
        [Test]
        public void Start_WillStartBaseComponent()
        {
            _component.Awake();
            
            _component.Start();
            
            _baseComponent.Received().OnStart();
        }
        
        [Test]
        public void OnDisable_WillDisableBaseComponent()
        {
            _component.Awake();
            _component.Start();
            
            _component.OnDisable();
            
            _baseComponent.Received().OnDisable();
        }
        
        [Test]
        public void OnDestroy_WillDestroyBaseComponent()
        {
            _component.Awake();
            _component.Start();
            
            _component.OnDestroy();
            
            _baseComponent.Received().OnDestroy();
        }

        [Test]
        public void ShowScene_WillShowScene()
        {
            var sceneUtils = Substitute.For<ISceneUtils>();
            _component.Awake();
            _component.Start();
            _component.SetSceneUtils(sceneUtils);
            
            _component.ShowScene(SceneName);

            sceneUtils.Received().LoadSceneAsync(SceneName, Arg.Any<Action>());
        }
        
        [Test]
        public void ShowSceneAdditive_WillShowSceneAdditive()
        {
            var sceneUtils = Substitute.For<ISceneUtils>();
            _component.Awake();
            _component.Start();
            _component.SetSceneUtils(sceneUtils);
            
            _component.ShowSceneAdditive(SceneName, null);

            sceneUtils.Received().LoadSceneAdditiveAsync(SceneName, Arg.Any<Action>());
        }
        
        [Test]
        public void HideScene_WillHideScene()
        {
            var sceneUtils = Substitute.For<ISceneUtils>();
            _component.Awake();
            _component.Start();
            _component.SetSceneUtils(sceneUtils);
            
            _component.HideScene(SceneName, null);

            sceneUtils.Received().UnloadSceneAsync(SceneName, Arg.Any<Action>());
        }
    }
}