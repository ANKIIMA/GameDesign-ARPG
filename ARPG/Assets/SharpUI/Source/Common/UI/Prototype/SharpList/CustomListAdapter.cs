using System;
using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Factory;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Prototype.SharpList
{
    public class CustomListAdapter : ListAdapter<Tuple<string, float>>
    {
        [SerializeField] public ListItemFactory itemFactory;
        
        public void SetItemsAndNotify(List<Tuple<string, float>> items)
        {
            SetData(items);
            NotifyDataSetChanged();
        }
        
        protected override ItemHolder<Tuple<string, float>> CreateListItemHolder()
        {
            var item = itemFactory.CreateCustomListItem();
            var holder = new CustomItemHolder(item);
            return holder;
        }

        protected override void BindListItemHolder(ItemHolder<Tuple<string, float>> holder, int position)
        {
            (holder as CustomItemHolder)?.BindData(GetData(position));
        }
    }
}