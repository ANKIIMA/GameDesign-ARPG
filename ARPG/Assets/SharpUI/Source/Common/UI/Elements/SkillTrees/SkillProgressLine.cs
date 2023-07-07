using System;
using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Elements.Progress;
using SharpUI.Source.Common.UI.Elements.Toggle;
using SharpUI.Source.Common.UI.Util;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.SkillTrees
{
    public class SkillProgressLine : ProgressBar, ISkillProgressLine
    {
        [SerializeField,CanBeNull] public SkillProgressLine parent;
        [SerializeField] public ToggleButton checkPointToggle;
        [SerializeField] public int requiredLevelsAmount;

        private readonly IUiUtil _uiUtil = new UiUtil();
        private int _unlockAtSkillAmount;
        private int _totalAmount;

        public override void Start()
        {
            base.Start();
            CalculateUnlockAmount();
            OnSkillAmountChanged(0);
        }

        public int GetUnlockAtSkillAmount() => _unlockAtSkillAmount;

        public int GetTotalAmount() => _totalAmount;
        
        public void OnSkillAmountChanged(int totalAmount)
        {
            _totalAmount = totalAmount;
            var percentageAmount = GetPercentageFromAmount();
            UpdatePercentage(percentageAmount);
            UpdateCheckPoint();
        }

        private void CalculateUnlockAmount()
        {
            _unlockAtSkillAmount = 0;
            var parentProgress = parent;
            while (parentProgress != null)
            {
                _unlockAtSkillAmount += parentProgress.requiredLevelsAmount;
                parentProgress = parentProgress.parent;
            }
        }

        private bool IsUnlocked() => _totalAmount >= _unlockAtSkillAmount;

        private bool IsFilled() => _totalAmount >= _unlockAtSkillAmount + requiredLevelsAmount;

        private float GetPercentageFromAmount()
        {
            if (!IsUnlocked()) return 0f;
            
            float amount = Math.Min(_totalAmount - _unlockAtSkillAmount, requiredLevelsAmount);
            return _uiUtil.ToPercentage(amount / requiredLevelsAmount);

        }

        private void UpdateCheckPoint()
        {
            if (checkPointToggle == null) return;
            
            if (IsFilled())
                checkPointToggle.ToggleOn();
            else
                checkPointToggle.ToggleOff();
        }
    }
}