using SharpUI.Source.Common.UI.Elements.ArrowLists.Extensions;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Common.Util.Reactive;
using TMPro;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SharpUI.Source.Common.UI.Elements.ArrowLists.Animation
{
    public class ArrowListAnimator : IArrowListAnimator
    {
        private const int SlideAnimationSteps = 20;
        private const int SlideAnimationIntervalMillis = 4;
        private const float DefaultTextAlpha = 1.0f;
        private const float InvisibleTextAlpha = 0.0f;

        private readonly IDelayObserver _delayObserver = new DelayObserver();
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private RectTransform _textTransform;
        private RectTransform _cloneTransform;
        private TMP_Text _text;
        private TMP_Text _cloneText;
        private Vector2 _textSize;
        private Vector3 _originalPosition;
        private Vector3 _clonePosition;
        private bool _isAnimating;
        private AnimateDirection _direction;

        public ArrowListAnimator() { }

        public ArrowListAnimator(CompositeDisposable disposable, IDelayObserver delayObserver)
        {
            _disposable = disposable;
            _delayObserver = delayObserver;
        }

        public AnimateDirection GetDirection() => _direction;

        public void BindTextComponent(TMP_Text text)
        {
            _text = text;
            _textTransform = text.GetComponent<RectTransform>();
            _textSize = _textTransform.sizeDelta;
        }

        public void SlideLeft()
        {
            _direction = AnimateDirection.Left;
            InitTextState();
            Animate();
        }
        
        public void SlideRight()
        {
            _direction = AnimateDirection.Right;
            InitTextState();
            Animate();
        }
        
        public void CloneText()
        {
            _cloneText = Object.Instantiate(_text, _text.transform.parent);
            _cloneText.text = _text.text;
            _cloneTransform = _cloneText.GetComponent<RectTransform>();
            _clonePosition = _cloneTransform.localPosition;
        }

        private void InitTextState()
        {
            _text.alpha = InvisibleTextAlpha;
            var localPosition = _textTransform.localPosition;
            localPosition.x -= _textSize.x*_direction.DirectionMultiplier();
            _textTransform.localPosition = localPosition;
            _originalPosition = localPosition;
        }

        public void Unbind()
        {
            _disposable.Dispose();
        }
        
        public bool IsAnimating() => _isAnimating;

        private void Animate()
        {
            _delayObserver
                .DelayMilliseconds(SlideAnimationIntervalMillis, Scheduler.MainThread, SlideAnimationSteps)
                .DoOnError(_ => OnAnimationFinished())
                .DoOnSubscribe(OnAnimationStarted)
                .Finally(OnAnimationFinished)
                .SubscribeWith(_disposable, AnimateStep);
        }

        private void OnAnimationStarted()
        {
            _isAnimating = true;
        }
        
        private void OnAnimationFinished()
        {
            if (!_isAnimating) return;
            
            _isAnimating = false;
            Object.DestroyImmediate(_cloneText.gameObject);
            _text.alpha = DefaultTextAlpha;
        }

        private void AnimateStep(long value)
        {
            OffsetPosition(value);
            ApplyAlpha(value);
            ApplyScale(value);
        }
        
        private void OffsetPosition(long value)
        {
            var offset = GetOffset(value) * _direction.DirectionMultiplier();
            SetLocalPositions(offset);
        }

        private void ApplyAlpha(long value)
        {
            var percentage = GetPercentage(value);
            _text.alpha = percentage;
            _cloneText.alpha = 1.0f - percentage;
        }

        private void ApplyScale(long value)
        {
            var scale = 1.0f - GetPercentage(value)*0.5f;
            _cloneText.transform.localScale = new Vector3(scale, scale, scale);
        }

        private float GetOffset(long value)
        {
            return GetPercentage(value) * _textSize.x;
        }

        private float GetPercentage(long value)
        {
            return (value + 1f) / SlideAnimationSteps;
        }

        private void SetLocalPositions(float offset)
        {
            _textTransform.localPosition =
                new Vector3(_originalPosition.x + offset, _originalPosition.y, _originalPosition.z);

            _cloneTransform.localPosition =
                new Vector3(_clonePosition.x + offset, _clonePosition.y, _clonePosition.z);
        }
    }
}