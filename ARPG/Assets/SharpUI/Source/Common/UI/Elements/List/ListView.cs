using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.List
{
    [RequireComponent(typeof(IListAdapter))]
    public class ListView : MonoBehaviour
    {
        [SerializeField] public GameObject container;
        [SerializeField] public bool enableItems;
        [SerializeField] public bool canClickItems;
        [SerializeField] public bool canSelectItems;
        [SerializeField] public ItemSelectionType selectionType;
        [SerializeField] public int selectionAmount;

        private IListAdapter _adapter;

        public void Awake()
        {
            if (GetComponent<IListAdapter>() != null)
                _adapter = GetComponent<IListAdapter>();
            
            SetInteraction();
            SetSelectionStrategy();
            ObserveDataChange();
        }

        public void SetAdapter(IListAdapter adapter)
        {
            _adapter = adapter;
        }

        public void SetItemsEnabled(bool itemsEnabled)
        {
            enableItems = itemsEnabled;
            _adapter.SetItemsEnabled(itemsEnabled);
        }

        public void SetItemsClickable(bool clickable)
        {
            canClickItems = clickable;
            _adapter.SetCanClickItems(clickable);
        }

        public void SetItemsSelectable(bool selectable)
        {
            canSelectItems = selectable;
            _adapter.SetCanSelectItems(selectable);
        }

        private void SetInteraction()
        {
            _adapter.SetCanClickItems(canClickItems);
            _adapter.SetCanSelectItems(canSelectItems);
            _adapter.SetItemsEnabled(enableItems);
        }

        private void SetSelectionStrategy()
        {
            _adapter.SetSelectionStrategy(selectionType, selectionAmount);
        }

        private void ObserveDataChange()
        {
            _adapter.ObserveDataChange().SubscribeWith(this, _ => RenderItems());
        }

        private void RenderItems()
        {
            for (var i = 0; i < _adapter.DataCount(); i++)
                _adapter.RenderTo(i, container.transform);
        }
    }
}
