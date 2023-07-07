using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Elements.SkillTrees;
using SharpUI.Source.Common.UI.Elements.Toggle;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Button
{
    public class SkillTreeButton : IconButton
    {
        [SerializeField] public Image frameImage;
        [SerializeField] public SkillLevel skillLevel;
        [SerializeField] public SkillAmountLimit skillLimit;
        [SerializeField,CanBeNull] public ToggleButton nodeOwner;
        [SerializeField] public Color activeFrameColor;
        [SerializeField] public Color disabledFrameColor;
        [SerializeField] public Color activeIconColor;
        [SerializeField] public Color disabledIconColor;

        private ISkillAmountLimit _skillAmountLimit;
        private ISkillLevel _skillLevel;
        
        protected override void SetupUI()
        {
            base.SetupUI();
            SetSkillAmountLimit(skillLimit);
            SetSkillLevel(skillLevel);
            ObserveClicks();
            SetColors();
        }

        public void SetSkillAmountLimit(ISkillAmountLimit skillAmountLimit) => _skillAmountLimit = skillAmountLimit;

        public void SetSkillLevel(ISkillLevel newSkillLevel) => _skillLevel = newSkillLevel;

        private void ObserveClicks()
        {
            dispatcher.ObserveOnLeftClicked().SubscribeWith(this, _ => Increment());
            dispatcher.ObserveOnRightClicked().SubscribeWith(this, _ => Decrement());
        }

        private void Increment()
        {
            if (!CanIncrement()) return;
            
            _skillLevel.IncrementLevel();
            SetColors();
        }

        private void Decrement()
        {
            if (!CanDecrement()) return;

            _skillLevel.DecrementLevel();
            SetColors();
        }

        private bool CanIncrement() => _skillAmountLimit.CanSpend() && (nodeOwner == null || nodeOwner.isOn);

        private bool CanDecrement() => _skillAmountLimit.CanTakeBack();

        private void SetColors()
        {
            if (_skillLevel.HaveLevels())
            {
                frameImage.color = activeFrameColor;
                iconImage.color = activeIconColor;
            }
            else
            {
                frameImage.color = disabledFrameColor;
                iconImage.color = disabledIconColor;
            }
        }
    }
}