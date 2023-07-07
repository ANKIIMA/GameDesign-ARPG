using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Sliders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Sliders
{
    public class SimpleSliderTests
    {
        
        private const float SliderValueMin = 0;
        private const float SliderValueMax = 100;
        private const float SliderValue = 37.8f;
        private SimpleSlider _slider;

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();
            _slider = gameObject.AddComponent<SimpleSlider>();
            _slider.slider = gameObject.AddComponent<Slider>();
            _slider.handle = new GameObject();
            _slider.GetComponent<Slider>().minValue = SliderValueMin;
            _slider.GetComponent<Slider>().maxValue = SliderValueMax;
            _slider.GetComponent<Slider>().value = SliderValue;
            _slider.percentageText = new GameObject().AddComponent<TextMeshPro>();
        }

        [Test]
        public void Start_WillSetDefaultSliderValueText()
        {
            _slider.Start();
            
            var expected = string.Format(SimpleSlider.DefaultPercentageTextFormat, SliderValue);
            Assert.AreEqual(expected, _slider.percentageText.text);
        }

        [Test]
        public void ShowHandle_WillShowHandle()
        {
            _slider.Start();
            _slider.handle.SetActive(false);
            
            _slider.ShowHandle();
            
            Assert.IsTrue(_slider.handle.activeSelf);
        }
        
        [Test]
        public void HideHandle_WillHideHandle()
        {
            _slider.Start();
            _slider.handle.SetActive(true);
            
            _slider.HideHandle();
            
            Assert.IsFalse(_slider.handle.activeSelf);
        }

        [Test]
        public void ShowPercentage_WillShowPercentageText()
        {
            _slider.Start();
            _slider.percentageText.gameObject.SetActive(false);
            
            _slider.ShowPercentage();
            
            Assert.IsTrue(_slider.percentageText.gameObject.activeSelf);
        }
        
        [Test]
        public void HidePercentage_WillHidePercentageText()
        {
            _slider.Start();
            _slider.percentageText.gameObject.SetActive(true);
            
            _slider.HidePercentage();
            
            Assert.IsFalse(_slider.percentageText.gameObject.activeSelf);
        }

        [Test]
        public void Start_whenDisplayTypePercentage_WillFormatPercentage()
        {
            _slider.textDisplayType = SimpleSlider.SliderValueDisplayType.Percentage;
            _slider.Start();
            
            var expected = string.Format(SimpleSlider.DefaultPercentageTextFormat, SliderValue);
            Assert.AreEqual(expected, _slider.percentageText.text);
        }
        
        [Test]
        public void Start_whenDisplayTypeInt_WillFormatInt()
        {
            _slider.textDisplayType = SimpleSlider.SliderValueDisplayType.RawIntValue;
            _slider.Start();
            
            var expected = string.Format(SimpleSlider.RawValueIntTextFormat, SliderValue);
            Assert.AreEqual(expected, _slider.percentageText.text);
        }
        
        [Test]
        public void Start_whenDisplayTypeFloat_WillFormatFloat()
        {
            _slider.textDisplayType = SimpleSlider.SliderValueDisplayType.RawFloatValue;
            _slider.Start();
            
            var expected = string.Format(SimpleSlider.RawValueFloatTextFormat, SliderValue);
            Assert.AreEqual(expected, _slider.percentageText.text);
        }
        
        [Test]
        public void Start_whenDisplayTypeNone_WillHideTextComponent()
        {
            _slider.textDisplayType = SimpleSlider.SliderValueDisplayType.None;
            _slider.Start();
            
            Assert.IsFalse(_slider.percentageText.gameObject.activeSelf);
        }
    }
}