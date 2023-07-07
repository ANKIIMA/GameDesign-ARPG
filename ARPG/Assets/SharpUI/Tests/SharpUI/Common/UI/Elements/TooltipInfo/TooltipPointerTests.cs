using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.TooltipInfo;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.TooltipInfo
{
    public class TooltipPointerTests
    {
        private const float Width = 248f;
        private const float Height = 386f;
        
        private TooltipPointer _tooltipPointer;

        private GameObject _gameObject;
        private readonly Vector3 _tooltipLocalPosition = new Vector3(12, 34, 56);
        private readonly Vector2 _sizeDelta = new Vector2(Width, Height);
        private readonly Vector2 _pivot = new Vector2(18, 46);
        private RectTransform _tooltipRectTransform;
        
        private GameObject _parentGameObject;
        private readonly Vector3 _parentLocalPosition = new Vector3(34, 66, 84);
        private readonly Vector2 _parentSizeDelta = new Vector2(388, 232);
        private readonly Vector2 _parentPivot = new Vector2(54, 124);
        private RectTransform _parentRectTransform;

        [SetUp]
        public void SetUp()
        {
            CreateTooltipPointer();
        }

        private void CreateTooltipPointer()
        {
            _gameObject = new GameObject();
            _parentGameObject = new GameObject();
            
            _parentRectTransform = _parentGameObject.AddComponent<RectTransform>();
            _parentRectTransform.sizeDelta = _parentSizeDelta;
            _parentRectTransform.localPosition = _parentLocalPosition;
            _parentRectTransform.pivot = _parentPivot;
            
            _gameObject.transform.SetParent(_parentGameObject.transform);
            _tooltipPointer = _gameObject.AddComponent<TooltipPointer>();
            _tooltipRectTransform = _gameObject.AddComponent<RectTransform>();
            _tooltipRectTransform.sizeDelta = _sizeDelta;
            _tooltipRectTransform.localPosition = _tooltipLocalPosition;
            _tooltipRectTransform.pivot = _pivot;
            _gameObject.GetComponent<TooltipPointer>().Awake();
        }

        private void StartTooltipPointer()
        {
            var tooltipPointer = _gameObject.GetComponent<TooltipPointer>();
            tooltipPointer.Start();
        }

        [Test]
        public void Width_willReturnCorrectSize()
        {
            StartTooltipPointer();
            var width = _tooltipPointer.Width;
            
            Assert.AreEqual(Width, width);
        }
        
        [Test]
        public void Height_willReturnCorrectSize()
        {
            StartTooltipPointer();
            var height = _tooltipPointer.Height;

            Assert.AreEqual(Height, height);
        }

        [Test]
        public void SetPosition_WithPositionLeft_WillApplyLeftOffset()
        {
            _tooltipPointer.SetPosition(PointerPosition.Left);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(-110f, position.x);
            Assert.AreEqual(227f, position.y);
            Assert.AreEqual(56f, position.z);
        }
        
        [Test]
        public void SetPosition_WithPositionRight_WillApplyRightOffset()
        {
            _tooltipPointer.SetPosition(PointerPosition.Right);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(522f, position.x);
            Assert.AreEqual(227f, position.y);
            Assert.AreEqual(56f, position.z);
        }
        
        [Test]
        public void SetPosition_WithPositionTop_WillApplyTopOffset()
        {
            _tooltipPointer.SetPosition(PointerPosition.Top);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(207f, position.x);
            Assert.AreEqual(388f, position.y);
            Assert.AreEqual(56f, position.z);
        }
        
        [Test]
        public void SetPosition_WithPositionBottom_WillApplyBottomOffset()
        {
            _tooltipPointer.SetPosition(PointerPosition.Bottom);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(207f, position.x);
            Assert.AreEqual(-88f, position.y);
            Assert.AreEqual(56f, position.z);
        }

        [Test]
        public void SetPosition_WithPositionLeft_WillApplyCorrectRotation()
        {
            _tooltipPointer.SetPosition(PointerPosition.Left);

            Assert.AreEqual(TooltipPointer.LeftAngleZ, _tooltipRectTransform.eulerAngles.z);
        }
        
        [Test]
        public void SetPosition_WithPositionRight_WillApplyCorrectRotation()
        {
            _tooltipPointer.SetPosition(PointerPosition.Right);

            Assert.AreEqual(TooltipPointer.RightAngleZ, _tooltipRectTransform.eulerAngles.z);
        }
        
        [Test]
        public void SetPosition_WithPositionTop_WillApplyCorrectRotation()
        {
            _tooltipPointer.SetPosition(PointerPosition.Top);

            Assert.AreEqual(TooltipPointer.TopAngleZ, _tooltipRectTransform.eulerAngles.z);
        }
        
        [Test]
        public void SetPosition_WithPositionBottom_WillApplyCorrectRotation()
        {
            _tooltipPointer.SetPosition(PointerPosition.Bottom);

            Assert.AreEqual(TooltipPointer.BottomAngleZ, _tooltipRectTransform.eulerAngles.z);
        }
        
        [Test]
        public void SetOffsetPercentage_To50_WithPositionLeft_WillOffsetCorrectly()
        {
            var percentage = 50f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Left);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(150f, position.y);
        }
        
        [Test]
        public void SetOffsetPercentage_To100_WithPositionLeft_WillOffsetCorrectly()
        {
            var percentage = 100f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Left);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(73f, position.y);
        }
        
        [Test]
        public void SetOffsetPercentage_To50_WithPositionRight_WillOffsetCorrectly()
        {
            var percentage = 50f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Right);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(150f, position.y);
        }
        
        [Test]
        public void SetOffsetPercentage_To100_WithPositionRight_WillOffsetCorrectly()
        {
            var percentage = 100f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Right);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(73f, position.y);
        }
        
        [Test]
        public void SetOffsetPercentage_To50_WithPositionTop_WillOffsetCorrectly()
        {
            var percentage = 50f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Top);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(206f, position.x);
        }
        
        [Test]
        public void SetOffsetPercentage_To100_WithPositionTop_WillOffsetCorrectly()
        {
            var percentage = 100f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Top);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(205f, position.x);
        }
        
        [Test]
        public void SetOffsetPercentage_To50_WithPositionBottom_WillOffsetCorrectly()
        {
            var percentage = 50f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Bottom);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(206f, position.x);
        }
        
        [Test]
        public void SetOffsetPercentage_To100_WithPositionBottom_WillOffsetCorrectly()
        {
            var percentage = 100f;
            _tooltipPointer.SetOffsetPercentage(percentage);
            _tooltipPointer.SetPosition(PointerPosition.Bottom);

            var position = _tooltipRectTransform.localPosition;
            Assert.AreEqual(205f, position.x);
        }

        [Test]
        public void SetOffsetPercentage_WhenNegative_WillNotChange()
        {
            _tooltipPointer.SetOffsetPercentage(50);
            _tooltipPointer.SetPosition(PointerPosition.Left);
            
            Assert.AreEqual(150f, _tooltipRectTransform.localPosition.y);
            
            _tooltipPointer.SetOffsetPercentage(-55f);
            _tooltipPointer.SetPosition(PointerPosition.Left);
            Assert.AreEqual(150f, _tooltipRectTransform.localPosition.y);
        }
        
        [Test]
        public void SetOffsetPercentage_WhenOverflow_WillNotChange()
        {
            _tooltipPointer.SetOffsetPercentage(50);
            _tooltipPointer.SetPosition(PointerPosition.Left);
            
            Assert.AreEqual(150f, _tooltipRectTransform.localPosition.y);
            
            _tooltipPointer.SetOffsetPercentage(155f);
            _tooltipPointer.SetPosition(PointerPosition.Left);
            Assert.AreEqual(150f, _tooltipRectTransform.localPosition.y);
        }

        [Test]
        public void OffsetSize_UnknownPosition_WillReturnZero()
        {
            _tooltipPointer.SetOffsetPercentage(50);
            _tooltipPointer.SetPosition((PointerPosition)(-1));

            Assert.AreEqual(0, _tooltipPointer.OffsetSize());
        }

        [Test]
        public void OffsetSize_WhenLeft_WillReturnCorrectValue()
        {
            _tooltipPointer.SetOffsetPercentage(50);
            _tooltipPointer.SetPosition(PointerPosition.Left);
            
            Assert.AreEqual(-77f, _tooltipPointer.OffsetSize());
        }
        
        [Test]
        public void OffsetSize_WhenRight_WillReturnCorrectValue()
        {
            _tooltipPointer.SetOffsetPercentage(50);
            _tooltipPointer.SetPosition(PointerPosition.Right);
            
            Assert.AreEqual(-77f, _tooltipPointer.OffsetSize());
        }

        [Test]
        public void OffsetSize_WhenTop_WillReturnCorrectValue()
        {
            _tooltipPointer.SetOffsetPercentage(50);
            _tooltipPointer.SetPosition(PointerPosition.Top);
            
            Assert.AreEqual(-1f, _tooltipPointer.OffsetSize());
        }
        
        [Test]
        public void OffsetSize_WhenBottom_WillReturnCorrectValue()
        {
            _tooltipPointer.SetOffsetPercentage(50);
            _tooltipPointer.SetPosition(PointerPosition.Bottom);
            
            Assert.AreEqual(-1f, _tooltipPointer.OffsetSize());
        }
    }
}