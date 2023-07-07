using SharpUI.Source.Common.UI.Util.Keyboard;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Button
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class BaseButton : BaseElement
    {
        protected UnityEngine.UI.Button button;

        private CompositeDisposable _deselectionObservable = new CompositeDisposable();
        private IKeyListener _keyListener;

        protected override void SetupElement()
        {
            base.SetupElement();
            button = GetComponent<UnityEngine.UI.Button>();
        }

        public void EnablePermanentSelection()
        {
            if (!_deselectionObservable.IsDisposed)
                _deselectionObservable.Dispose();
        }

        public void DisablePermanentSelection()
        {
            if (!_deselectionObservable.IsDisposed) return;
            
            _deselectionObservable = new CompositeDisposable();
            button.OnDeselectAsObservable().SubscribeWith(_deselectionObservable,
                _ => eventPromoter.ObservedDeselect());
        }

        public void SetKeyListener(IKeyListener keyListener) => _keyListener = keyListener;
        
        public IKeyListener GetKeyListener() => _keyListener ?? (_keyListener = CreateKeyListener());

        private IKeyListener CreateKeyListener()
        {
            _keyListener = gameObject.AddComponent<SimpleKeyListener>();
            _keyListener.TakeButton(this);
            return _keyListener;
        }

        protected override void SetupSelectable() => selectableElement = button;

        protected override void ObserveEvents()
        {
            button.OnPointerDownAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerDown());
            button.OnPointerUpAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerUp());
            button.OnClickAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedClick());
            button.OnPointerEnterAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerEnter());
            button.OnPointerExitAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerExit());
            button.OnSelectAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedSelected());
            button.OnDeselectAsObservable().SubscribeWith(_deselectionObservable, _ => eventPromoter.ObservedDeselect());
            button.OnPointerClickAsObservable().SubscribeWith(this, data => eventPromoter.ObservePointerClick(data.button));
            
            if (isSelected)
                eventPromoter.ObservedSelected();
        }
    }
}