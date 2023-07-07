using System;
using UniRx;

namespace SharpUI.Source.Common.UI.Elements.SkillTrees
{
    public interface ISkillAmountLimit
    {
        void UpdateSpent(int spent);
        bool CanSpend();
        bool CanTakeBack();
        int GetAvailable();
        IObservable<Unit> ObserveAmountChanged();
    }
}