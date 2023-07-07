using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Events;
using SharpUI.Source.Common.UI.Elements.State;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Button
{
    public class BaseButtonTests
    {
        private IElementState _state;
        private BaseButton _baseButton;

        [SetUp]
        public void SetUp()
        {
            _baseButton = new GameObject().AddComponent<BaseButton>();
            _baseButton.isClickable = true;
            _baseButton.isSelectable = true;
            _baseButton.isSelected = false;
            _baseButton.Awake();

            _state = _baseButton.GetState();
        }

        [Test]
        public void GetKeyListener_WillCreateNewKeyListener()
        {
            _baseButton.SetKeyListener(null);

            var keyListener = _baseButton.GetKeyListener();
            
            Assert.IsNotNull(keyListener);
        }

        [Test]
        public void GetEventListener_WillReturnEventListener()
        {
            var listener = _baseButton.GetEventListener();
            
            Assert.IsInstanceOf<IElementEventListener>(listener);
        }

        [Test]
        public void SetClickable_ToTrue_WillSetStateClickable()
        {
            _baseButton.SetClickable(true);
            
            Assert.IsTrue(_state.IsClickable());
        }
        
        [Test]
        public void SetClickable_ToFalse_WillSetStateNonClickable()
        {
            _baseButton.SetClickable(false);
            
            Assert.IsTrue(_state.IsNotClickable());
        }

        [Test]
        public void SetSelectable_ToTrue_WillSetStateSelectable()
        {
            _baseButton.SetSelectable(true);
            
            Assert.IsTrue(_state.IsSelectable());
        }
        
        [Test]
        public void SetSelectable_ToFalse_WillSetStateNonSelectable()
        {
            _baseButton.SetSelectable(false);
            
            Assert.IsTrue(_state.IsNonSelectable());
        }

        [Test]
        public void SetSelected_ToTrue_WhenIsSelectable_WillSetStateSelected()
        {
            _baseButton.SetSelectable(true);
            
            _baseButton.SetSelected(true);
            
            Assert.IsTrue(_state.IsSelected());
        }
        
        [Test]
        public void SetSelected_ToFalse_WhenIsSelectable_WillSetStateDeselected()
        {
            _baseButton.SetSelectable(true);
            
            _baseButton.SetSelected(false);
            
            Assert.IsTrue(_state.IsDeselected());
        }

        [Test]
        public void Start_WhenSelected_CanObserve()
        {
            _baseButton = new GameObject().AddComponent<BaseButton>();
            _baseButton.isClickable = true;
            _baseButton.isSelectable = true;
            _baseButton.isSelected = true;
            _baseButton.Awake();
            _baseButton.Start();
        }

        [Test]
        public void EnablePermanentSelection_WillDisableDeselectHandler()
        {
            _baseButton.EnablePermanentSelection();
            
            var button = _baseButton.GetComponent<UnityEngine.UI.Button>();
            var deselectHandler = (IDeselectHandler)button.GetComponent<ObservableDeselectTrigger>();

            Assert.IsNull(deselectHandler);
        }

        [Test]
        public void DisablePermanentSelection_WillHaveDeselectHandler()
        {
            _baseButton.EnablePermanentSelection();
            _baseButton.DisablePermanentSelection();

            var button = _baseButton.GetComponent<UnityEngine.UI.Button>();
            var deselectHandler = (IDeselectHandler)button.GetComponent<ObservableDeselectTrigger>();
            
            Assert.IsNotNull(deselectHandler);
        }
        
        [Test]
        public void DisablePermanentSelection_WhenDeselected_WillBeDeselected()
        {
            _baseButton.EnablePermanentSelection();
            _baseButton.DisablePermanentSelection();
            var button = _baseButton.GetComponent<UnityEngine.UI.Button>();
            var deselectHandler = (IDeselectHandler)button.GetComponent<ObservableDeselectTrigger>();
            
            deselectHandler.OnDeselect(new BaseEventData(EventSystem.current));
            
            Assert.IsTrue(_baseButton.GetState().IsDeselected());
        }
    }
}