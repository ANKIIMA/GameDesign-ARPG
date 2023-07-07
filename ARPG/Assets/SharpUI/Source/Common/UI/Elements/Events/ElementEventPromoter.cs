using System;
using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.UI.Elements.State;
using UnityEngine.EventSystems;

namespace SharpUI.Source.Common.UI.Elements.Events
{
    public class ElementEventPromoter : IElementEventPromoter
    {
        private readonly IElementEventDispatcher _eventDispatcher;
        private readonly IElementState _state;
        private readonly List<IDecorator> _decorators;
        
        public ElementEventPromoter(
            IElementEventDispatcher eventDispatcher,
            IElementState state,
            List<IDecorator> decorators)
        {
            _eventDispatcher = eventDispatcher;
            _state = state;
            _decorators = decorators;
        }

        public void ObservedPointerDown()
        {
            if (_state.IsClickable())
                PromotePointerDown();
        }

        public void ObservedPointerUp()
        {
            if (_state.IsClickable())
                PromotePointerUp();
        }

        public void ObservedClick()
        {
            if (_state.IsClickable())
                PromoteClick();
        }

        public void ObservePointerClick(PointerEventData.InputButton inputButton)
        {
            if (!_state.IsClickable()) return;

            switch (inputButton)
            {
                case PointerEventData.InputButton.Left:
                    PromoteLeftClick();
                    break;
                case PointerEventData.InputButton.Right:
                    PromoteRightClick();
                    break;
                case PointerEventData.InputButton.Middle:
                    PromoteMiddleClick();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputButton), inputButton, null);
            }
        }

        public void ObservedPointerEnter() => PromoteEnter();

        public void ObservedPointerExit() => PromoteExit();

        public void ObservedSelected()
        {
            if (_state.IsClickable())
                PromoteSelect();
        }

        public void ObservedDeselect()
        {
            if (_state.IsClickable())
                PromoteDeselect();
        }

        private void PromotePointerDown()
        {
            _state.Press();
            _eventDispatcher.OnPressed();
            _decorators.OnPressed();
        }
        
        private void PromotePointerUp()
        {
            _state.Release();
            _eventDispatcher.OnReleased();
            _decorators.OnReleased();
        }
        
        private void PromoteClick() => _eventDispatcher.OnClicked();

        private void PromoteLeftClick() => _eventDispatcher.OnLeftClicked();
        
        private void PromoteRightClick() => _eventDispatcher.OnRightClicked();
        
        private void PromoteMiddleClick() => _eventDispatcher.OnMiddleClicked();
        
        private void PromoteEnter()
        {
            _state.Focus();
            _eventDispatcher.OnEnter();
            _decorators.OnEnter();
        }
        
        private void PromoteExit()
        {
            _state.UnFocus();
            _eventDispatcher.OnExit();
            _decorators.OnExit();
        }
        
        protected void PromoteSelect()
        {
            _state.SelectIfSelectable();
            if (_state.IsDeselected()) return;
            
            _eventDispatcher.OnSelect();
            _decorators.OnSelected();
        }
        
        protected void PromoteDeselect()
        {
            _state.DeselectIfSelectable();
            if (_state.IsSelected()) return;
            
            _eventDispatcher.OnDeselect();
            _decorators.OnDeselected();
        }
    }
}