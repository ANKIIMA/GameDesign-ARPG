namespace SharpUI.Source.Common.UI.Util.Animation
{
    public interface IAnimationState
    {
        void OnAnimationBegin();
        void OnAnimationEnd();
        bool IsAnimating();
    }
}