using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Events;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.List.ListItem
{
    [RequireComponent(typeof(UnityEngine.UI.Button), typeof(Image))]
    public class Item : RectButton
    {
        protected override void SetupUI()
        {
            base.SetupUI();
            eventPromoter = new ItemEventPromoter(dispatcher, state, decorators);
        }
        
        protected override void ObserveEvents()
        {
            button.OnPointerDownAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerDown());
            button.OnPointerUpAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerUp());
            button.OnClickAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedClick());
            button.OnPointerEnterAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerEnter());
            button.OnPointerExitAsObservable().SubscribeWith(this, _ => eventPromoter.ObservedPointerExit());
            
            if (isSelected)
                eventPromoter.ObservedSelected();
        }

        public void SelectItem() => (eventPromoter as ItemEventPromoter)?.SelectItem();

        public void DeselectItem() => (eventPromoter as ItemEventPromoter)?.DeselectItem();
    }
}