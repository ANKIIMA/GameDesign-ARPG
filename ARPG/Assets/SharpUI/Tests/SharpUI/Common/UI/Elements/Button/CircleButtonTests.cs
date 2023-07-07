using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Button
{
    public class CircleButtonTests
    {
        private const float ExpectedRadius = 50.0f;
        private const float ExpectedRectWidth = 100.0f;
        private const float ExpectedRectHeight = 100.0f;
        private CircleButton _circleButton;

        [SetUp]
        public void SetUp()
        {
            _circleButton = new GameObject().AddComponent<CircleButton>();
            _circleButton.gameObject.AddComponent<RectTransform>();
            _circleButton.Awake();
            _circleButton.Start();
        }

        [Test]
        public void ColliderData_IsValidForTest()
        {
            var rectTransformWidth = _circleButton.GetComponent<RectTransform>().rect.width;
            var rectTransformHeight = _circleButton.GetComponent<RectTransform>().rect.height;
            var radius = _circleButton.GetComponent<CircleCollider2D>().radius;
            
            Assert.AreEqual(ExpectedRadius, radius);
            Assert.AreEqual(ExpectedRectWidth, rectTransformWidth);
            Assert.AreEqual(ExpectedRectHeight, rectTransformHeight);
        }

        [Test]
        public void IsRaycastLocationValid_IsValid_WhenInsideCircleCollider()
        {
            var radius = _circleButton.GetComponent<CircleCollider2D>().radius;
            var pointerPosition = new Vector2(radius/2, radius/2);
            
            var valid = _circleButton.IsRaycastLocationValid(pointerPosition, _circleButton.GetComponent<Camera>());
            
            Assert.IsTrue(valid);
        }
        
        [Test]
        public void IsRaycastLocationValid_IsNotValid_WhenOutsideCircleCollider()
        {
            var radius = _circleButton.GetComponent<CircleCollider2D>().radius;
            var pointerPosition = new Vector2(radius*4, radius*4);
            
            var valid = _circleButton.IsRaycastLocationValid(pointerPosition, _circleButton.GetComponent<Camera>());
            
            Assert.IsFalse(valid);
        }
    }
}