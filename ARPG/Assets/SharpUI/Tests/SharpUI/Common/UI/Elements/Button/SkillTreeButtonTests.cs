using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.SkillTrees;
using SharpUI.Source.Common.UI.Elements.Toggle;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Button
{
    public class SkillTreeButtonTests
    {
        private GameObject _gameObject;
        private SkillTreeButton _skillButton;
        private ISkillAmountLimit _skillAmountLimit;
        private ISkillLevel _skillLevel;
        private ToggleButton _nodeOwner;

        [SetUp]
        public void SetUp()
        {
            _skillAmountLimit = Substitute.For<ISkillAmountLimit>();
            _skillLevel = Substitute.For<ISkillLevel>();
            
            _gameObject = new GameObject();
            _skillButton = _gameObject.AddComponent<SkillTreeButton>();
            _skillButton.frameImage = new GameObject().AddComponent<Image>();
            _skillButton.skillLevel = new GameObject().AddComponent<SkillLevel>();
            _skillButton.skillLimit = new GameObject().AddComponent<SkillAmountLimit>();
            _skillButton.iconImage = new GameObject().AddComponent<Image>();
            _nodeOwner = new GameObject().AddComponent<ToggleButton>();
            _skillButton.nodeOwner = _nodeOwner;
            _skillButton.activeFrameColor = Color.blue;
            _skillButton.disabledFrameColor = Color.gray;
            _skillButton.activeIconColor = Color.green;
            _skillButton.disabledIconColor = Color.yellow;

            _skillButton.isClickable = true;
            _skillButton.Awake();
            _skillButton.Start();
            _skillButton.SetSkillAmountLimit(_skillAmountLimit);
            _skillButton.SetSkillLevel(_skillLevel);
        }

        [Test]
        public void WhenLeftClicked_CanIncrement_WillIncrement()
        {
            _skillAmountLimit.CanSpend().Returns(true);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnLeftClicked();
            
            _skillLevel.Received().IncrementLevel();
        }
        
        [Test]
        public void WhenLeftClicked_CannotIncrement_WillNotIncrement()
        {
            _skillAmountLimit.CanSpend().Returns(false);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnLeftClicked();
            
            _skillLevel.DidNotReceive().IncrementLevel();
        }
        
        [Test]
        public void WhenRightClicked_CanDecrement_WillDecrement()
        {
            _skillAmountLimit.CanTakeBack().Returns(true);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnRightClicked();
            
            _skillLevel.Received().DecrementLevel();
        }
        
        [Test]
        public void WhenRightClicked_CannotDecrement_WillNotDecrement()
        {
            _skillAmountLimit.CanTakeBack().Returns(false);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnRightClicked();
            
            _skillLevel.DidNotReceive().DecrementLevel();
        }

        [Test]
        public void WhenHaveSkillLevels_WillSetActiveColorOnFrame()
        {
            _skillLevel.HaveLevels().Returns(true);
            _skillAmountLimit.CanSpend().Returns(true);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnLeftClicked();
            
            Assert.AreEqual(_skillButton.activeFrameColor, _skillButton.frameImage.color);
        }
        
        [Test]
        public void WhenDontHaveSkillLevels_WillSetDisabledColorOnFrame()
        {
            _skillLevel.HaveLevels().Returns(false);
            _skillAmountLimit.CanSpend().Returns(true);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnLeftClicked();
            
            Assert.AreEqual(_skillButton.disabledFrameColor, _skillButton.frameImage.color);
        }
        
        [Test]
        public void WhenHaveSkillLevels_WillSetActiveColorOnIcon()
        {
            _skillLevel.HaveLevels().Returns(true);
            _skillAmountLimit.CanSpend().Returns(true);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnLeftClicked();
            
            Assert.AreEqual(_skillButton.activeIconColor, _skillButton.iconImage.color);
        }
        
        [Test]
        public void WhenDontHaveSkillLevels_WillSetDisabledColorOnIcon()
        {
            _skillLevel.HaveLevels().Returns(false);
            _skillAmountLimit.CanSpend().Returns(true);
            _nodeOwner.isOn = true;
            
            _skillButton.GetEventDispatcher().OnLeftClicked();
            
            Assert.AreEqual(_skillButton.disabledIconColor, _skillButton.iconImage.color);
        }
    }
}