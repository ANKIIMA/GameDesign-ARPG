using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.SkillTrees;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.SkillTrees
{
    public class SkillAmountLimitTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private const int TotalAvailable = 24;
        private const int TotalSpent = 8;
        private GameObject _gameObject;
        private SkillAmountLimit _skillAmountLimit;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _skillAmountLimit = _gameObject.AddComponent<SkillAmountLimit>();
            _skillAmountLimit.availableText = new GameObject().AddComponent<TextMeshPro>();
            _skillAmountLimit.spentText = new GameObject().AddComponent<TextMeshPro>();
            _skillAmountLimit.totalAvailable = 0;
            _skillAmountLimit.totalSpent = 0;
        }

        [Test]
        public void Awake_ZeroValues_WillRenderCorrectly()
        {
            _skillAmountLimit.Awake();
            
            Assert.AreEqual("0", _skillAmountLimit.availableText.text);
            Assert.AreEqual("0", _skillAmountLimit.spentText.text);
        }
        
        [Test]
        public void Awake_ZeroValues_CannotSpend()
        {
            _skillAmountLimit.Awake();
            
            Assert.IsFalse(_skillAmountLimit.CanSpend());
        }

        [Test]
        public void Awake_ZeroValues_CannotTakeBack()
        {
            _skillAmountLimit.Awake();
            
            Assert.IsFalse(_skillAmountLimit.CanTakeBack());
        }

        [Test]
        public void UpdateSpent_ZeroValues_WillNotUpdate()
        {
            var observed = false;
            _skillAmountLimit.ObserveAmountChanged().SubscribeWith(_disposable, _ => observed = true);
            
            _skillAmountLimit.Awake();
            
            Assert.IsFalse(observed);
        }

        [Test]
        public void UpdateSpent_ValidAmounts_WillUpdate()
        {
            var observed = false;
            _skillAmountLimit.ObserveAmountChanged().SubscribeWith(_disposable, _ => observed = true);
            _skillAmountLimit.totalAvailable = TotalAvailable;
            _skillAmountLimit.totalSpent = TotalSpent;
            _skillAmountLimit.Awake();
            
            _skillAmountLimit.UpdateSpent(4);
            
            Assert.IsTrue(observed);
        }
        
        [Test]
        public void UpdateSpent_ValidAmounts_WillUpdateAmountCorrectly()
        {
            const int spent = 5;
            _skillAmountLimit.totalAvailable = TotalAvailable;
            _skillAmountLimit.totalSpent = TotalSpent;
            _skillAmountLimit.Awake();
            
            _skillAmountLimit.UpdateSpent(spent);
            
            Assert.AreEqual(TotalAvailable-spent, _skillAmountLimit.GetAvailable());
        }
        
        [Test]
        public void UpdateSpent_NotValidAmounts_WillNotUpdate()
        {
            const int spent = 5;
            _skillAmountLimit.totalAvailable = 3;
            _skillAmountLimit.totalSpent = 100;
            _skillAmountLimit.Awake();
            
            _skillAmountLimit.UpdateSpent(spent);

            var expected = _skillAmountLimit.totalAvailable - _skillAmountLimit.totalSpent;
            Assert.AreEqual(expected, _skillAmountLimit.GetAvailable());
        }
    }
}