using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Animation;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Notification = SharpUI.Source.Common.UI.Elements.Notifications.Notification;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Notifications
{
    public class NotificationTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private const long DelayMillis = 2897;
        private const string Text = "Text";
        private GameObject _gameObject;
        private Notification _notification;
        private ICanvasGroupAnimator _animator;
        private CanvasGroup _canvasGroup;

        [SetUp]
        public void SetUp()
        {
            _animator = Substitute.For<ICanvasGroupAnimator>();
            _gameObject = new GameObject();
            _canvasGroup = _gameObject.AddComponent<CanvasGroup>();
            _notification = _gameObject.AddComponent<Notification>();
            _notification.textTitle = new GameObject().AddComponent<TextMeshPro>();
            _notification.textSubtitle = new GameObject().AddComponent<TextMeshPro>();
            _notification.autoFadeout = false;
            _notification.hideOnStart = false;
            _notification.hideDelayMillis = DelayMillis;
            _notification.SetCanvasGroupAnimator(_animator);
        }

        [Test]
        public void Awake_AnimatorWillTakeCanvasGroup()
        {
            _notification.Awake();
            
            _animator.Received().TakeCanvasGroup(_canvasGroup);
        }

        [Test]
        public void Awake_AnimatorSetShowDelayMillis()
        {
            _notification.Awake();
            
            _animator.Received().SetHideDelayMillis(DelayMillis);
        }

        [Test]
        public void Awake_WillFadeOutIfCriteriaCorrect()
        {
            _animator.IsCanvasGroupVisible().Returns(true);
            _notification.autoFadeout = true;
            _notification.hideOnStart = false;
            
            _notification.Awake();
            
            _animator.Received().FadeOut();
        }

        [Test]
        public void Awake_WhenHideRequested_WillHide()
        {
            _notification.hideOnStart = true;
            
            _notification.Awake();
            
            _animator.Received().HideCanvasGroup();
        }

        [Test]
        public void OnDestroy_AnimatorWillDropCanvasGroup()
        {
            _notification.Awake();
            
            _notification.OnDestroy();
            
            _animator.DropCanvasGroup();
        }

        [Test]
        public void ObserveOnClick_WhenClicked_WillBeObserved()
        {
            var observed = false;
            _notification.Awake();
            _notification.ObserveOnClick().SubscribeWith(_disposable, _ => observed = true);
            
            _notification.OnPointerClick(new PointerEventData(EventSystem.current));
            
            Assert.IsTrue(observed);
        }
        
        [Test]
        public void OnPointerClick_WhenCloseOnClickRequested_AnimatorWillDropCanvasGroup()
        {
            _notification.closeWhenClicked = true;
            _notification.Awake();
            
            _notification.OnPointerClick(new PointerEventData(EventSystem.current));
            
            _animator.Received().DropCanvasGroup();
        }
        
        [Test]
        public void OnPointerClick_WhenCloseOnClickRequested_WillDestroyGameObject()
        {
            _notification.closeWhenClicked = true;
            _notification.Awake();
            
            _notification.OnPointerClick(new PointerEventData(EventSystem.current));
            
            Assert.IsFalse(_gameObject);
        }

        [Test]
        public void SetTitle_WillSetTitle()
        {
            _notification.Awake();
            
            _notification.SetTitle(Text);
            
            Assert.AreEqual(Text, _notification.textTitle.text);
        }

        [Test]
        public void SetSubtitle_WillSetSubtitle()
        {
            _notification.Awake();
            
            _notification.SetSubtitle(Text);
            
            Assert.AreEqual(Text, _notification.textSubtitle.text);
        }

        [Test]
        public void Show_AnimatorWillShowCanvasGroup()
        {
            _notification.Awake();
            
            _notification.Show();
            
            _animator.Received().ShowCanvasGroup();
        }
        
        [Test]
        public void Hide_AnimatorWillHideCanvasGroup()
        {
            _notification.Awake();
            
            _notification.Hide();
            
            _animator.Received().HideCanvasGroup();
        }

        [Test]
        public void ShowAnimated_AnimatorWillFadeCanvasGroupIn()
        {
            _notification.Awake();
            
            _notification.ShowAnimated();
            
            _animator.Received().FadeIn();
        }
        
        [Test]
        public void HideAnimated_AnimatorWillFadeCanvasGroupOut()
        {
            _notification.Awake();
            
            _notification.HideAnimated();
            
            _animator.Received().FadeOut();
        }
    }
}