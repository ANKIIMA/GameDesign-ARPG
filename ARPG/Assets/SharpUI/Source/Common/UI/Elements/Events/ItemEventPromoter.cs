using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.UI.Elements.State;

namespace SharpUI.Source.Common.UI.Elements.Events
{
    public class ItemEventPromoter : ElementEventPromoter
    {
        public ItemEventPromoter(
            IElementEventDispatcher eventDispatcher,
            IElementState state,
            List<IDecorator> decorators
            ) : base(eventDispatcher, state, decorators)
        {
        }

        public void SelectItem() => PromoteSelect();

        public void DeselectItem() => PromoteDeselect();
    }
}