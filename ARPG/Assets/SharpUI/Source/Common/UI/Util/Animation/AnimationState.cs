namespace SharpUI.Source.Common.UI.Util.Animation
{
    public class AnimationState : IAnimationState
    {
        private bool _isAnimating;

        public AnimationState() { }

        public AnimationState(bool isAnimating) => _isAnimating = isAnimating;

        public void OnAnimationBegin() => _isAnimating = true;

        public void OnAnimationEnd() => _isAnimating = false;

        public bool IsAnimating() => _isAnimating;
    }
}