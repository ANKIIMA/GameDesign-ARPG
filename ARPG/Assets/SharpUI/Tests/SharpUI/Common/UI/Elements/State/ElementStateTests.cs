using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.State;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.State
{
    public class ElementStateTests
    {
        private ElementState _state;

        [SetUp]
        public void SetUp()
        {
            _state = new ElementState();
        }

        [Test]
        public void Enable_WillSetDefaultStates()
        {
            _state.Enable();
            
            Assert.IsTrue(_state.IsEnabled());
            Assert.IsTrue(_state.IsDeselected());
            Assert.IsTrue(_state.IsReleased());
            Assert.IsTrue(_state.IsUnFocused());
        }

        [Test]
        public void Disable_WillBeDisabled()
        {
            _state.Disable();
            
            Assert.IsTrue(_state.IsDisabled());
        }

        [Test]
        public void Press_WillBePressed()
        {
            _state.Press();
            
            Assert.IsTrue(_state.IsPressed());
        }

        [Test]
        public void Release_WillBeReleased()
        {
            _state.Release();
            
            Assert.IsTrue(_state.IsReleased());
        }

        [Test]
        public void Focus_WillBeFocused()
        {
            _state.Focus();
            
            Assert.IsTrue(_state.IsFocused());
        }
        
        [Test]
        public void UnFocus_WillBeUnfocused()
        {
            _state.UnFocus();
            
            Assert.IsTrue(_state.IsUnFocused());
        }

        [Test]
        public void SelectIfSelectable_WhenSelectable_WillBeSelected()
        {
            _state = new ElementState(true);
            
            _state.SelectIfSelectable();
            
            Assert.IsTrue(_state.IsSelected());
        }
        
        [Test]
        public void SelectIfSelectable_WhenNotSelectable_WillNotBeSelected()
        {
            _state = new ElementState(false);
            
            _state.SelectIfSelectable();
            
            Assert.IsFalse(_state.IsSelected());
        }
        
        [Test]
        public void DeselectIfSelectable_WhenSelectable_WillBeDeselected()
        {
            _state = new ElementState(true);
            
            _state.DeselectIfSelectable();
            
            Assert.IsTrue(_state.IsDeselected());
        }
        
        [Test]
        public void DeselectIfSelectable_WhenNotSelectable_WillNotBeDeselected()
        {
            _state = new ElementState(false, true, true, true);
            
            _state.DeselectIfSelectable();
            
            Assert.IsFalse(_state.IsDeselected());
        }

        [Test]
        public void MakeSelectable_WillBeSelectable()
        {
            _state.MakeSelectable();
            
            Assert.IsTrue(_state.IsSelectable());
        }

        [Test]
        public void MakeNonSelectable_WillBeNonSelectable()
        {
            _state.MakeNonSelectable();
            
            Assert.IsTrue(_state.IsNonSelectable());
        }

        [Test]
        public void MakeClickable_WillBeClickable()
        {
            _state.MakeClickable();
            
            Assert.IsTrue(_state.IsClickable());
        }
        
        [Test]
        public void MakeNonClickable_WillBeNotClickable()
        {
            _state.MakeNonClickable();
            
            Assert.IsTrue(_state.IsNotClickable());
        }
    }
}
