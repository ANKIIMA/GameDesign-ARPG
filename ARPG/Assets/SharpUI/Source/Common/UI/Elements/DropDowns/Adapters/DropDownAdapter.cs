using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using static TMPro.TMP_Dropdown;

namespace SharpUI.Source.Common.UI.Elements.DropDowns.Adapters
{
    public abstract class DropDownAdapter<TData> : IDropDownAdapter where TData : class
    {
        private readonly Subject<Unit> _dataChangeObserver = new Subject<Unit>();
        protected List<TData> data = new List<TData>();
        
        public void SetData(IEnumerable<TData> newData)
        {
            data = new List<TData>(newData);
            NotifyDataSetChanged();
        }
        
        public int DataCount() => data.Count;

        private void NotifyDataSetChanged() => _dataChangeObserver.OnNext(Unit.Default);

        public IObservable<Unit> ObserveDataChange() => _dataChangeObserver;

        public List<OptionData> GetOptionsData()
            => data
                .Select((item, index) => new OptionData(GetItemTextAt(index)))
                .ToList();

        public abstract string GetItemTextAt(int index);
    }
}