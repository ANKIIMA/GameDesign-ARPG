using System;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.ActionBars;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.ActionBars
{
    public class ActionBarCooldownTests
    {
        private readonly CompositeDisposable _disposable =new CompositeDisposable();
        private const Image.FillMethod FillMethod = Image.FillMethod.Radial360;
        private const float Seconds = 3.59f;
        private ActionBarCooldown _cooldown;
        private TMP_Text _cooldownText;
        private Image _cooldownFillImage;

        [SetUp]
        public void SetUp()
        {
            _cooldown = new ActionBarCooldown();
            _cooldownText = new GameObject().AddComponent<TextMeshPro>();
            _cooldownFillImage = new GameObject().AddComponent<Image>();
            
            _cooldown.TakeCooldownText(_cooldownText);
            _cooldown.TakeCooldownImage(_cooldownFillImage);
            _cooldown.SetFillMethod(FillMethod);
        }

        [Test]
        public void CoolDown_WillStartConsumingTime()
        {
            _cooldown.CoolDown(Seconds);
            
            Assert.IsTrue(_cooldown.IsConsumingTime());
        }

        [Test]
        public void CoolDown_WillShowCooldownText()
        {
            _cooldownText.gameObject.SetActive(false);
            
            _cooldown.CoolDown(Seconds);
            
            Assert.IsTrue(_cooldownText.gameObject.activeSelf);
        }
        
        [Test]
        public void CoolDown_WillShowCooldownImage()
        {
            _cooldownFillImage.gameObject.SetActive(false);
            
            _cooldown.CoolDown(Seconds);
            
            Assert.IsTrue(_cooldownFillImage.gameObject.activeSelf);
        }

        [Test]
        public void Expire_WillFinishCooldown()
        {
            _cooldown.CoolDown(Seconds);
            
            _cooldown.Expire();
            
            Assert.IsFalse(_cooldown.IsConsumingTime());
        }
        
        [Test]
        public void Expire_WillHideCooldown()
        {
            _cooldown.CoolDown(Seconds);
            
            _cooldown.Expire();
            
            Assert.IsFalse(_cooldown.IsConsumingTime());
        }
        
        [Test]
        public void Expire_WillHideCooldownText()
        {
            _cooldown.CoolDown(Seconds);
            
            _cooldown.Expire();
            
            Assert.IsFalse(_cooldownText.gameObject.activeSelf);
        }
        
        [Test]
        public void Expire_WillHideCooldownImage()
        {
            _cooldown.CoolDown(Seconds);
            
            _cooldown.Expire();
            
            Assert.IsFalse(_cooldownFillImage.gameObject.activeSelf);
        }

        [Test]
        public void ObserveCooldownFinished_WhenFinished_WillBeObserved()
        {
            var observed = false;
            _cooldown.ObserveCooldownFinished().SubscribeWith(_disposable, _ => observed = true);
            _cooldown.CoolDown(Seconds);
            
            _cooldown.Expire();
            
            Assert.IsTrue(observed);
        }

        [Test]
        public void ConsumeSeconds_WhenAllConsumed_WillFinish()
        {
            var finished = false;
            _cooldown.ObserveCooldownFinished().SubscribeWith(_disposable, _ => finished = true);
            _cooldown.CoolDown(Seconds);
            
            _cooldown.ConsumeSeconds(Seconds * 2f);
            _cooldown.Update();
            
            Assert.IsTrue(finished);
        }
        
        [Test]
        public void ConsumeSeconds_WhenNotAllConsumed_WillNotFinish()
        {
            var finished = false;
            _cooldown.ObserveCooldownFinished().SubscribeWith(_disposable, _ => finished = true);
            _cooldown.CoolDown(Seconds);
            
            _cooldown.ConsumeSeconds(Seconds / 2f);
            _cooldown.Update();
            
            Assert.IsFalse(finished);
        }

        [Test]
        public void ConsumeSeconds_WillSetFillImageCorrectly()
        {
            _cooldown.CoolDown(Seconds);
            
            const float consumed = 2f;
            _cooldown.ConsumeSeconds(consumed);
            _cooldown.Update();

            const float fillAmount = (Seconds - consumed) / Seconds;
            Assert.Greater(Seconds, consumed);
            Assert.AreEqual(fillAmount, _cooldownFillImage.fillAmount);
        }

        [Test]
        public void ConsumeSeconds_LessThan10sec_WillFormatTimeCorrectly()
        {
            _cooldown.CoolDown(Seconds);
            
            _cooldown.ConsumeSeconds(0f);
            _cooldown.Update();

            var expected = $"{Seconds:0.0}";
            Assert.AreEqual(expected, _cooldownText.text);
        }
        
        [Test]
        public void ConsumeSeconds_LessThan1min_WillFormatTimeCorrectly()
        {
            const float seconds = 34f;
            _cooldown.CoolDown(seconds);
            
            _cooldown.ConsumeSeconds(0f);
            _cooldown.Update();

            var expected = $"{seconds:0} s";
            Assert.AreEqual(expected, _cooldownText.text);
        }
        
        [Test]
        public void ConsumeSeconds_LessThan1hr_WillFormatTimeCorrectly()
        {
            const float seconds = 384f;
            _cooldown.CoolDown(seconds);
            
            _cooldown.ConsumeSeconds(0f);
            _cooldown.Update();

            var expected = $"{(int)Math.Ceiling(seconds/60):0} m";
            Assert.AreEqual(expected, _cooldownText.text);
        }
        
        [Test]
        public void ConsumeSeconds_LessThan1day_WillFormatTimeCorrectly()
        {
            const float seconds = 5 * 3600f;
            _cooldown.CoolDown(seconds);
            
            _cooldown.ConsumeSeconds(0f);
            _cooldown.Update();

            var expected = $"{(int)Math.Ceiling(seconds/60/60):0} h";
            Assert.AreEqual(expected, _cooldownText.text);
        }
        [Test]
        public void ConsumeSeconds_LargerThan1day_WillFormatTimeCorrectly()
        {
            const float seconds = 2 * 86405f;
            _cooldown.CoolDown(seconds);
            
            _cooldown.ConsumeSeconds(0f);
            _cooldown.Update();

            var expected = $"{(int)Math.Ceiling(seconds/60/60/24):0} d";
            Assert.AreEqual(expected, _cooldownText.text);
        }
    }
}