using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Animation;
using SharpUI.Source.Common.Util.Reactive;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Animation
{
    public class CanvasGroupAnimatorTests
    {
        private readonly Exception _animationException = new Exception("Animation error!");
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private GameObject _gameObject;
        private CanvasGroup _canvasGroup;
        private CanvasGroupAnimator _animator;
        private IDelayObserver _delayObserver;
        private IAnimationState _state;

        [SetUp]
        public void SetUp()
        {
            _state = Substitute.For<IAnimationState>();
            _state.IsAnimating().Returns(false);
            
            _delayObserver = Substitute.For<IDelayObserver>();
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .Returns(Observable.Return((long) 1));
            
            _gameObject = new GameObject();
            _canvasGroup = _gameObject.AddComponent<CanvasGroup>();
            _animator = new CanvasGroupAnimator();

            _animator.SetDisposable(_disposable);
            _animator.SetAnimationState(_state);
            _animator.TakeCanvasGroup(_canvasGroup);
            _animator.SetHideDelayMillis(0);
            _animator.SetDelayObserver(_delayObserver);
        }

        [Test]
        public void DropCanvasGroup_WillClearDisposable()
        {
            _animator.DropCanvasGroup();
            
            Assert.IsTrue(_disposable.IsDisposed);
        }

        [Test]
        public void FadeOut_OnError_WillFinishFadeOut()
        {
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .ReturnsForAnyArgs(Observable.Throw<long>(_animationException));
            _canvasGroup.alpha = 1.0f;
            
            Assert.Throws<Exception>(() => _animator.FadeOut());
            Assert.IsFalse(_animator.IsCanvasGroupVisible());
        }

        [Test]
        public void FadeOut_WillSetAlphaToZero()
        {
            _canvasGroup.alpha = 1.0f;
            
            _animator.FadeOut();
            
            Assert.AreEqual(0.0, _canvasGroup.alpha);
        }

        [Test]
        public void FadeOut_WhenAlreadyAnimating_WillNotFadeOutAgain()
        {
            _state.IsAnimating().Returns(true);
            _canvasGroup.alpha = 0.46f;
            
            _animator.FadeOut();
            
            Assert.AreNotEqual(0.0, _canvasGroup.alpha);
        }

        [Test]
        public void FadeIn_OnError_WillFinishFadeIn()
        {
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .ReturnsForAnyArgs(Observable.Throw<long>(_animationException));
            _canvasGroup.alpha = 0.0f;
            
            Assert.Throws<Exception>(() => _animator.FadeIn());
            Assert.IsTrue(_animator.IsCanvasGroupVisible());
        }
        
        [Test]
        public void FadeIn_WillSetAlphaToOne()
        {
            _canvasGroup.alpha = 0.0f;
            
            _animator.FadeIn();
            
            Assert.AreEqual(1.0f, _canvasGroup.alpha);
        }

        [Test]
        public void FadeIn_WhenAlreadyAnimating_WillNotFadeInAgain()
        {
            _state.IsAnimating().Returns(true);
            _canvasGroup.alpha = 0.34f;
            
            _animator.FadeIn();
            
            Assert.AreNotEqual(1.0, _canvasGroup.alpha);
        }

        [Test]
        public void IsCanvasGroupVisible_WhenAlphaZero_WillBeFalse()
        {
            _canvasGroup.alpha = 0.0f;
            
            Assert.IsFalse(_animator.IsCanvasGroupVisible());
        }
        
        [Test]
        public void IsCanvasGroupVisible_WhenAlphaLargerThanOne_WillBeTrue()
        {
            _canvasGroup.alpha = 0.4f;
            
            Assert.IsTrue(_animator.IsCanvasGroupVisible());
        }

        [Test]
        public void ShowCanvasGroup_WhenNotAnimating_WillShowCanvasGroup()
        {
            _state.IsAnimating().Returns(false);
            _canvasGroup.alpha = 0.0f;
            
            _animator.ShowCanvasGroup();
            
            Assert.IsTrue(_animator.IsCanvasGroupVisible());
        }
        
        [Test]
        public void ShowCanvasGroup_WhenAnimating_WillNotShowCanvasGroup()
        {
            _state.IsAnimating().Returns(true);
            _canvasGroup.alpha = 0.0f;
            
            _animator.ShowCanvasGroup();
            
            Assert.IsFalse(_animator.IsCanvasGroupVisible());
        }
        
        [Test]
        public void HideCanvasGroup_WhenNotAnimating_WillHideCanvasGroup()
        {
            _state.IsAnimating().Returns(false);
            _canvasGroup.alpha = 1.0f;
            
            _animator.HideCanvasGroup();
            
            Assert.IsFalse(_animator.IsCanvasGroupVisible());
        }
        
        [Test]
        public void HideCanvasGroup_WhenAnimating_WillNotHideCanvasGroup()
        {
            _state.IsAnimating().Returns(true);
            _canvasGroup.alpha = 1.0f;
            
            _animator.HideCanvasGroup();
            
            Assert.IsTrue(_animator.IsCanvasGroupVisible());
        }
    }
}