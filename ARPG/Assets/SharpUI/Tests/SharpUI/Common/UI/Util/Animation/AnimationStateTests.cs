using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Animation;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Animation
{
    public class AnimationStateTests
    {
        private AnimationState _state;

        [SetUp]
        public void SetUp()
        {
            _state = new AnimationState();
        }

        [Test]
        public void AnimationState_DefaultConstructor_WillNotBeAnimating()
        {
            _state = new AnimationState();
            
            Assert.IsFalse(_state.IsAnimating());
        }

        [Test]
        public void AnimationState_StateConstructor_WithTrue_WillBeAnimating()
        {
            _state = new AnimationState(true);
            
            Assert.IsTrue(_state.IsAnimating());
        }

        [Test]
        public void OnAnimationBegin_WhenNotAnimating_WillBeAnimating()
        {
            _state = new AnimationState(false);
            
            _state.OnAnimationBegin();
            
            Assert.IsTrue(_state.IsAnimating());
        }

        [Test]
        public void OnAnimationEnd_WhenAnimating_WillNotBeAnimating()
        {
            _state = new AnimationState(true);
            
            _state.OnAnimationEnd();
            
            Assert.IsFalse(_state.IsAnimating());
        }
    }
}