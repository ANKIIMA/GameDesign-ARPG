using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Progress;
using SharpUI.Source.Common.UI.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Progress
{
    public class ProgressBarTests
    {
        private GameObject _gameObject;
        private ProgressBar _progressBar;
        private readonly IUiUtil _uiUtil = new UiUtil();

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _progressBar = _gameObject.AddComponent<ProgressBar>();
            _progressBar.barImage = new GameObject().AddComponent<Image>();
            _progressBar.barText = new GameObject().AddComponent<TextMeshPro>();
            
            _progressBar.Start();
        }
        
        [Test]
        public void Start_WillSetImageFillToDefaultValue()
        {
            Assert.AreEqual(ProgressBar.DefaultPercentage, _progressBar.barImage.fillAmount);
        }

        [Test]
        public void Start_WillSetTextPercentageToDefaultValue()
        {
            var expected = string.Format(ProgressBar.DefaultBarTextFormat, ProgressBar.DefaultPercentage);
            Assert.AreEqual(expected, _progressBar.barText.text);
        }

        [Test]
        public void UpdatePercentage_ValueInRange_WillSetImageFillCorrectly()
        {
            const float value = 34.89f;
            
            _progressBar.UpdatePercentage(value);
            
            Assert.AreEqual(_uiUtil.ToDecimalPercentage(value), _progressBar.barImage.fillAmount);
        }
        
        [Test]
        public void UpdatePercentage_ValueInRange_WillSetTextCorrectly()
        {
            const float value = 34.89f;
            var expected = string.Format(ProgressBar.DefaultBarTextFormat, value);
            _progressBar.UpdatePercentage(value);
            
            Assert.AreEqual(expected, _progressBar.barText.text);
        }

        [Test]
        public void UpdatePercentage_Underflow_WillDoNothing()
        {
            const float value = -54.35f;
            const float expectedFill = 0.0f;
            var expectedText = string.Format(ProgressBar.DefaultBarTextFormat, expectedFill);
            _progressBar.UpdatePercentage(value);
            
            Assert.AreEqual(expectedFill, _progressBar.barImage.fillAmount);
            Assert.AreEqual(expectedText, _progressBar.barText.text);
        }
        
        [Test]
        public void UpdatePercentage_Overflow_WillDoNothing()
        {
            const float value = 129.71f;
            const float expectedFill = 0.0f;
            var expectedText = string.Format(ProgressBar.DefaultBarTextFormat, expectedFill);
            _progressBar.UpdatePercentage(value);
            
            Assert.AreEqual(expectedFill, _progressBar.barImage.fillAmount);
            Assert.AreEqual(expectedText, _progressBar.barText.text);
        }

        [Test]
        public void GetPercentage_WillReturnPercentage()
        {
            const float percentage = 38.49f;
            _progressBar.UpdatePercentage(percentage);
            
            Assert.AreEqual(percentage, _progressBar.GetPercentage());
        }

        [Test]
        public void SetBarText_WillSetBarText()
        {
            const string title = "bar text title";
            
            _progressBar.SetBarText(title);
            
            Assert.AreEqual(title, _progressBar.barText.text);
        }
    }
}