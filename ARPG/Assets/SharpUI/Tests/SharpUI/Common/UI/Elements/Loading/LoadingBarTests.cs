using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Loading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Loading
{
    public class LoadingBarTests
    {
        private LoadingBar _loadingBar;

        [SetUp]
        public void SetUp()
        {
            _loadingBar = new GameObject().AddComponent<LoadingBar>();
            _loadingBar.barImage = new GameObject().AddComponent<Image>();
            _loadingBar.barText = new GameObject().AddComponent<TextMeshPro>();
            _loadingBar.pivotImage = new GameObject().AddComponent<Image>();
            _loadingBar.Start();
        }

        [Test]
        public void UpdatePercentage_WillUpdatePercentage()
        {
            const float percentage = 48.9f;
            _loadingBar.UpdatePercentage(percentage);
            
            Assert.AreEqual(percentage, _loadingBar.GetPercentage());
        }
    }
}