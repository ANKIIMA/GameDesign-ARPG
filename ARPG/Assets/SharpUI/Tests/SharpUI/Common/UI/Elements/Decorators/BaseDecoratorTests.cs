using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Decorators
{
    public class BaseDecoratorTests
    {
        private class TestDecorator : BaseDecorator<Color>
        {
            public bool decoratedPressed;
            public bool decoratedReleased;
            public bool decoratedSelected;
            public bool decoratedDeselected;
            public bool decoratedEntered;
            public bool decoratedExited;
            public bool decoratedEnabled;
            public bool decoratedDisabled;

            protected override void Decorate(Color component) { }
            protected override void DecoratePressed() => decoratedPressed = true;
            protected override void DecorateReleased() => decoratedReleased = true;
            protected override void DecorateSelected() => decoratedSelected = true;
            protected override void DecorateDeselected() => decoratedDeselected = true;
            protected override void DecorateEnter() => decoratedEntered = true;
            protected override void DecorateExit() => decoratedExited = true;
            protected override void DecorateEnabled() => decoratedEnabled = true;
            protected override void DecorateDisabled() => decoratedDisabled = true;

            public void InitStates(
                StateActive active = StateActive.Enabled,
                StateSelected selected = StateSelected.Deselected,
                StateHover hover = StateHover.Outside,
                StatePressed pressed = StatePressed.Released)
            {
                SetStates(active, selected, hover, pressed);
            }
        }

        private TestDecorator _decorator;

        private static TestDecorator CreateTestDecorator(
            StateActive active = StateActive.Enabled,
            StateSelected selected = StateSelected.Deselected,
            StateHover hover = StateHover.Outside,
            StatePressed pressed = StatePressed.Released)
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<TestDecorator>();
            var testDecorator = gameObject.GetComponent<TestDecorator>();
            testDecorator.InitStates(active, selected, hover, pressed);
            return testDecorator;
        }
        
        [Test]
        public void BaseDecorator_WillSetDefaults()
        {
            _decorator = CreateTestDecorator();
            
            Assert.IsTrue(_decorator.IsEnabled());
            Assert.IsTrue(_decorator.IsOutside());
            Assert.IsTrue(_decorator.IsDeselected());
            Assert.IsTrue(_decorator.IsReleased());
        }

        [Test]
        public void OnPressed_WhenEnabled_WillBePressed()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnPressed();
            
            Assert.IsTrue(_decorator.IsPressed());
        }

        [Test]
        public void OnPressed_WhenDisabled_WillNotBePressed()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnPressed();
            
            Assert.IsFalse(_decorator.IsPressed());
        }

        [Test]
        public void OnPressed_WhenEnabled_WillDecoratePressed()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnPressed();
            
            Assert.IsTrue(_decorator.decoratedPressed);
        }
        
        [Test]
        public void OnPressed_WhenDisabled_WillNotDecoratePressed()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnPressed();
            
            Assert.IsFalse(_decorator.decoratedPressed);
        }

        [Test]
        public void OnReleased_WhenEnabled_WillBeReleased()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnReleased();
            
            Assert.IsTrue(_decorator.IsReleased());
        }

        [Test]
        public void OnReleased_WhenDisabled_WillNotBeReleased()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled, StateSelected.Deselected, StateHover.Outside,
                StatePressed.Pressed);
            
            _decorator.OnReleased();
            
            Assert.IsFalse(_decorator.IsReleased());
        }

        [Test]
        public void OnReleased_WhenEnabled_WillDecorateReleased()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnReleased();
            
            Assert.IsTrue(_decorator.decoratedReleased);
        }
        
        [Test]
        public void OnReleased_WhenDisabled_WillNotDecorateReleased()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnReleased();
            
            Assert.IsFalse(_decorator.decoratedReleased);
        }

        [Test]
        public void OnSelected_WhenEnabled_WillBeSelected()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnSelected();
            
            Assert.IsTrue(_decorator.IsSelected());
        }

        [Test]
        public void OnSelected_WhenDisabled_WillNotBeSelected()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnSelected();
            
            Assert.IsFalse(_decorator.IsSelected());
        }

        [Test]
        public void OnSelected_WhenEnabled_WillDecorateSelected()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnSelected();
            
            Assert.IsTrue(_decorator.decoratedSelected);
        }

        [Test]
        public void OnSelected_WhenDisabled_WillNotDecorateSelected()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnSelected();
            
            Assert.IsFalse(_decorator.decoratedSelected);
        }

        [Test]
        public void OnDeselected_WhenEnabled_WillBeDeselected()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnDeselected();
            
            Assert.IsTrue(_decorator.IsDeselected());
        }

        [Test]
        public void OnDeselected_WhenDisabled_WillNotBeSelected()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled, StateSelected.Selected);
            
            _decorator.OnDeselected();
            
            Assert.IsFalse(_decorator.IsDeselected());
        }

        [Test]
        public void OnDeselected_WhenEnabled_WillDecorateDeselected()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnDeselected();
            
            Assert.IsTrue(_decorator.decoratedDeselected);
        }

        [Test]
        public void OnDeselected_WhenDisabled_WillNotDecorateDeselected()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnDeselected();
            
            Assert.IsFalse(_decorator.decoratedDeselected);
        }

        [Test]
        public void OnEnter_WhenEnabledAndDeselected_WillEnter()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnEnter();
            
            Assert.IsTrue(_decorator.IsInside());
        }
        
        [Test]
        public void OnEnter_WhenEnabledAndSelected_WillNotEnter()
        {
            _decorator = CreateTestDecorator(StateActive.Enabled, StateSelected.Selected);
            
            _decorator.OnEnter();
            
            Assert.IsFalse(_decorator.IsInside());
        }

        [Test]
        public void OnEnter_WhenDisabled_WillNotEnter()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnEnter();
            
            Assert.IsFalse(_decorator.IsInside());
        }

        [Test]
        public void OnEnter_WhenEnabledAndDeselected_WillDecorateEnter()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnEnter();
            
            Assert.IsTrue(_decorator.decoratedEntered);
        }

        [Test]
        public void OnEnter_WhenEnabledAndSelected_WillNotDecorateEnter()
        {
            _decorator = CreateTestDecorator(StateActive.Enabled, StateSelected.Selected);
            
            _decorator.OnEnter();
            
            Assert.IsFalse(_decorator.decoratedEntered);
        }

        [Test]
        public void OnEnter_WhenDisabled_WillNotDecorateEnter()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnEnter();
            
            Assert.IsFalse(_decorator.decoratedEntered);
        }

        [Test]
        public void OnExit_WhenEnabled_WillBeOutside()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnExit();
            
            Assert.IsTrue(_decorator.IsOutside());
        }

        [Test]
        public void OnExit_WhenDisabled_WillNotBeOutside()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled, StateSelected.Deselected, StateHover.Inside);
            
            _decorator.OnExit();
            
            Assert.IsFalse(_decorator.IsOutside());
        }

        [Test]
        public void OnExit_WhenEnabled_WillDecorateExit()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnExit();
            
            Assert.IsTrue(_decorator.decoratedExited);
        }

        [Test]
        public void OnExit_WhenDisabled_WillNotDecorateExit()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled);
            
            _decorator.OnExit();
            
            Assert.IsFalse(_decorator.decoratedExited);
        }

        [Test]
        public void OnEnabled_WillSetDefaultStates()
        {
            _decorator = CreateTestDecorator(StateActive.Disabled, StateSelected.Selected, StateHover.Inside,
                StatePressed.Pressed);
            
            _decorator.OnEnabled();
            
            Assert.IsTrue(_decorator.IsEnabled());
            Assert.IsTrue(_decorator.IsDeselected());
            Assert.IsTrue(_decorator.IsOutside());
            Assert.IsTrue(_decorator.IsReleased());
        }

        [Test]
        public void OnEnabled_WillDecorateEnabled()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnEnabled();
            
            Assert.IsTrue(_decorator.decoratedEnabled);
        }
        
        [Test]
        public void OnDisabled_WillSetDefaultsAndBeDisabled()
        {
            _decorator = CreateTestDecorator(StateActive.Enabled, StateSelected.Selected, StateHover.Inside,
                StatePressed.Pressed);
            
            _decorator.OnDisabled();
            
            Assert.IsTrue(_decorator.IsDisabled());
            Assert.IsTrue(_decorator.IsDeselected());
            Assert.IsTrue(_decorator.IsOutside());
            Assert.IsTrue(_decorator.IsReleased());
        }

        [Test]
        public void OnDisabled_WillDecorateDisabled()
        {
            _decorator = CreateTestDecorator();
            
            _decorator.OnDisabled();
            
            Assert.IsTrue(_decorator.decoratedDisabled);
        }
    }
}
