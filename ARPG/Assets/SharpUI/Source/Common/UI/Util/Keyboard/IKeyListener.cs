using System;
using SharpUI.Source.Common.UI.Elements.Button;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.Keyboard
{
    public interface IKeyListener
    {
        void ListenToKey(KeyCode keyCode);
        void AddFilterTag(string filterTag);
        void RemoveFilterTag(string filterTag);
        void TakeButton(BaseButton baseButton);
        IObservable<Unit> ObserveDown();
        IObservable<Unit> ObserveUp();
        void RequireAnyShift(bool require = true);
        void RequireAnyControl(bool require = true);
    }
}