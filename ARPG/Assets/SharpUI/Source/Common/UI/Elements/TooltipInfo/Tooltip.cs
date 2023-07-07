using System;
using SharpUI.Source.Common.UI.Util;
using SharpUI.Source.Common.UI.Util.Layout;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Common.Util.Reactive;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.TooltipInfo
{
    public class Tooltip : MonoBehaviour, ITooltip
    {
        private CompositeDisposable _showDisposable = new CompositeDisposable();
        private Margin _margin = new Margin(10, 10, 10, 10);
        private readonly IUiUtil _util = new UiUtil();
        private ITooltipPointer _tooltipPointer;
        private RectTransform _rectTransform;
        private RectTransform _sourceTransform;
        private RectTransform _contentTransform;
        private long _showDelayTimeMillis;
        private IDelayObserver _delayObserver = new DelayObserver();
        
        public void Awake()
        {
            _tooltipPointer = GetComponentInChildren<TooltipPointer>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void Start()
        {
            PositionPointerTo(PointerPosition.Right);
        }

        public void OnDestroy()
        {
            _showDisposable.Dispose();
        }

        public void SetShowDisposable(CompositeDisposable disposable) => _showDisposable = disposable;

        public void SetDelayObserver(IDelayObserver observer)
        {
            _delayObserver = observer;
        }

        public void SetTooltipPointer(ITooltipPointer pointer)
        {
            _tooltipPointer = pointer;
        }

        public void PositionPointerTo(PointerPosition pointerPosition)
        {
            _tooltipPointer.SetPosition(pointerPosition);
        }

        public void OffsetPointerByPercentage(float offset)
        {
            _tooltipPointer.SetOffsetPercentage(offset);
        }

        public void Hide()
        {
            _showDisposable.Clear();
            _contentTransform.SetParent(null);
            gameObject.SetActive(false);
        }

        public long GetShowDelayTime() => _showDelayTimeMillis;
        
        public void SetShowDelayTimeMillis(long millis) => _showDelayTimeMillis = millis;

        private void ShowInside(RectTransform parentTransform)
        {
            _sourceTransform = parentTransform;
            transform.SetParent(parentTransform.transform.parent);
            _contentTransform.SetParent(_rectTransform);
            gameObject.SetActive(true);
        }

        private float CenterHorizontalOffset()
            => _util.Half(_sourceTransform.sizeDelta.x) - _util.Half(_tooltipPointer.Height) -
               _tooltipPointer.OffsetSize();

        private float CenterVerticalOffset()
            => _util.Half(_sourceTransform.sizeDelta.y) - _util.Half(_tooltipPointer.Height) -
               _tooltipPointer.OffsetSize();
        
        private float PivotHorizontalOffset()
            => -_sourceTransform.pivot.x * _sourceTransform.sizeDelta.x
               +_rectTransform.pivot.x * _rectTransform.sizeDelta.x;

        private float PivotVerticalOffset()
            => -_sourceTransform.pivot.y * _sourceTransform.sizeDelta.y
               +_rectTransform.pivot.y * _rectTransform.sizeDelta.y;

        private float TopVerticalOffset()
            => _sourceTransform.sizeDelta.y + _tooltipPointer.Width;

        private float BottomVerticalOffset()
            => -_rectTransform.sizeDelta.y - _tooltipPointer.Width;

        private float LeftHorizontalOffset()
            => -_rectTransform.sizeDelta.x - _tooltipPointer.Width;

        private float RightHorizontalOffset()
            => _sourceTransform.sizeDelta.x + _tooltipPointer.Width;
        
        private Vector2 TopOffset()
            => new Vector2(
                PivotHorizontalOffset() + CenterHorizontalOffset(),
                PivotVerticalOffset() + TopVerticalOffset());

        private Vector2 BottomOffset()
            => new Vector2(
                PivotHorizontalOffset() + CenterHorizontalOffset(),
                PivotVerticalOffset() + BottomVerticalOffset());

        private Vector2 LeftOffset()
            => new Vector2(
                PivotHorizontalOffset() + LeftHorizontalOffset(),
                PivotVerticalOffset() + CenterVerticalOffset());
        
        private Vector2 RightOffset()
            => new Vector2(
                PivotHorizontalOffset() + RightHorizontalOffset(),
                PivotVerticalOffset() + CenterVerticalOffset());

        private void SetLocalPositionWith(Vector2 offset)
        {
            var sourceLocalPosition = _sourceTransform.localPosition;
            _rectTransform.localPosition = new Vector3(
                sourceLocalPosition.x + offset.x,
                sourceLocalPosition.y + offset.y);
            _contentTransform.gameObject.SetActive(true);
        }

        private IObservable<long> ClearAndDelay()
        {
            _showDisposable.Clear();
            return _delayObserver.DelayMilliseconds(_showDelayTimeMillis, Scheduler.MainThread);
        }

        public void ShowToLeftOf(RectTransform sourceTransform)
        {
            ClearAndDelay().SubscribeWith(_showDisposable, _ =>
            {
                _tooltipPointer.SetPosition(PointerPosition.Right);
                ShowInside(sourceTransform);
                SetLocalPositionWith(LeftOffset());
            });
        }

        public void ShowToRightOf(RectTransform sourceTransform)
        {
            ClearAndDelay().SubscribeWith(_showDisposable, _ =>
            {
                _tooltipPointer.SetPosition(PointerPosition.Left);
                ShowInside(sourceTransform);
                SetLocalPositionWith(RightOffset());
            });
        }

        public void ShowAbove(RectTransform sourceTransform)
        {
            ClearAndDelay().SubscribeWith(_showDisposable, _ =>
            {
                _tooltipPointer.SetPosition(PointerPosition.Bottom);
                ShowInside(sourceTransform);
                SetLocalPositionWith(TopOffset());
            });
        }
        
        public void ShowBelow(RectTransform sourceTransform)
        {
            ClearAndDelay().SubscribeWith(_showDisposable, _ =>
            {
                _tooltipPointer.SetPosition(PointerPosition.Top);
                ShowInside(sourceTransform);
                SetLocalPositionWith(BottomOffset());
            });
        }

        public void SetMargins(Margin margin) => _margin = margin;

        public void BindContent(RectTransform contentTransform)
        {
            _contentTransform = contentTransform;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentTransform);
            SetContentSize(_contentTransform.rect);
            _contentTransform.SetParent(_rectTransform);
            _contentTransform.localPosition = new Vector3(
                -(_rectTransform.pivot.x - 0.5f) * _rectTransform.sizeDelta.x,
                -(_rectTransform.pivot.y - 0.5f) * _rectTransform.sizeDelta.y);
            _contentTransform.gameObject.SetActive(false);
        }

        private void SetContentSize(Rect rect)
        {
            var width = rect.width + _margin.Left + _margin.Right;
            var height = rect.height + _margin.Top + _margin.Bottom;
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}