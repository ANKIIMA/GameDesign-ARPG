using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.SkillTrees
{
    public class SkillAmountLimit : MonoBehaviour, ISkillAmountLimit
    {
        [SerializeField] public int totalAvailable;
        [SerializeField] public int totalSpent;
        [SerializeField] public TMP_Text availableText;
        [SerializeField] public TMP_Text spentText;

        private readonly Subject<Unit> _amountChangedObserver = new Subject<Unit>();
        private int _totalAvailable;
        private int _available;
        private int _spent;

        public void Awake()
        {
            _totalAvailable = totalAvailable;
            _spent = totalSpent;
            _available = _totalAvailable - _spent;
            Render();
        }

        public void UpdateSpent(int spent)
        {
            if (!AreValidAmounts())
                return;
            
            _spent = spent;
            _available = _totalAvailable - _spent;
            _amountChangedObserver.OnNext(Unit.Default);
            Render();
        }

        private bool AreValidAmounts() => _spent <= _totalAvailable;

        public bool CanSpend() => _available > 0;

        public bool CanTakeBack() => _spent > 0;

        public int GetAvailable() => _available;

        public IObservable<Unit> ObserveAmountChanged() => _amountChangedObserver;

        private void Render()
        {
            availableText.text = _available.ToString();
            spentText.text = _spent.ToString();
        }
    }
}