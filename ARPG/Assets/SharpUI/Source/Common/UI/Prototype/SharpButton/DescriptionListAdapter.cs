using System;
using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Factory;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Prototype.SharpButton
{
    public class DescriptionListAdapter : ListAdapter<Tuple<string, string>>
    {
        [SerializeField] public ListItemFactory itemFactory;
        
        public void SetItemsAndNotify(List<Tuple<string, string>> items)
        {
            SetData(items);
            NotifyDataSetChanged();
        }
        
        protected override ItemHolder<Tuple<string, string>> CreateListItemHolder()
        {
            var item = itemFactory.CreateListItemDescription();
            var holder = new DescriptionItemHolder(item);
            return holder;
        }

        protected override void BindListItemHolder(ItemHolder<Tuple<string, string>> holder, int position)
        {
            (holder as DescriptionItemHolder)?.BindData(GetData(position));
        }
    }
}