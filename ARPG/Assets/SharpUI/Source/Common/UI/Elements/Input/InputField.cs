using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx.Triggers;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Input
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputField : BaseElement
    {
        private TMP_InputField _inputField;

        protected override void SetupElement()
        {
            base.SetupElement();
            _inputField = GetComponent<TMP_InputField>();
        }
        
        protected override void SetupSelectable() => selectableElement = _inputField;

        protected override void ObserveEvents()
        {
            _inputField.OnPointerDownAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerDown());
            _inputField.OnPointerUpAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerUp());
            _inputField.OnPointerEnterAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerEnter());
            _inputField.OnPointerExitAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerExit());
            _inputField.OnSelectAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedSelected());
            _inputField.OnDeselectAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedDeselect());

            if (isSelected)
                eventPromoter.ObservedSelected();
        }
    }
}