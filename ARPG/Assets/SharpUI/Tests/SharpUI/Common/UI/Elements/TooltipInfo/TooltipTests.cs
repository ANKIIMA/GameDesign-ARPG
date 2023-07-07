using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.TooltipInfo;
using SharpUI.Source.Common.UI.Util.Layout;
using SharpUI.Source.Common.Util.Reactive;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.TooltipInfo
{
    public class TooltipTests
    {
        private const float ContentWidth = 27;
        private const float ContentHeight = 58;
        private const float ContentMarginLeft = 11f;
        private const float ContentMarginRight = 14f;
        private const float ContentMarginTop = 38f;
        private const float ContentMarginBottom = 19f;
        private const long DelayTime = 123;
        
        private GameObject _tooltipGameObject;
        private Tooltip _tooltip;
        private RectTransform _rectTransform;
        private ITooltipPointer _tooltipPointer;

        private readonly CompositeDisposable _showDisposable = new CompositeDisposable();
        private GameObject _destinationGameObject;
        private readonly Vector2 _destinationSizeDelta = new Vector2(246, 384);
        private readonly Vector3 _destinationLocalPosition = new Vector3(14, 28, 37);
        private RectTransform _destinationRectTransform;
        private IDelayObserver _delayObserver;

        [SetUp]
        public void SetUp()
        {
            _delayObserver = Substitute.For<IDelayObserver>();
            _delayObserver.DelayMilliseconds(Arg.Any<long>(), Arg.Any<IScheduler>())
                .Returns(Observable.Return((long) 1));
            _tooltipGameObject = new GameObject();
            _tooltip = _tooltipGameObject.AddComponent<Tooltip>();
            _tooltip.SetDelayObserver(_delayObserver);
            _rectTransform = _tooltipGameObject.AddComponent<RectTransform>();
            _rectTransform.pivot = new Vector2(13, 27);
            _rectTransform.sizeDelta = new Vector2(345, 289);
            _tooltipPointer = Substitute.For<ITooltipPointer>();

            _destinationGameObject = new GameObject();
            _destinationRectTransform = _destinationGameObject.AddComponent<RectTransform>();
            _destinationRectTransform.localPosition = _destinationLocalPosition;
            _destinationRectTransform.sizeDelta = _destinationSizeDelta;
            _destinationGameObject.transform.SetParent(new GameObject().AddComponent<RectTransform>());
            _destinationGameObject.transform.parent.localPosition = new Vector3(12, 45, 23);

            _tooltip.Awake();
            _tooltip.SetTooltipPointer(_tooltipPointer);
            _tooltip.Start();
            
            var contentTransform = new GameObject().AddComponent<RectTransform>();
            contentTransform.sizeDelta = new Vector2(ContentWidth, ContentHeight);
            _tooltip.BindContent(contentTransform);
        }

        [Test]
        public void Start_WillPositionPointerToRight()
        {
            _tooltipPointer.Received().SetPosition(PointerPosition.Right);
        }

        [Test]
        public void Start_GameObjectWillBeActive()
        {
            Assert.IsTrue(_tooltipGameObject.activeSelf);
        }

        [Test]
        public void OnDestroy_WillDisposeShowDisposable()
        {
            _tooltip.SetShowDisposable(_showDisposable);
            
            _tooltip.OnDestroy();
            
            Assert.IsTrue(_showDisposable.IsDisposed);
        }

        [Test]
        public void OffsetPointer_WillSetOffsetOnPointer()
        {
            const float offset = 38f;
            _tooltip.OffsetPointerByPercentage(offset);

            _tooltipPointer.Received().SetOffsetPercentage(offset);
        }

        [Test]
        public void Hide_GameObjectWillBeInactive()
        {
            _tooltip.Hide();

            Assert.IsFalse(_tooltipGameObject.activeSelf);
        }
        
        [Test]
        public void Hide_ContentHasNoParent()
        {
            var destinationTransform = new GameObject().AddComponent<RectTransform>();
            _tooltip.BindContent(destinationTransform);
            
            _tooltip.Hide();

            Assert.IsNull(destinationTransform.parent);
        }

        [Test]
        public void SetShowDelayTime_WillDelayCorrectly()
        {
            _tooltip.SetShowDelayTimeMillis(DelayTime);
            
            Assert.AreEqual(DelayTime, _tooltip.GetShowDelayTime());
        }

        [Test]
        public void ShowToLeftOf_WillPositionPointerToRight()
        {
            _tooltip.ShowToLeftOf(_destinationRectTransform);
            
            _tooltipPointer.Received().SetPosition(PointerPosition.Right);
        }
        
        [Test]
        public void ShowToRightOf_WillPositionPointerToLeft()
        {
            _tooltip.ShowToRightOf(_destinationRectTransform);
            
            _tooltipPointer.Received().SetPosition(PointerPosition.Left);
        }
        
        [Test]
        public void ShowAbove_WillPositionPointerOnBottom()
        {
            _tooltip.ShowAbove(_destinationRectTransform);
            
            _tooltipPointer.Received().SetPosition(PointerPosition.Bottom);
        }
        
        [Test]
        public void ShowBelow_WillPositionPointerOnTop()
        {
            _tooltip.ShowBelow(_destinationRectTransform);
            
            _tooltipPointer.Received().SetPosition(PointerPosition.Top);
        }

        [Test]
        public void ShowToLeftOf_WilShowGameObject()
        {
            _tooltipGameObject.SetActive(false);
            _tooltip.ShowToLeftOf(_destinationRectTransform);
            
            Assert.IsTrue(_tooltipGameObject.activeSelf);
        }
        
        [Test]
        public void ShowToRightOf_WilShowGameObject()
        {
            _tooltipGameObject.SetActive(false);
            _tooltip.ShowToRightOf(_destinationRectTransform);
            
            Assert.IsTrue(_tooltipGameObject.activeSelf);
        }
        
        [Test]
        public void ShowAbove_WilShowGameObject()
        {
            _tooltipGameObject.SetActive(false);
            _tooltip.ShowAbove(_destinationRectTransform);
            
            Assert.IsTrue(_tooltipGameObject.activeSelf);
        }
        
        [Test]
        public void ShowBelow_WilShowGameObject()
        {
            _tooltipGameObject.SetActive(false);
            _tooltip.ShowBelow(_destinationRectTransform);
            
            Assert.IsTrue(_tooltipGameObject.activeSelf);
        }

        [Test]
        public void ShowToLeftOf_TransformParentIsCorrect()
        {
            _tooltip.ShowToLeftOf(_destinationRectTransform);
            
            Assert.AreSame(
                _tooltipGameObject.transform.parent,
                _destinationRectTransform.transform.parent);
        }
        
        [Test]
        public void ShowToRightOf_TransformParentIsCorrect()
        {
            _tooltip.ShowToRightOf(_destinationRectTransform);
            
            Assert.AreSame(
                _tooltipGameObject.transform.parent,
                _destinationRectTransform.transform.parent);
        }
        
        [Test]
        public void ShowAbove_TransformParentIsCorrect()
        {
            _tooltip.ShowAbove(_destinationRectTransform);

            Assert.AreSame(
                _tooltipGameObject.transform.parent,
                _destinationRectTransform.transform.parent);
        }
        
        [Test]
        public void ShowBelow_TransformParentIsCorrect()
        {
            _tooltip.ShowBelow(_destinationRectTransform);
            
            Assert.AreSame(
                _tooltipGameObject.transform.parent,
                _destinationRectTransform.transform.parent);
        }
        
        [Test]
        public void BindContent_WillSetParentTransformCorrectly()
        {
            var rectTrans = new GameObject().AddComponent<RectTransform>();
            
            _tooltip.BindContent(rectTrans);
            
            Assert.AreSame(rectTrans.parent, _rectTransform);
        }

        [Test]
        public void BindContent_WillSetContentSizeCorrectly()
        {
            var destinationTransform = new GameObject().AddComponent<RectTransform>();
            destinationTransform.sizeDelta = new Vector2(ContentWidth, ContentHeight);
            _tooltip.SetMargins(new Margin(0, 0, 0, 0));
            
            _tooltip.BindContent(destinationTransform);
            
            Assert.AreEqual(destinationTransform.sizeDelta, _rectTransform.sizeDelta);
        }
        
        [Test]
        public void BindContent_WillSetContentParentCorrectly()
        {
            var destinationTransform = new GameObject().AddComponent<RectTransform>();
            
            _tooltip.BindContent(destinationTransform);
            
            Assert.AreSame(destinationTransform.parent, _rectTransform);
        }

        [Test]
        public void SetMargins_WillApplyMarginsCorrectly()
        {
            var destinationTransform = new GameObject().AddComponent<RectTransform>();
            destinationTransform.sizeDelta = new Vector2(ContentWidth, ContentHeight);
            _tooltip.SetMargins(new Margin(ContentMarginLeft, ContentMarginRight, ContentMarginTop, ContentMarginBottom));
            _tooltip.BindContent(destinationTransform);
            
            var margins = new Vector2(ContentMarginLeft+ContentMarginRight, ContentMarginTop+ContentMarginBottom);
            Assert.AreEqual(destinationTransform.sizeDelta + margins, _rectTransform.sizeDelta);
        }
    }
}