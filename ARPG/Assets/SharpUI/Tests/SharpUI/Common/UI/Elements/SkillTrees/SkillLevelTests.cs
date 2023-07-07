using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.SkillTrees;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.SkillTrees
{
    public class SkillLevelTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private GameObject _gameObject;
        private SkillLevel _skillLevel;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _skillLevel = _gameObject.AddComponent<SkillLevel>();
            _skillLevel.levelMaxText = new GameObject().AddComponent<TextMeshPro>();
            _skillLevel.currentLevelText = new GameObject().AddComponent<TextMeshPro>();
            _skillLevel.initialMaxLevel = -1;
            _skillLevel.initialCurrentLevel = -1;
            _skillLevel.activeTextColor = Color.green;
            _skillLevel.disabledTextColor = Color.red;
        }

        [Test]
        public void Awake_WillSetDefaultLevelValues()
        {
            _skillLevel.Awake();
            
            Assert.AreEqual(SkillLevel.DefaultMaxLevel, _skillLevel.GetMaxLevel());
            Assert.AreEqual(SkillLevel.DefaultCurrentLevel, _skillLevel.GetCurrentLevel());
        }

        [Test]
        public void SetMaxLevel_WhenLessThanCurrentLevel_WillAdjustCurrentLevel()
        {
            const int maxLevel = 4;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 6;
            _skillLevel.Awake();
            
            _skillLevel.SetMaxLevel(maxLevel);
            
            Assert.AreEqual(maxLevel, _skillLevel.GetCurrentLevel());
        }

        [Test]
        public void SetMaxLevel_WillSetText()
        {
            const int maxLevel = 4;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.Awake();
            
            _skillLevel.SetMaxLevel(maxLevel);
            
            Assert.AreEqual(maxLevel.ToString(), _skillLevel.levelMaxText.text);
        }
        
        [Test]
        public void SetMaxLevel_WillBeObserved()
        {
            var observed = false;
            const int maxLevel = 4;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.Awake();
            _skillLevel.ObserveMaxLevelChanged().SubscribeWith(_disposable, _ => observed = true);
            
            _skillLevel.SetMaxLevel(maxLevel);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void SetCurrentLevel_WhenValueInRange_WillSetCurrentLevel()
        {
            const int currentLevel = 4;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 2;
            _skillLevel.Awake();
            
            _skillLevel.SetCurrentLevel(currentLevel);
            
            Assert.AreEqual(currentLevel, _skillLevel.GetCurrentLevel());
        }
        
        [Test]
        public void SetCurrentLevel_WhenUnderflow_WillNotSetCurrentLevel()
        {
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 2;
            _skillLevel.Awake();
            
            _skillLevel.SetCurrentLevel(-4);
            
            Assert.AreEqual(2, _skillLevel.GetCurrentLevel());
        }
        
        [Test]
        public void SetCurrentLevel_WhenOverflow_WillNotSetCurrentLevel()
        {
            const int currentLevel = 24;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 2;
            _skillLevel.Awake();
            
            _skillLevel.SetCurrentLevel(currentLevel);
            
            Assert.AreEqual(2, _skillLevel.GetCurrentLevel());
        }
        
        [Test]
        public void SetCurrentLevel_WhenValueInRange_WillSetText()
        {
            const int currentLevel = 4;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 2;
            _skillLevel.Awake();
            
            _skillLevel.SetCurrentLevel(currentLevel);
            
            Assert.AreEqual(currentLevel.ToString(), _skillLevel.currentLevelText.text);
        }

        [Test]
        public void SetCurrentLevel_WhenValueInRange_WillBeObserved()
        {
            var observed = false;
            const int currentLevel = 4;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 2;
            _skillLevel.Awake();
            _skillLevel.ObserveCurrentLevelChanged().SubscribeWith(_disposable, _ => observed = true);
            
            _skillLevel.SetCurrentLevel(currentLevel);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void SetCurrentLevel_WhenCurrentLevelZero_WillSetDisabledColors()
        {
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 1;
            _skillLevel.Awake();
            
            _skillLevel.SetCurrentLevel(0);
            
            Assert.AreEqual(_skillLevel.disabledTextColor, _skillLevel.levelMaxText.color);
            Assert.AreEqual(_skillLevel.disabledTextColor, _skillLevel.currentLevelText.color);
        }
        
        [Test]
        public void SetCurrentLevel_WhenCurrentLevelPositive_WillSetEnabledColors()
        {
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = 6;
            _skillLevel.Awake();
            
            _skillLevel.SetCurrentLevel(2);
            
            Assert.AreEqual(_skillLevel.activeTextColor, _skillLevel.levelMaxText.color);
            Assert.AreEqual(_skillLevel.activeTextColor, _skillLevel.currentLevelText.color);
        }

        [Test]
        public void IncrementLevel_WhenPossible_WillIncrement()
        {
            const int currentLevel = 6;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = currentLevel;
            _skillLevel.Awake();
            
            _skillLevel.IncrementLevel();
            
            Assert.AreEqual(currentLevel + 1, _skillLevel.GetCurrentLevel());
        }
        
        [Test]
        public void IncrementLevel_WhenFull_WillNotIncrement()
        {
            const int currentLevel = 6;
            _skillLevel.initialMaxLevel = currentLevel;
            _skillLevel.initialCurrentLevel = currentLevel;
            _skillLevel.Awake();
            
            _skillLevel.IncrementLevel();
            
            Assert.AreEqual(currentLevel, _skillLevel.GetCurrentLevel());
        }
        
        [Test]
        public void DecrementLevel_WhenPossible_WillDecrement()
        {
            const int currentLevel = 6;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = currentLevel;
            _skillLevel.Awake();
            
            _skillLevel.DecrementLevel();
            
            Assert.AreEqual(currentLevel - 1, _skillLevel.GetCurrentLevel());
        }
        
        [Test]
        public void DecrementLevel_WhenEmpty_WillNotDecrement()
        {
            const int currentLevel = 0;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = currentLevel;
            _skillLevel.Awake();
            
            _skillLevel.DecrementLevel();
            
            Assert.AreEqual(currentLevel, _skillLevel.GetCurrentLevel());
        }

        [Test]
        public void IsFullLevels_WhenFull_IsTrue()
        {
            const int currentLevel = 0;
            _skillLevel.initialMaxLevel = currentLevel;
            _skillLevel.initialCurrentLevel = currentLevel;
            _skillLevel.Awake();

            Assert.IsTrue(_skillLevel.IsFullLevels());
        }
        
        [Test]
        public void IsFullLevels_WhenNotFull_IsFalse()
        {
            const int currentLevel = 2;
            _skillLevel.initialMaxLevel = 8;
            _skillLevel.initialCurrentLevel = currentLevel;
            _skillLevel.Awake();

            Assert.IsFalse(_skillLevel.IsFullLevels());
        }
    }
}