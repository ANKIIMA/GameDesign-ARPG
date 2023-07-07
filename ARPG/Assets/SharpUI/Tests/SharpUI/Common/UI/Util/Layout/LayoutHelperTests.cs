using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Layout;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Layout
{
    public class LayoutHelperTests
    {
        private readonly Vector2 _rectAnchoredPosition = new Vector2(239, -28);
        private readonly Vector2 _rectSizeDelta = new Vector2(35, 42);
        private readonly Vector2 _parentAnchoredPosition = new Vector2(14, 38);
        private readonly Vector2 _parentSizeDelta = new Vector2(140, 180);
        
        private LayoutHelper _helper;
        private GameObject _rectTransformGameObject;
        private GameObject _parentTransformGameObject;
        private RectTransform _rectTransform;
        private RectTransform _parentTransform;

        [SetUp]
        public void SetUp()
        {
            _helper = new LayoutHelper();
            _rectTransformGameObject = new GameObject();
            _parentTransformGameObject = new GameObject();
            _rectTransform = _rectTransformGameObject.AddComponent<RectTransform>();
            _parentTransform = _parentTransformGameObject.AddComponent<RectTransform>();
            
            _rectTransform.anchoredPosition = _rectAnchoredPosition;
            _rectTransform.sizeDelta = _rectSizeDelta;
            
            _parentTransform.anchoredPosition = _parentAnchoredPosition;
            _parentTransform.sizeDelta = _parentSizeDelta;
        }

        [Test]
        public void ForceRebuildLayoutImmediate_willRebuildLayout()
        {
            _rectTransform.SetParent(_parentTransform);
            
            _helper.ForceRebuildLayoutImmediate(_rectTransform);

            var rectPositionX = _rectAnchoredPosition.x - _parentAnchoredPosition.x;
            var rectPositionY = _rectAnchoredPosition.y - _parentAnchoredPosition.y;
            Assert.AreEqual(_rectTransform.anchoredPosition.x, rectPositionX);
            Assert.AreEqual(_rectTransform.anchoredPosition.y, rectPositionY);
        }
    }
}