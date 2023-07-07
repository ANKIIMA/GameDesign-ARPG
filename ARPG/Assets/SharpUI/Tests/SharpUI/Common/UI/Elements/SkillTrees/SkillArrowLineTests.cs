using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Progress;
using SharpUI.Source.Common.UI.Elements.SkillTrees;
using SharpUI.Source.Common.UI.Elements.Toggle;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.SkillTrees
{
    public class SkillArrowLineTests
    {
        private GameObject _gameObject;
        private SkillArrowLine _skillArrowLine;
        private ProgressBar _progressBar;
        private ToggleButton _nodeOwnerToggle;

        [SetUp]
        public void SetUp()
        {
            _progressBar = new GameObject().AddComponent<ProgressBar>();
            _progressBar.barText = new GameObject().AddComponent<TextMeshPro>();
            _progressBar.barImage = new GameObject().AddComponent<Image>();
            _progressBar.backgroundImage = new GameObject().AddComponent<Image>();
            
            _nodeOwnerToggle = new GameObject().AddComponent<ToggleButton>();
            _nodeOwnerToggle.checkedImage = new GameObject().AddComponent<Image>();
            _nodeOwnerToggle.isOn = false;

            _gameObject = new GameObject();
            _skillArrowLine = _gameObject.AddComponent<SkillArrowLine>();
            _skillArrowLine.progressBar = _progressBar;
            _skillArrowLine.triangleImage = new GameObject().AddComponent<Image>();
            _skillArrowLine.nodeOwner = _nodeOwnerToggle;
            _skillArrowLine.activeColor = Color.green;
            _skillArrowLine.disabledColor = Color.red;
            
            _skillArrowLine.Start();
        }

        [Test]
        public void Start_WillSetDefaultColorsOnProgressBar()
        {
            Assert.AreEqual(_skillArrowLine.disabledColor, _skillArrowLine.progressBar.backgroundImage.color);
            Assert.AreEqual(_skillArrowLine.activeColor, _skillArrowLine.progressBar.barImage.color);
        }

        [Test]
        public void WhenToggleActivated_WillSetPercentageTo100()
        {
            _nodeOwnerToggle.isOn = false;
            _nodeOwnerToggle.Awake();
            _nodeOwnerToggle.Start();
            
            _nodeOwnerToggle.ToggleOn();
            
            Assert.AreEqual(1f, _skillArrowLine.progressBar.barImage.fillAmount);
        }
        
        [Test]
        public void WhenToggleActivated_WillSetActiveColorOnTriangle()
        {
            _nodeOwnerToggle.isOn = false;
            _nodeOwnerToggle.Awake();
            _nodeOwnerToggle.Start();
            
            _nodeOwnerToggle.ToggleOn();
            
            Assert.AreEqual(_skillArrowLine.activeColor, _skillArrowLine.triangleImage.color);
        }
        
        [Test]
        public void WhenToggleDeactivated_WillSetPercentageTo0()
        {
            _nodeOwnerToggle.isOn = true;
            _nodeOwnerToggle.Awake();
            _nodeOwnerToggle.Start();
            
            _nodeOwnerToggle.ToggleOff();
            
            Assert.AreEqual(0f, _skillArrowLine.progressBar.barImage.fillAmount);
        }
        
        [Test]
        public void WhenToggleDeactivated_WillSetDisabledColorOnTriangle()
        {
            _nodeOwnerToggle.isOn = true;
            _nodeOwnerToggle.Awake();
            _nodeOwnerToggle.Start();
            
            _nodeOwnerToggle.ToggleOff();
            
            Assert.AreEqual(_skillArrowLine.disabledColor, _skillArrowLine.triangleImage.color);
        }
    }
}