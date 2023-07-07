using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.ArrowLists.Animation;
using SharpUI.Source.Common.Util.Reactive;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.ArrowLists.Animation
{
    public class ArrowListAnimatorTests
    {
        private readonly Exception _animationException = new Exception("Animation error!");
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private IDelayObserver _delayObserver;
        private ArrowListAnimator _animator;
        private TMP_Text _textComponent;
        private RectTransform _textRectTransform;
        private Vector2 _sizeDelta;

        [SetUp]
        public void SetUp()
        {
            _delayObserver = Substitute.For<IDelayObserver>();
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .Returns(Observable.Return((long) 1));
            
            _animator = new ArrowListAnimator(_disposable, _delayObserver);
            
            var go = new GameObject();
            _textComponent = go.AddComponent<TextMeshPro>();
            _textRectTransform = go.GetComponent<RectTransform>();
            
            _sizeDelta = new Vector2(23, 45);
            _textRectTransform.sizeDelta = _sizeDelta;
            
            _animator.BindTextComponent(_textComponent);
        }

        [Test]
        public void ArrowListAnimator_CanCreateEmptyConstructor()
        {
            _animator = new ArrowListAnimator();
        }

        [Test]
        public void SlideRight_WillFinishAnimating()
        {
            _animator.CloneText();
            
            _animator.SlideRight();

            Assert.IsFalse(_animator.IsAnimating());
        }

        [Test]
        public void SlideRight_WillSetAnimateDirectionToRight()
        {
            _animator.CloneText();
            
            _animator.SlideRight();
            
            Assert.AreEqual(AnimateDirection.Right, _animator.GetDirection());
        }

        [Test]
        public void SlideRight_WillAnimatePositionX()
        {
            _animator.CloneText();
            
            var previousPositionX = _textRectTransform.localPosition.x;
            _animator.SlideRight();
            
            Assert.AreNotEqual(previousPositionX, _textRectTransform.localPosition.x);
        }
        
        [Test]
        public void SlideRight_WillAnimateAlpha()
        {
            _animator.CloneText();
            _textComponent.alpha = 0.1f;
            
            var previousAlpha = _textComponent.alpha;
            _animator.SlideRight();

            Assert.AreNotEqual(previousAlpha, _textComponent.alpha);
        }
        
        [Test]
        public void SlideLeft_WillFinishAnimating()
        {
            _animator.CloneText();
            
            _animator.SlideLeft();

            Assert.IsFalse(_animator.IsAnimating());
        }
        
        [Test]
        public void SlideLeft_WillSetAnimateDirectionToLeft()
        {
            _animator.CloneText();
            
            _animator.SlideLeft();

            Assert.AreEqual(AnimateDirection.Left, _animator.GetDirection());
        }
        
        [Test]
        public void SlideLeft_WillAnimatePositionX()
        {
            _animator.CloneText();
            
            var previousPositionX = _textRectTransform.localPosition.x;
            _animator.SlideLeft();
            
            Assert.AreNotEqual(previousPositionX, _textRectTransform.localPosition.x);
        }
        
        [Test]
        public void SlideLeft_WillAnimateAlpha()
        {
            _animator.CloneText();
            _textComponent.alpha = 0.1f;
            
            var previousAlpha = _textComponent.alpha;
            _animator.SlideLeft();

            Assert.AreNotEqual(previousAlpha, _textComponent.alpha);
        }

        [Test]
        public void Unbind_WillDispose()
        {
            _animator.CloneText();
            
            _animator.Unbind();
            
            Assert.IsTrue(_disposable.IsDisposed);
        }

        [Test]
        public void SlideRight_WhenAnimationFails_WillFinishAnimation()
        {
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .ReturnsForAnyArgs(Observable.Throw<long>(_animationException));
            _animator.CloneText();

            Assert.Throws<Exception>(() => _animator.SlideRight());
            Assert.IsFalse(_animator.IsAnimating());
        }
        
        [Test]
        public void SlideLeft_WhenAnimationFails_WillFinishAnimation()
        {
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .ReturnsForAnyArgs(Observable.Throw<long>(_animationException));
            _animator.CloneText();

            Assert.Throws<Exception>(() => _animator.SlideLeft());
            Assert.IsFalse(_animator.IsAnimating());
        }
    }
}