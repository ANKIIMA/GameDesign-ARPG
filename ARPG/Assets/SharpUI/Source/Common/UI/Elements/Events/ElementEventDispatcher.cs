using System;
using JetBrains.Annotations;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.Events
{
    public class ElementEventDispatcher : IElementEventDispatcher, IElementEventListener
    {
        [CanBeNull] private Subject<Unit> _onPressedObserver;
        [CanBeNull] private Subject<Unit> _onReleasedObserver;
        [CanBeNull] private Subject<Unit> _onClickedObserver;
        [CanBeNull] private Subject<Unit> _onLeftClickedObserver;
        [CanBeNull] private Subject<Unit> _onRightClickedObserver;
        [CanBeNull] private Subject<Unit> _onMiddleClickedObserver;
        [CanBeNull] private Subject<Unit> _onEnabledObserver;
        [CanBeNull] private Subject<Unit> _onDisabledObserver;
        [CanBeNull] private Subject<Unit> _onEnteredObserver;
        [CanBeNull] private Subject<Unit> _onExitedObserver;
        [CanBeNull] private Subject<Unit> _onSelectedObserver;
        [CanBeNull] private Subject<Unit> _onDeselectedObserver;

        public void OnPressed() => _onPressedObserver?.OnNext(Unit.Default);
        
        public void OnReleased() => _onReleasedObserver?.OnNext(Unit.Default);
        
        public void OnClicked() => _onClickedObserver?.OnNext(Unit.Default);
        
        public void OnLeftClicked() => _onLeftClickedObserver?.OnNext(Unit.Default);
        
        public void OnRightClicked() => _onRightClickedObserver?.OnNext(Unit.Default);
        
        public void OnMiddleClicked() => _onMiddleClickedObserver?.OnNext(Unit.Default);

        public void OnEnabled() => _onEnabledObserver?.OnNext(Unit.Default);

        public void OnDisabled() => _onDisabledObserver?.OnNext(Unit.Default);

        public void OnEnter() => _onEnteredObserver?.OnNext(Unit.Default);

        public void OnExit() => _onExitedObserver?.OnNext(Unit.Default);

        public void OnSelect() => _onSelectedObserver?.OnNext(Unit.Default);

        public void OnDeselect() => _onDeselectedObserver?.OnNext(Unit.Default);

        public IObservable<Unit> ObserveOnPressed()
            => _onPressedObserver ?? (_onPressedObserver = new Subject<Unit>());

        public IObservable<Unit> ObserveOnReleased()
            => _onReleasedObserver ?? (_onReleasedObserver = new Subject<Unit>());
        
        public IObservable<Unit> ObserveOnClicked()
            => _onClickedObserver ?? (_onClickedObserver = new Subject<Unit>());
        
        public IObservable<Unit> ObserveOnLeftClicked()
            => _onLeftClickedObserver ?? (_onLeftClickedObserver = new Subject<Unit>());
        
        public IObservable<Unit> ObserveOnRightClicked()
            => _onRightClickedObserver ?? (_onRightClickedObserver = new Subject<Unit>());
        
        public IObservable<Unit> ObserveOnMiddleClicked()
            => _onMiddleClickedObserver ?? (_onMiddleClickedObserver = new Subject<Unit>());

        public IObservable<Unit> ObserveOnEnabled()
            => _onEnabledObserver ?? (_onEnabledObserver = new Subject<Unit>());

        public IObservable<Unit> ObserveOnDisabled()
            => _onDisabledObserver ?? (_onDisabledObserver = new Subject<Unit>());

        public IObservable<Unit> ObserveOnEntered()
            => _onEnteredObserver ?? (_onEnteredObserver = new Subject<Unit>());

        public IObservable<Unit> ObserveOnExited()
            => _onExitedObserver ?? (_onExitedObserver = new Subject<Unit>());

        public IObservable<Unit> ObserveOnSelected()
            => _onSelectedObserver ?? (_onSelectedObserver = new Subject<Unit>());

        public IObservable<Unit> ObserveOnDeselected()
            => _onDeselectedObserver ?? (_onDeselectedObserver = new Subject<Unit>());
    }
}