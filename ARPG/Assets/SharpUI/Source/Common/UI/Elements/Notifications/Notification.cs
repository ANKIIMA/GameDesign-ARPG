using System;
using SharpUI.Source.Common.UI.Util.Animation;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SharpUI.Source.Common.UI.Elements.Notifications
{
    public class Notification : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] public TMP_Text textTitle;
        [SerializeField] public TMP_Text textSubtitle;
        [SerializeField] public bool autoFadeout;
        [SerializeField] public bool hideOnStart;
        [SerializeField] public bool closeWhenClicked;
        [SerializeField] public long hideDelayMillis;

        private Subject<Unit> _onPointerClickObserver;
        private ICanvasGroupAnimator _animator = new CanvasGroupAnimator();

        public void Awake()
        {
            InitAnimator();
            InitFadeOut();
            InitVisibility();
        }

        public void OnDestroy()
        {
            _animator.DropCanvasGroup();
        }

        public IObservable<Unit> ObserveOnClick()
            => _onPointerClickObserver ?? (_onPointerClickObserver = new Subject<Unit>());

        public void SetCanvasGroupAnimator(ICanvasGroupAnimator animator) => _animator = animator;

        private void InitAnimator()
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            _animator.TakeCanvasGroup(canvasGroup);
            _animator.SetHideDelayMillis(hideDelayMillis);
        }
        
        private void InitFadeOut()
        {
            if (autoFadeout && !hideOnStart && _animator.IsCanvasGroupVisible())
                _animator.FadeOut();
        }

        private void InitVisibility()
        {
            if (hideOnStart)
                Hide();
            else
                Show();
        }

        public void SetTitle(string title) => textTitle.text = title;

        public void SetSubtitle(string subtitle) => textSubtitle.text = subtitle;

        public void Show() => _animator.ShowCanvasGroup();

        public void Hide() => _animator.HideCanvasGroup();

        public void ShowAnimated() => _animator.FadeIn();

        public void HideAnimated() => _animator.FadeOut();

        private void Close()
        {
            _animator.DropCanvasGroup();
            DestroyImmediate(gameObject);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _onPointerClickObserver?.OnNext(Unit.Default);

            if (closeWhenClicked)
                Close();
        }
    }
}