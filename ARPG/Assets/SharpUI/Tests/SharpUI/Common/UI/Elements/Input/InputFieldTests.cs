using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.UI.Elements.Input;
using SharpUI.Source.Common.UI.Elements.State;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Input
{
    public class InputFieldTests
    {
        private IElementState _state;
        private InputField _inputField;
        private GameObject _gameObject;
        
        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _inputField = _gameObject.AddComponent<InputField>();
            _inputField.isClickable = true;
            _inputField.isSelectable = true;
            _inputField.isSelected = false;
            _inputField.Awake();

            _state = _inputField.GetState();
        }

        [Test]
        public void GetEventDispatcher_WillReturnDispatcher()
        {
            var dispatcher = _inputField.GetEventDispatcher();
            
            Assert.IsNotNull(dispatcher);
        }

        [Test]
        public void Awake_WillLoadDecorators()
        {
            _gameObject.AddComponent<ColorDecoratorForImage>();
            _inputField.Awake();
            
            var decoratorCount = _inputField.GetDecorators().Count;
            
            Assert.AreEqual(1, decoratorCount);
        }
        
        [Test]
        public void GetEventListener_WillReturnEventListener()
        {
            var listener = _inputField.GetEventListener();
            
            Assert.IsNotNull(listener);
        }
        
        [Test]
        public void SetClickable_ToTrue_WillPromoteStateToClickable()
        {
            _inputField.SetClickable(true);
            
            Assert.IsTrue(_state.IsClickable());
        }
        
        [Test]
        public void SetClickable_ToFalse_WillPromoteStateToNonClickable()
        {
            _inputField.SetClickable(false);
            
            Assert.IsTrue(_state.IsNotClickable());
        }

        [Test]
        public void SetSelectable_ToTrue_WillPromoteStateToSelectable()
        {
            _inputField.SetSelectable(true);
            
            Assert.IsTrue(_state.IsSelectable());
        }
        
        [Test]
        public void SetSelectable_ToFalseWillPromoteStateToNonSelectable()
        {
            _inputField.SetSelectable(false);
            
            Assert.IsTrue(_state.IsNonSelectable());
        }

        [Test]
        public void SetSelected_ToTrue_WhenIsSelectable_WillPromoteStateToSelected()
        {
            _inputField.SetSelectable(true);
            
            _inputField.SetSelected(true);
            
            Assert.IsTrue(_state.IsSelected());
        }
        
        [Test]
        public void SetSelected_ToFalse_WhenSsSelectable_WillPromoteStateToDeselected()
        {
            _inputField.SetSelectable(true);
            
            _inputField.SetSelected(false);
            
            Assert.IsTrue(_state.IsDeselected());
        }

        [Test]
        public void Start_WhenSelected_CanObserve()
        {
            _inputField = new GameObject().AddComponent<InputField>();
            _inputField.isClickable = true;
            _inputField.isSelectable = true;
            _inputField.isSelected = true;
            _inputField.Awake();
            _inputField.Start();
        }
    }
}