using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.ModalViews;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.ModelViews
{
    public class ModalViewTests
    {
        private GameObject _gameObject;
        private ModalView _modalView;
        private IconButton _closeButton, _collapseButton;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _gameObject.AddComponent<RectTransform>();
            _modalView = _gameObject.AddComponent<ModalView>();
            
            _closeButton = new GameObject().AddComponent<IconButton>();
            _collapseButton = new GameObject().AddComponent<IconButton>();
            _collapseButton.iconImage = new GameObject().AddComponent<Image>();
            _modalView.closeButton = _closeButton;
            _modalView.collapseButton = _collapseButton;
            _modalView.headerRectTransform = new GameObject().AddComponent<RectTransform>();
            _modalView.contentRectTransform = new GameObject().AddComponent<RectTransform>();
            _modalView.backgroundRectTransform = new GameObject().AddComponent<RectTransform>();
            _modalView.isCollapsed = false;
            _modalView.isDraggable = true;
            _modalView.showBackground = true;

            _closeButton.isClickable = true;
            _closeButton.Awake();
            _closeButton.Start();
            _collapseButton.isClickable = true;
            _collapseButton.Awake();
            _collapseButton.Start();
        }

        [Test]
        public void Start_WhenNotCollapsed_WillShowContent()
        {
            _modalView.Start();
            
            Assert.IsTrue(_modalView.contentRectTransform.gameObject.activeSelf);
        }
        
        [Test]
        public void Start_WhenCollapsed_WillHideContent()
        {
            _modalView.isCollapsed = true;
            
            _modalView.Start();
            
            Assert.IsFalse(_modalView.contentRectTransform.gameObject.activeSelf);
        }
        
        [Test]
        public void Start_WhenNotCollapsed_WillShowBackground()
        {
            _modalView.Start();
            
            Assert.IsTrue(_modalView.backgroundRectTransform.gameObject.activeSelf);
        }
        
        [Test]
        public void Start_WhenCollapsed_WillNotShowBackground()
        {
            _modalView.isCollapsed = true;

            _modalView.Start();
            
            Assert.IsFalse(_modalView.backgroundRectTransform.gameObject.activeSelf);
        }

        [Test]
        public void WhenCloseClicked_WillCLoseModalView()
        {
            _modalView.Start();
            
            _modalView.closeButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            
            Assert.IsFalse(_modalView);
        }

        [Test]
        public void WhenCollapseClicked_WillToggleCollapsed()
        {
            _modalView.Start();
            
            _modalView.collapseButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            
            Assert.IsFalse(_modalView.contentRectTransform.gameObject.activeSelf);
            Assert.IsFalse(_modalView.backgroundRectTransform.gameObject.activeSelf);
        }
    }
}