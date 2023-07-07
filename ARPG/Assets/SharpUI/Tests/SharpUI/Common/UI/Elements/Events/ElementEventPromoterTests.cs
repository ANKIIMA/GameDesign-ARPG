using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.UI.Elements.Events;
using SharpUI.Source.Common.UI.Elements.State;
using UnityEngine.EventSystems;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Events
{
    public class ElementEventPromoterTests
    {
        private const int DecoratorsCount = 25;
        
        private IElementEventDispatcher _eventDispatcher;
        private IElementState _state;
        private List<IDecorator> _decorators;
        private ElementEventPromoter _promoter;

        [SetUp]
        public void SetUp()
        {
            _eventDispatcher = Substitute.For<IElementEventDispatcher>();
            _state = Substitute.For<IElementState>();
            _decorators = CreateMockedDecorators();
            _promoter = new ElementEventPromoter(_eventDispatcher, _state, _decorators);
        }

        private static List<IDecorator> CreateMockedDecorators()
        {
            var decorators = new List<IDecorator>(DecoratorsCount);
            decorators.AddRange(
                Enumerable.Range(1, DecoratorsCount)
                    .Select(_ => Substitute.For<IDecorator>()));
            return decorators;
        }

        [Test]
        public void ObservedPointerDown_WhenClickable_WillPromotePressed()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservedPointerDown();
            
            _state.Received().Press();
            _eventDispatcher.Received().OnPressed();
            _decorators.ForEach(decorator => decorator.Received().OnPressed());
        }
        
        [Test]
        public void ObservedPointerDown_WhenNotClickable_WillNotPromotePressed()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservedPointerDown();
            
            _state.DidNotReceive().Press();
            _eventDispatcher.DidNotReceive().OnPressed();
            _decorators.ForEach(decorator => decorator.DidNotReceive().OnPressed());
        }

        [Test]
        public void ObservedPointerUp_WhenClickable_WillPromoteRelease()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservedPointerUp();
            
            _state.Received().Release();
            _eventDispatcher.Received().OnReleased();
            _decorators.ForEach(decorator => decorator.Received().OnReleased());
        }
        
        [Test]
        public void ObservedPointerUp_WhenNotClickable_WillNotPromoteRelease()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservedPointerUp();
            
            _state.DidNotReceive().Release();
            _eventDispatcher.DidNotReceive().OnReleased();
            _decorators.ForEach(decorator => decorator.DidNotReceive().OnReleased());
        }

        [Test]
        public void ObservedClick_WhenClickable_WillPromoteClicked()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservedClick();
            
            _eventDispatcher.Received().OnClicked();
        }
        
        [Test]
        public void ObservedClick_WhenNotClickable_WillNotPromoteClicked()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservedClick();
            
            _eventDispatcher.DidNotReceive().OnClicked();
        }

        [Test]
        public void ObservePointerClick_LeftButton_WhenClickable_WillPromoteLeftClick()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservePointerClick(PointerEventData.InputButton.Left);
            
            _eventDispatcher.Received().OnLeftClicked();
        }
        
        [Test]
        public void ObservePointerClick_RightButton_WhenClickable_WillPromoteRightClick()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservePointerClick(PointerEventData.InputButton.Right);
            
            _eventDispatcher.Received().OnRightClicked();
        }
        
        [Test]
        public void ObservePointerClick_MiddleButton_WhenClickable_WillPromoteMiddleClick()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservePointerClick(PointerEventData.InputButton.Middle);
            
            _eventDispatcher.Received().OnMiddleClicked();
        }
        
        [Test]
        public void ObservePointerClick_LeftButton_WhenNotClickable_WillNotPromoteLeftClick()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservePointerClick(PointerEventData.InputButton.Left);
            
            _eventDispatcher.DidNotReceive().OnLeftClicked();
        }
        
        [Test]
        public void ObservePointerClick_RightButton_WhenNotClickable_WillNotPromoteRightClick()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservePointerClick(PointerEventData.InputButton.Right);
            
            _eventDispatcher.DidNotReceive().OnRightClicked();
        }
        
        [Test]
        public void ObservePointerClick_MiddleButton_WhenNotClickable_WillNotPromoteMiddleClick()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservePointerClick(PointerEventData.InputButton.Middle);
            
            _eventDispatcher.DidNotReceive().OnMiddleClicked();
        }

        [Test]
        public void ObservePointerClick_UnknownButton_WhenClickable_WillThrowException()
        {
            _state.IsClickable().Returns(true);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                _promoter.ObservePointerClick((PointerEventData.InputButton)int.MaxValue));
        }

        [Test]
        public void ObservedPointerEnter_WillPromoteEnter()
        {
            _promoter.ObservedPointerEnter();
            
            _state.Received().Focus();
            _eventDispatcher.Received().OnEnter();
            _decorators.ForEach(decorator => decorator.Received().OnEnter());
        }

        [Test]
        public void ObservedPointerExit_WillPromoteExit()
        {
            _promoter.ObservedPointerExit();
            
            _state.Received().UnFocus();
            _eventDispatcher.Received().OnExit();
            _decorators.ForEach(decorator => decorator.Received().OnExit());
        }

        [Test]
        public void ObservedSelected_WhenClickable_WillPromoteSelected()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservedSelected();
            
            _state.Received().SelectIfSelectable();
            _eventDispatcher.Received().OnSelect();
            _decorators.ForEach(decorator => decorator.Received().OnSelected());
        }
        
        [Test]
        public void ObservedSelected_WhenNotClickable_WillNotPromoteSelected()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservedSelected();
            
            _state.DidNotReceive().SelectIfSelectable();
            _eventDispatcher.DidNotReceive().OnSelect();
            _decorators.ForEach(decorator => decorator.DidNotReceive().OnSelected());
        }

        [Test]
        public void ObservedDeselect_WhenClickable_WillPromoteDeselected()
        {
            _state.IsClickable().Returns(true);
            
            _promoter.ObservedDeselect();
            
            _state.Received().DeselectIfSelectable();
            _eventDispatcher.Received().OnDeselect();
            _decorators.ForEach(decorator => decorator.Received().OnDeselected());
        }
        
        [Test]
        public void ObservedDeselect_WhenNotClickable_WillNotPromoteDeselected()
        {
            _state.IsClickable().Returns(false);
            
            _promoter.ObservedDeselect();
            
            _state.DidNotReceive().DeselectIfSelectable();
            _eventDispatcher.DidNotReceive().OnDeselect();
            _decorators.ForEach(decorator => decorator.DidNotReceive().OnDeselected());
        }
    }
}
