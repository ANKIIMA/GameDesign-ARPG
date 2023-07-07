using System;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.Events
{
    public interface IElementEventListener
    {
        IObservable<Unit> ObserveOnPressed();
        IObservable<Unit> ObserveOnReleased();
        IObservable<Unit> ObserveOnClicked();
        IObservable<Unit> ObserveOnLeftClicked();
        IObservable<Unit> ObserveOnRightClicked();
        IObservable<Unit> ObserveOnMiddleClicked();
        IObservable<Unit> ObserveOnEnabled();
        IObservable<Unit> ObserveOnDisabled();
        IObservable<Unit> ObserveOnEntered();
        IObservable<Unit> ObserveOnExited();
        IObservable<Unit> ObserveOnSelected();
        IObservable<Unit> ObserveOnDeselected();
    }
}