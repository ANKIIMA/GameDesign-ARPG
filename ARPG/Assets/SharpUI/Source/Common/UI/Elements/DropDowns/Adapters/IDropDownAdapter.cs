using System;
using System.Collections.Generic;
using UniRx;
using static TMPro.TMP_Dropdown;

namespace SharpUI.Source.Common.UI.Elements.DropDowns.Adapters
{
    public interface IDropDownAdapter
    {
        string GetItemTextAt(int index);
        int DataCount();
        IObservable<Unit> ObserveDataChange();
        List<OptionData> GetOptionsData();
    }
}