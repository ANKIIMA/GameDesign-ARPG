using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Util.Event;
using SharpUI.Source.Common.UI.Util.Keyboard;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Keyboard
{
    public class SimpleKeyListenerTests
    {
        private CompositeDisposable _disposable = new CompositeDisposable();
        private GameObject _gameObject;
        private IKeyInputState _state;
        private ICurrentGameObjectProvider _currentGameObjectProvider;
        private SimpleKeyListener _keyListener;
        private BaseButton _baseButton;
        private bool allKeysDown;
        private bool allKeysUp;
        private GameObject _currentGameObject;

        [SetUp]
        public void SetUp()
        {
            allKeysDown = false;
            allKeysUp = false;

            _currentGameObject = new GameObject();
            
            _baseButton = new GameObject().AddComponent<BaseButton>();
            _baseButton.isClickable = true;
            _baseButton.Awake();
            _baseButton.Start();

            _state = Substitute.For<IKeyInputState>();
            _currentGameObjectProvider = Substitute.For<ICurrentGameObjectProvider>();
            _gameObject = new GameObject();
            _keyListener = _gameObject.AddComponent<SimpleKeyListener>();
            
            _keyListener.TakeButton(_baseButton);
            _keyListener.ObserveDown().SubscribeWith(_disposable, _ => allKeysDown = true);
            _keyListener.ObserveUp().SubscribeWith(_disposable, _ => allKeysUp = true);
            _keyListener.SetKeyInputState(_state);
            _keyListener.SetCurrentGameObjectProvider(_currentGameObjectProvider);
            _currentGameObjectProvider.GetCurrentSelectedGameObject().Returns(_currentGameObject);
        }

        [Test]
        public void WhenKeyPressed_WillBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysDown);
        }
        
        [Test]
        public void WhenKeyReleased_WillBeUp()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _state.IsKeyReleased(KeyCode.A).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysUp);
        }

        [Test]
        public void RequireAnyShift_ShiftNotPressed_WillNotBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsFalse(allKeysDown);
        }
        
        [Test]
        public void RequireAnyShift_LeftShiftPressed_WillBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.LeftShift).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysDown);
        }
        
        [Test]
        public void RequireAnyShift_RightShiftPressed_WillBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.RightShift).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysDown);
        }
        
        [Test]
        public void RequireAnyShift_False_RightShiftPressed_WillNotBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.RightShift).Returns(true);
            
            _keyListener.RequireAnyShift(false);
            _keyListener.Update();
            
            Assert.IsFalse(allKeysDown);
        }
        
        [Test]
        public void RequireAnyControl_ControlNotPressed_WillNotBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsFalse(allKeysDown);
        }
        
        [Test]
        public void RequireAnyControl_False_ControlPressed_WillNotBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.RightControl).Returns(true);
            
            _keyListener.RequireAnyControl(false);
            _keyListener.Update();
            
            Assert.IsFalse(allKeysDown);
        }
        
        [Test]
        public void RequireAnyControl_LeftControlPressed_WillBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.LeftControl).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysDown);
        }
        
        [Test]
        public void RequireAnyControl_RightControlPressed_WillBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.RightControl).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysDown);
        }

        [Test]
        public void MultipleRequiredPrefixKeys_NonePressed_WillNotBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsFalse(allKeysDown);
        }
        
        [Test]
        public void MultipleRequiredPrefixKeys_OnePressed_WillNotBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.LeftShift).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsFalse(allKeysDown);
        }
        
        [Test]
        public void MultipleRequiredPrefixKeys_AllRightPressed_WillBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.RightShift).Returns(true);
            _state.IsKeyPressed(KeyCode.RightControl).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysDown);
        }
        
        [Test]
        public void MultipleRequiredPrefixKeys_AllLeftPressed_WillBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyPressed(KeyCode.LeftShift).Returns(true);
            _state.IsKeyPressed(KeyCode.LeftControl).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsTrue(allKeysDown);
        }
        
        [Test]
        public void MultipleRequiredPrefixKeys_AllLeftReleased_WillNotBeDown()
        {
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RequireAnyShift();
            _keyListener.RequireAnyControl();
            _state.IsKeyPressed(KeyCode.A).Returns(true);
            _state.IsKeyReleased(KeyCode.LeftShift).Returns(true);
            _state.IsKeyReleased(KeyCode.LeftControl).Returns(true);
            
            _keyListener.Update();
            
            Assert.IsFalse(allKeysDown);
        }

        [Test]
        public void AddFilterTag_WillFilterKey()
        {
            _currentGameObject.tag = SimpleKeyListener.DefaultInputTextTag;
            _keyListener.ListenToKey(KeyCode.A);
            _keyListener.RemoveFilterTag(SimpleKeyListener.DefaultInputTextTag);
            _state.IsKeyReleased(KeyCode.A).Returns(true);
            
            _keyListener.AddFilterTag(SimpleKeyListener.DefaultInputTextTag);
            _keyListener.Update();
            
            Assert.IsFalse(allKeysUp);
        }
        
        [Test]
        public void RemoveFilterTag_WillNotFilterKey()
        {
            _currentGameObject.tag = SimpleKeyListener.DefaultInputTextTag;
            _keyListener.ListenToKey(KeyCode.A);
            _state.IsKeyReleased(KeyCode.A).Returns(true);
            
            _keyListener.RemoveFilterTag(SimpleKeyListener.DefaultInputTextTag);
            _keyListener.Update();
            
            Assert.IsTrue(allKeysUp);
        }
    }
}