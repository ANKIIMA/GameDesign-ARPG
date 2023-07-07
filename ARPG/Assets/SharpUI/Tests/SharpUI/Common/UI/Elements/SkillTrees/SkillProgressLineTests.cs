using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.SkillTrees;
using SharpUI.Source.Common.UI.Elements.Toggle;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.SkillTrees
{
    public class SkillProgressLineTests
    {
        private const int RequiredLevels = 8;
        private const int RequiredLevelsParent1 = 12;
        private const int RequiredLevelsParent2 = 7;
        
        private SkillProgressLine _parent1;
        private SkillProgressLine _parent2;
        private GameObject _gameObject = new GameObject();
        private SkillProgressLine _skillProgressLine;
        private ToggleButton _checkpointToggle;

        [SetUp]
        public void SetUp()
        {
            _parent1 = new GameObject().AddComponent<SkillProgressLine>();
            _parent1.requiredLevelsAmount = RequiredLevelsParent1;
            _parent1.barImage = new GameObject().AddComponent<Image>();
            _parent1.Start();
            
            _parent2 = new GameObject().AddComponent<SkillProgressLine>();
            _parent2.requiredLevelsAmount = RequiredLevelsParent2;
            _parent2.barImage = new GameObject().AddComponent<Image>();
            _parent2.parent = _parent1;
            _parent2.Start();
            
            _checkpointToggle = new GameObject().AddComponent<ToggleButton>();
            _checkpointToggle.checkedImage = new GameObject().AddComponent<Image>();
            _checkpointToggle.isOn = false;
            
            _gameObject = new GameObject();
            _skillProgressLine = _gameObject.AddComponent<SkillProgressLine>();
            _skillProgressLine.barImage = new GameObject().AddComponent<Image>();
            _skillProgressLine.backgroundImage = new GameObject().AddComponent<Image>();
            _skillProgressLine.parent = null;
            _skillProgressLine.requiredLevelsAmount = RequiredLevels;
            _skillProgressLine.checkPointToggle = _checkpointToggle;
        }

        [Test]
        public void Start_ParentNull_WillCalculateUnlockAmountCorrectly()
        {
            _skillProgressLine.Start();
            
            Assert.AreEqual(0, _skillProgressLine.GetUnlockAtSkillAmount());
        }
        
        [Test]
        public void Start_WithParents_WillCalculateUnlockAmountCorrectly()
        {
            _skillProgressLine.parent = _parent2;
            _skillProgressLine.Start();

            const int expected = RequiredLevelsParent1 + RequiredLevelsParent2;
            Assert.AreEqual(expected, _skillProgressLine.GetUnlockAtSkillAmount());
        }

        [Test]
        public void OnSkillAmountChanged_WillSetTotalAmount()
        {
            _skillProgressLine.Start();
            
            _skillProgressLine.OnSkillAmountChanged(RequiredLevels);
            
            Assert.AreEqual(RequiredLevels, _skillProgressLine.GetTotalAmount());
        }

        [Test]
        public void OnSkillAmountChanged_WhenFilled_WillSwitchToggleOn()
        {
            _checkpointToggle.isOn = false;
            _skillProgressLine.Start();
            
            _skillProgressLine.OnSkillAmountChanged(RequiredLevels);
            
            Assert.IsTrue(_checkpointToggle.isOn);
        }
        
        [Test]
        public void OnSkillAmountChanged_WhenNotFilled_WillSwitchToggleOff()
        {
            _checkpointToggle.isOn = true;
            _skillProgressLine.Start();
            
            _skillProgressLine.OnSkillAmountChanged(RequiredLevels - 1);
            
            Assert.IsFalse(_checkpointToggle.isOn);
        }
    }
}