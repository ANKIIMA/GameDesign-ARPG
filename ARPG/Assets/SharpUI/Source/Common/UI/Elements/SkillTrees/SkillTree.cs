using System.Collections.Generic;
using System.Linq;
using SharpUI.Source.Common.UI.Elements.Toggle;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.SkillTrees
{
    public class SkillTree : MonoBehaviour
    {
        [SerializeField] public ToggleButton rootNode;
        [SerializeField] public SkillAmountLimit skillAmounts;
        
        private List<ISkillLevel> _skillLevels;
        private List<ISkillProgressLine> _skillProgressLines;
        private ISkillAmountLimit _skillAmountLimit;
        private bool _initialized;

        public void Awake()
        {
            SetupSkillTree();
        }

        public void OnGUI()
        {
            if (_initialized) return;
            _initialized = true;
            
            ObserveChanges();
            TriggerRootNodeChange();
            OnAnyLevelChanged();
        }

        public void SetSkillLevels(List<ISkillLevel> skillLevels)
            => _skillLevels = skillLevels;

        public void SetSkillProgressLines(List<ISkillProgressLine> skillProgressLines) =>
            _skillProgressLines = skillProgressLines;

        public void SetSkillAmountLimit(ISkillAmountLimit skillAmountLimit)
            => _skillAmountLimit = skillAmountLimit;

        private void TriggerRootNodeChange()
        {
            rootNode.ToggleOn();
        }

        private void SetupSkillTree()
        {
            _skillAmountLimit = skillAmounts;
            _skillLevels = gameObject.GetComponentsInChildren<ISkillLevel>().ToList();
            _skillProgressLines = gameObject.GetComponentsInChildren<ISkillProgressLine>().ToList();
        }

        private void ObserveChanges()
        {
            _skillLevels.ForEach(skillLevel =>
                    skillLevel.ObserveCurrentLevelChanged().SubscribeWith(this, _ => OnAnyLevelChanged()));
        }

        private void OnAnyLevelChanged()
        {
            var amountSpent = _skillLevels.Sum(skillLevel => skillLevel.GetCurrentLevel());
            UpdateProgressLines(amountSpent);
            UpdateSkillAmounts(amountSpent);
        }

        private void UpdateProgressLines(int spent)
        {
            _skillProgressLines.ForEach(progressLine => progressLine.OnSkillAmountChanged(spent));
        }

        private void UpdateSkillAmounts(int spent)
        {
            _skillAmountLimit.UpdateSpent(spent);
        }
    }
}