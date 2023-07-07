using System;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.List.Adapter
{
    public interface IListAdapter
    {
        void SetCanClickItems(bool canClick);
        void SetCanSelectItems(bool canSelect);
        void SetItemsEnabled(bool itemsEnabled);
        void ClearAll();
        int DataCount();
        int HoldersCount();
        void RenderTo(int position, Transform containerTransform);
        IObservable<Unit> ObserveDataChange();
        void SetSelectionStrategy(ItemSelectionType type, int amount = 0);
    }
}