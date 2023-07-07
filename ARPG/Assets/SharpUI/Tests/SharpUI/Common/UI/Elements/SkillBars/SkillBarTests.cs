using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.SkillBars;
using SharpUI.Source.Common.UI.Util.TimeUtils;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.SkillBars
{
    public class SkillBarTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private const string SkillName = "Some Skill";
        private const float DeltaTime = 0.1f;
        private const float Cooldown = 0.0001f;
        private GameObject _gameObject;
        private SkillBar _skillBar;
        private ITimeProvider _timeProvider;

        [SetUp]
        public void SetUp()
        {
            _timeProvider = Substitute.For<ITimeProvider>();
            _timeProvider.GetDeltaTime().Returns(DeltaTime);
            
            _gameObject = new GameObject();
            _gameObject.AddComponent<RectTransform>();
            _skillBar = _gameObject.AddComponent<SkillBar>();
            _skillBar.skillBarImage = new GameObject().AddComponent<Image>();
            _skillBar.skillIconImage = new GameObject().AddComponent<Image>();
            _skillBar.skillNameText = new GameObject().AddComponent<TextMeshPro>();
            _skillBar.skillRemainingCooldownText = new GameObject().AddComponent<TextMeshPro>();
            _skillBar.skillCooldown = Cooldown;
            _skillBar.skillCooldownRemaining = Cooldown;
            _skillBar.skillName = SkillName;
            _skillBar.consumeType = SkillBar.CooldownConsumeType.Fill;
            _skillBar.depleteWhenCompleted = false;
            _skillBar.SetTimeProvider(_timeProvider);
        }

        [Test]
        public void Start_IsNotFinished()
        {
            _skillBar.Start();
            
            Assert.AreEqual(false, _skillBar.IsFinished());
        }

        [Test]
        public void OnDestroy_WillStopManagedUiUpdates()
        {
            _skillBar.Start();
            
            _skillBar.OnDestroy();
            
            Assert.IsFalse(_skillBar.IsUpdatingUi());
        }

        [Test]
        public void ObserveCooldownFinished_WhenAllConsumed_WillBeObserved()
        {
            var observed = false;
            _skillBar.ObserveCooldownFinished().SubscribeWith(_disposable, _ => observed = true);
            _skillBar.Start();
            _skillBar.StartCooldown();
            
            _skillBar.Update();
            _skillBar.UpdateProgress(DeltaTime);
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void ObserveCooldownFinished_WhenFinished_WillNotObserveAgain()
        {
            var timesObserved = 0;
            _skillBar.ObserveCooldownFinished().SubscribeWith(_disposable, _ => timesObserved++);
            _skillBar.Start();
            _skillBar.StartCooldown();
            
            _skillBar.Update();
            _skillBar.UpdateProgress(DeltaTime);
            _skillBar.Update();
            _skillBar.UpdateProgress(DeltaTime);
            
            Assert.AreEqual(1, timesObserved);
        }

        [Test]
        public void UpdateProgress_WhenAllTimeConsumedAndDepleteRequested_WillDeplete()
        {
            _skillBar.depleteWhenCompleted = true;
            _skillBar.Start();
            _skillBar.StartCooldown();
            _skillBar.Update();

            _skillBar.UpdateProgress(DeltaTime);
            
            Assert.IsFalse(_skillBar);
        }

        [Test]
        public void Cancel_WillDeplete()
        {
            _skillBar.Start();
            _skillBar.StartCooldown();
            _skillBar.Update();
            
            _skillBar.Cancel();
            
            Assert.IsFalse(_skillBar);
        }

        [Test]
        public void RestartCooldown_WillStartCooldown()
        {
            _skillBar.RestartCooldown();
            
            Assert.IsFalse(_skillBar.IsFinished());
        }

        [Test]
        public void UpdateProgress_WhenFillBarr_WillFill()
        {
            _skillBar.consumeType = SkillBar.CooldownConsumeType.Fill;
            _skillBar.skillCooldown = 2.5f;
            _skillBar.skillCooldownRemaining = 2.5f;
            _skillBar.Start();
            _skillBar.StartCooldown();
            _skillBar.Update();
            
            _skillBar.UpdateProgress(DeltaTime);
            
            Assert.Positive(_skillBar.skillBarImage.GetComponent<RectTransform>().localPosition.x);
        }
        
        [Test]
        public void UpdateProgress_WhenDrainBarr_WillDrain()
        {
            _skillBar.consumeType = SkillBar.CooldownConsumeType.Drain;
            _skillBar.skillCooldown = 2.5f;
            _skillBar.skillCooldownRemaining = 2.5f;
            _skillBar.Start();
            _skillBar.StartCooldown();
            _skillBar.Update();
            
            _skillBar.UpdateProgress(DeltaTime);
            
            Assert.Negative(_skillBar.skillBarImage.GetComponent<RectTransform>().localPosition.x);
        }
    }
}