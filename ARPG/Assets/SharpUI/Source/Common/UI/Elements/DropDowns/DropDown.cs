using SharpUI.Source.Common.UI.Elements.DropDowns.Adapters;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.DropDowns
{
    public class DropDown : MonoBehaviour
    {
        private CompositeDisposable _disposable = new CompositeDisposable();
        private IDropDownAdapter _adapter;
        private TMP_Dropdown _dropdown;
        
        public void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
        }

        public void SelectAtIndex(int index)
        {
            _dropdown.value = index;
        }

        public void SetDisposable(CompositeDisposable disposable)
            => _disposable = disposable;

        public void SetAdapter(IDropDownAdapter adapter)
        {
            _adapter = adapter;
            ObserveChanges();
        }

        private void ObserveChanges()
        {
            _adapter.ObserveDataChange().SubscribeWith(_disposable, _ => Render());
        }

        private void Render()
        {
            _dropdown.options = _adapter.GetOptionsData();
        }

        public void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}