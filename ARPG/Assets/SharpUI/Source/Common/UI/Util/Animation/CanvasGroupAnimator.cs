using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Common.Util.Reactive;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.Animation
{
    public class CanvasGroupAnimator : ICanvasGroupAnimator
    {
        private const long DefaultFadeDelayMillis = 8;
        private const int DefaultFadeOutSteps = 200;
        private const int DefaultFadeInSteps = 80;
     
        private CompositeDisposable _disposable = new CompositeDisposable();
        private CanvasGroup _canvasGroup;
        private IDelayObserver _delayObserver = new DelayObserver();
        private IAnimationState _state = new AnimationState();
        private long _hideDelayMillis;

        public void SetAnimationState(IAnimationState state) => _state = state;
        
        public void SetDisposable(CompositeDisposable disposable) => _disposable = disposable;
        
        public void TakeCanvasGroup(CanvasGroup group) => _canvasGroup = group;
        
        public void SetHideDelayMillis(long delay) => _hideDelayMillis = delay;
        
        public void SetDelayObserver(IDelayObserver delayObserver) => _delayObserver = delayObserver;

        public void DropCanvasGroup()
        {
            _disposable.Dispose();
            _canvasGroup = null;
        }
        
        public void FadeIn()
        {
            if (_state.IsAnimating()) return;

            ShowCanvasGroup();
            SetCanvasGroupAlpha(0);
            _delayObserver
                .DelayMilliseconds(DefaultFadeDelayMillis, Scheduler.MainThread, DefaultFadeInSteps)
                .DoOnError(_ => FinishFadeout())
                .DoOnSubscribe(OnAnimationStarted)
                .Finally(FinishFadein)
                .SubscribeWith(_disposable, value => SetCanvasGroupAlpha((value + 1f) / DefaultFadeInSteps));
        }

        public void FadeOut()
        {
            if (_state.IsAnimating()) return;

            ShowCanvasGroup();
            SetCanvasGroupAlpha(1);
            _delayObserver
                .DelayMilliseconds(_hideDelayMillis, Scheduler.MainThread, 1)
                .DoOnError(_ => FinishFadeout())
                .DoOnSubscribe(OnAnimationStarted)
                .SubscribeWith(_disposable, _ => AnimateFadeOut());
        }

        private void AnimateFadeOut()
        {
            _delayObserver
                .DelayMilliseconds(DefaultFadeDelayMillis, Scheduler.MainThread, DefaultFadeOutSteps)
                .Finally(FinishFadeout)
                .SubscribeWith(_disposable,
                    value => SetCanvasGroupAlpha((float)(DefaultFadeOutSteps - value) / DefaultFadeOutSteps));
        }
        
        private void FinishFadeout()
        {
            OnAnimationFinished();
            SetCanvasGroupAlpha(0);
        }

        private void FinishFadein()
        {
            OnAnimationFinished();
            SetCanvasGroupAlpha(1);
        }
        
        private void OnAnimationStarted() => _state.OnAnimationBegin();

        private void OnAnimationFinished() => _state.OnAnimationEnd();
        
        private void SetCanvasGroupAlpha(float alpha) => _canvasGroup.alpha = alpha;

        public bool IsCanvasGroupVisible() => _canvasGroup.alpha > 0.0f;

        public void ShowCanvasGroup()
        {
            if (_state.IsAnimating()) return;
            
            _canvasGroup.gameObject.SetActive(true);
            SetCanvasGroupAlpha(1);
        }

        public void HideCanvasGroup()
        {
            if (_state.IsAnimating()) return;
            
            SetCanvasGroupAlpha(0);
            _canvasGroup.gameObject.SetActive(false);
        }
    }
}