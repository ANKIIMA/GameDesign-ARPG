using System;
using System.Globalization;
using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Source.Common.UI.Prototype.SharpList
{
    public class CustomItemHolder : ItemHolder<Tuple<string, float>>
    {
        private readonly CustomItem _customItem;
        
        public CustomItemHolder(CustomItem customItem) : base(customItem)
        {
            _customItem = customItem;
        }

        public override void BindData(Tuple<string, float> customData)
        {
            base.BindData(customData);
            _customItem.leftText.text = customData.Item1;
            _customItem.rightText.text = customData.Item2.ToString(CultureInfo.InvariantCulture);
        }
    }
}