using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.ActionBars;
using SharpUI.Source.Common.UI.Util.TimeUtils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.ActionBars
{
    public class ActionBarButtonTests
    {
        private const Image.FillMethod FillMethod = Image.FillMethod.Radial180;
        private const float Seconds = 2.56f;
        private GameObject _gameObject;
        private ActionBarButton _actionButton;
        private IActionBarCooldown _cooldown;
        private ITimeProvider _timeProvider;

        [SetUp]
        public void SetUp()
        {
            _timeProvider = Substitute.For<ITimeProvider>();
            _timeProvider.GetDeltaTime().Returns(0.1f);
            
            _gameObject = new GameObject();
            _cooldown = Substitute.For<IActionBarCooldown>();
            _actionButton = _gameObject.AddComponent<ActionBarButton>();
            _actionButton.iconImage = new GameObject().AddComponent<Image>();
            _actionButton.cooldownText = new GameObject().AddComponent<TextMeshPro>();
            _actionButton.cooldownFillImage = new GameObject().AddComponent<Image>();
            
            _actionButton.Awake();
            _actionButton.Start();
            _actionButton.SetActionBarCooldown(_cooldown);
            _actionButton.SetTimeProvider(_timeProvider);
        }

        [Test]
        public void Update_WillConsumeTime()
        {
            _actionButton.CoolDown(Seconds);
            
            _actionButton.Update();
            
            _cooldown.Received().ConsumeSeconds(Arg.Any<float>());
        }

        [Test]
        public void CoolDown_WillStartCooldown()
        {
            _actionButton.CoolDown(Seconds);
            
            _cooldown.Received().CoolDown(Seconds);
        }

        [Test]
        public void SetFillMethod_WillSetCooldownFillMethod()
        {
            _actionButton.SetFillMethod(FillMethod);
            
            _cooldown.Received().SetFillMethod(FillMethod);
        }

        [Test]
        public void CoolDown_WhenAbilityNotBound_WillNotStart()
        {
            _actionButton.canOverrideCooldown = true;
            _actionButton.iconImage.gameObject.SetActive(false);
            
            _actionButton.CoolDown(10f);
            
            Assert.IsFalse(_actionButton.IsCoolingDown());
        }
    }
}