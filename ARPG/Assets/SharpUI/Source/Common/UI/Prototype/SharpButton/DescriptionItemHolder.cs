using System;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.ListItem;

namespace SharpUI.Source.Common.UI.Prototype.SharpButton
{
    public class DescriptionItemHolder : ItemHolder<Tuple<string, string>>
    {
        private readonly ItemDescription _itemDescription;
        
        public DescriptionItemHolder(ItemDescription itemDescription) : base(itemDescription)
        {
            _itemDescription = itemDescription;
        }

        public override void BindData(Tuple<string, string> descriptionData)
        {
            base.BindData(descriptionData);
            _itemDescription.textTitle.text = descriptionData.Item1;
            _itemDescription.textDescription.text = descriptionData.Item2;
        }
    }
}