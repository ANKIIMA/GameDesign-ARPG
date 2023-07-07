using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Keyboard;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Keyboard
{
    public class KeyInputStateTests
    {
        private KeyInputState _keyInputState;

        [SetUp]
        public void SetUp()
        {
            _keyInputState = new KeyInputState();
        }

        [Test]
        public void IsKeyPressed_FalseByDefault()
        {
            var pressed = _keyInputState.IsKeyPressed(KeyCode.A);
            
            Assert.IsFalse(pressed);
        }

        [Test]
        public void IsKeyReleased_FalseByDefault()
        {
            var released = _keyInputState.IsKeyReleased(KeyCode.A);
            
            Assert.IsFalse(released);
        }
    }
}