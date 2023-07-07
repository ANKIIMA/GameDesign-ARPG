using SharpUI.Source.Common.Util.Reactive;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.Animation
{
    public interface ICanvasGroupAnimator
    {
        void SetDisposable(CompositeDisposable disposable);
        void SetAnimationState(IAnimationState state);
        void TakeCanvasGroup(CanvasGroup group);
        void SetDelayObserver(IDelayObserver delayObserver);
        void SetHideDelayMillis(long delay);
        void DropCanvasGroup();
        bool IsCanvasGroupVisible();
        void FadeIn();
        void FadeOut();
        void ShowCanvasGroup();
        void HideCanvasGroup();
    }
}