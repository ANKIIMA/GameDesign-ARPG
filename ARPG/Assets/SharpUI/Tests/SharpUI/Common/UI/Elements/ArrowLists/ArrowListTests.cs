using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.ArrowLists;
using SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter;
using SharpUI.Source.Common.UI.Elements.ArrowLists.Animation;
using SharpUI.Source.Common.UI.Elements.Button;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.ArrowLists
{
    public class ArrowListTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private IArrowListAnimator _textAnimator;
        private IArrowListAdapter _adapter;
        private ArrowList _arrowList;
        
        private TMP_Text _text;
        private RectButton _leftButton, _rightButton;
        private string _title = "Title";

        [SetUp]
        public void SetUp()
        {
            _text = new GameObject().AddComponent<TextMeshPro>();
            _leftButton = new GameObject().AddComponent<RectButton>();
            _rightButton = new GameObject().AddComponent<RectButton>();
            
            _leftButton.isClickable = true;
            _leftButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            _leftButton.Awake();
            _leftButton.Start();
            _rightButton.isClickable = true;
            _rightButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
            _rightButton.Awake();
            _rightButton.Start();
            
            _textAnimator = Substitute.For<IArrowListAnimator>();
            _arrowList = new GameObject().AddComponent<ArrowList>();
            _arrowList.itemText = _text;
            _arrowList.leftButton = _leftButton;
            _arrowList.rightButton = _rightButton;
            _arrowList.SetDisposable(_disposable);
            _arrowList.SetArrowListAnimator(_textAnimator);
        }

        [Test]
        public void Start_WillBindTextComponent()
        {
            _arrowList.Start();
            
            _textAnimator.Received().BindTextComponent(_text);
        }

        [Test]
        public void SetAdapter_WillObserveDataChange()
        {
            _adapter = Substitute.For<IArrowListAdapter>();
            _arrowList.SetAdapter(_adapter);
            _arrowList.Start();
            
            _adapter.Received().ObserveDataChange();
        }

        [Test]
        public void SetAdapter_WillObserveSelectionChange()
        {
            _adapter = Substitute.For<IArrowListAdapter>();
            _arrowList.SetAdapter(_adapter);
            _arrowList.Start();
            
            _adapter.Received().ObserveSelectionChange();
        }

        [Test]
        public void WhenHavePreviousItem_WillEnableLeftButton()
        {
            _leftButton.DisableButton();
            _adapter = Substitute.For<IArrowListAdapter>();
            _adapter.HasPreviousData().Returns(true);
            _arrowList.SetAdapter(_adapter);

            _arrowList.Start();
            
            Assert.IsTrue(_leftButton.GetState().IsEnabled());
        }
        
        [Test]
        public void WhenNoPreviousItem_WillDisableLeftButton()
        {
            _leftButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            _leftButton.EnableButton();
            _adapter = Substitute.For<IArrowListAdapter>();
            _adapter.HasPreviousData().Returns(false);
            _arrowList.SetAdapter(_adapter);

            _arrowList.Start();
            
            Assert.IsTrue(_leftButton.GetState().IsDisabled());
        }

        [Test]
        public void WhenHaveNextData_WillEnableRightButton()
        {
            _rightButton.DisableButton();
            _adapter = Substitute.For<IArrowListAdapter>();
            _adapter.HasNextData().Returns(true);
            _arrowList.SetAdapter(_adapter);
            
            _arrowList.Start();
            
            Assert.IsTrue(_rightButton.GetState().IsEnabled());
        }
        
        [Test]
        public void WhenNoNextData_WillDisableRightButton()
        {
            _rightButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
            _rightButton.EnableButton();
            _adapter = Substitute.For<IArrowListAdapter>();
            _adapter.HasNextData().Returns(false);
            _arrowList.SetAdapter(_adapter);
            
            _arrowList.Start();
            
            Assert.IsTrue(_rightButton.GetState().IsDisabled());
        }

        [Test]
        public void OnDestroy_WillUnbindTextAnimator()
        {
            _arrowList.OnDestroy();
            
            _textAnimator.Received().Unbind();
        }

        [Test]
        public void WhenCurrentItemNotNull_WillRenderText()
        {
            _adapter = Substitute.For<IArrowListAdapter>();
            _adapter.CurrentItem().Returns(_title);
            
            _arrowList.Start();
            _arrowList.SetAdapter(_adapter);
            
            Assert.AreEqual(_arrowList.itemText.text, _title);
        }
        
        [Test]
        public void WhenCurrentItemIsNull_WillRenderEmptyText()
        {
            _adapter = Substitute.For<IArrowListAdapter>();
            
            _arrowList.Start();
            _arrowList.SetAdapter(_adapter);
            
            Assert.AreEqual(_arrowList.itemText.text, "");
        }

        [Test]
        public void WhenLeftClicked_AndAnimating_WillNotAnimate()
        {
            _textAnimator.IsAnimating().Returns(true);
            _adapter = Substitute.For<IArrowListAdapter>();
            _arrowList.Start();
            _arrowList.SetAdapter(_adapter);

            _leftButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            
            _textAnimator.DidNotReceive().SlideRight();
            _textAnimator.DidNotReceive().SlideLeft();
        }

        [Test]
        public void WhenLeftClicked_AndNotAnimating_WillAnimateToRight()
        {
            _adapter = Substitute.For<IArrowListAdapter>();
            _arrowList.Start();
            _arrowList.SetAdapter(_adapter);

            _leftButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            
            _textAnimator.Received().SlideRight();
        }
        
        [Test]
        public void WhenRightClicked_AndAnimating_WillNotAnimate()
        {
            _textAnimator.IsAnimating().Returns(true);
            _adapter = Substitute.For<IArrowListAdapter>();
            _arrowList.Start();
            _arrowList.SetAdapter(_adapter);

            _rightButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            
            _textAnimator.DidNotReceive().SlideRight();
            _textAnimator.DidNotReceive().SlideLeft();
        }
        
        [Test]
        public void WhenRightClicked_AndNotAnimating_WillAnimateToLeft()
        {
            _adapter = Substitute.For<IArrowListAdapter>();
            _arrowList.Start();
            _arrowList.SetAdapter(_adapter);

            _rightButton.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            
            _textAnimator.Received().SlideLeft();
        }
    }
}