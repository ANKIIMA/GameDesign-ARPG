using TMPro;

namespace SharpUI.Source.Common.UI.Elements.ArrowLists.Animation
{
    public interface IArrowListAnimator
    {
        void BindTextComponent(TMP_Text text);
        void CloneText();
        void SlideLeft();
        void SlideRight();
        void Unbind();
        bool IsAnimating();
        AnimateDirection GetDirection();
    }
}