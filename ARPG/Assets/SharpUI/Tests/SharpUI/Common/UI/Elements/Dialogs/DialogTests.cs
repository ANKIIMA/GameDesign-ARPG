using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Dialogs;
using SharpUI.Source.Common.UI.Util.Layout;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Dialogs
{
    public class DialogTests
    {
        private const string Title = "Title";
        private const string Description = "Some description...";
        
        private RectTransform _contentTransform;
        private GameObject _gameObject;
        private Dialog _dialog;
        private ILayoutHelper _layoutHelper;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _layoutHelper = Substitute.For<ILayoutHelper>();
            _contentTransform = new GameObject().AddComponent<RectTransform>();
            _dialog = _gameObject.AddComponent<Dialog>();
            _dialog.contentTransform = _contentTransform;
            _dialog.closeIconButton = new GameObject().AddComponent<UnityEngine.UI.Button>();
            _dialog.titleText = new GameObject().AddComponent<TextMeshPro>();
            _dialog.descriptionText = new GameObject().AddComponent<TextMeshPro>();
            _dialog.defaultButtonsContainer = new GameObject();
            _dialog.buttonPositive = new GameObject().AddComponent<RectButton>();
            _dialog.buttonNegative = new GameObject().AddComponent<RectButton>();
            _dialog.buttonNeutral = new GameObject().AddComponent<RectButton>();
            _dialog.buttonPositive.gameObject.AddComponent<TextMeshPro>();
            _dialog.buttonNegative.gameObject.AddComponent<TextMeshPro>();
            _dialog.buttonNeutral.gameObject.AddComponent<TextMeshPro>();
            _dialog.closeIconButton = new GameObject().AddComponent<UnityEngine.UI.Button>();
            _dialog.iconImage = new GameObject().AddComponent<Image>();
            _dialog.spriteInfo = Sprite.Create(Texture2D.redTexture, Rect.zero, Vector2.one);
            _dialog.spriteQuestion = Sprite.Create(Texture2D.grayTexture, Rect.zero, Vector2.one);
            _dialog.spriteWarning = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.one);
            _dialog.spriteCustom = Sprite.Create(Texture2D.whiteTexture, Rect.zero, Vector2.one);

            _dialog.Start();
            _dialog.SetLayoutHelper(_layoutHelper);
        }

        [Test]
        public void WhenCloseButtonClicked_WillDestroyDialog()
        {
            _dialog.closeIconButton.onClick.Invoke();
            
            Assert.IsFalse(_dialog);
        }

        [Test]
        public void WhenCloseButtonClicked_WillDestroyDialogGameObject()
        {
            _dialog.closeIconButton.onClick.Invoke();
            
            Assert.IsFalse(_gameObject);
        }

        [Test]
        public void Show_WillShowDialog()
        {
            _gameObject.SetActive(false);
            
            _dialog.Show();
            
            Assert.IsTrue(_gameObject.activeSelf);
        }
        
        [Test]
        public void Hide_WillHideDialog()
        {
            _gameObject.SetActive(true);
            
            _dialog.Hide();
            
            Assert.IsFalse(_gameObject.activeSelf);
        }

        [Test]
        public void Close_WillDestroyDialog()
        {
            _dialog.Close();
            
            Assert.IsFalse(_dialog);
            Assert.IsFalse(_gameObject);
        }

        [Test]
        public void SetTitle_WillSetTitle()
        {
            _dialog.SetTitle(Title);
            
            Assert.AreEqual(Title, _dialog.titleText.text);
        }

        [Test]
        public void SetDescription_WillSetDescription()
        {
            _dialog.SetDescription(Description);
            
            Assert.AreEqual(Description, _dialog.descriptionText.text);
        }
        
        [Test]
        public void SetDescription_WillRebuildLayout()
        {
            _dialog.SetDescription(Description);
            
            _layoutHelper.Received().ForceRebuildLayoutImmediate(_contentTransform);
        }

        [Test]
        public void SetDescriptionVisibility_ToTrue_WillShowDescription()
        {
            _dialog.descriptionText.gameObject.SetActive(false);
            
            _dialog.SetDescriptionVisibility(true);
            
            Assert.IsTrue(_dialog.descriptionText.gameObject.activeSelf);
        }
        
        [Test]
        public void SetDescriptionVisibility_ToFalse_WillHideDescription()
        {
            _dialog.descriptionText.gameObject.SetActive(true);
            
            _dialog.SetDescriptionVisibility(false);
            
            Assert.IsFalse(_dialog.descriptionText.gameObject.activeSelf);
        }

        [Test]
        public void HideDefaultButtons_WillHideButtonsContainer()
        {
            _dialog.defaultButtonsContainer.SetActive(true);
            _dialog.HideDefaultButtons();
            
            Assert.IsFalse(_dialog.defaultButtonsContainer.activeSelf);
        }
        
        [Test]
        public void ShowDefaultButtons_WillHideButtonsContainer()
        {
            _dialog.defaultButtonsContainer.SetActive(false);
            _dialog.ShowDefaultButtons();
            
            Assert.IsTrue(_dialog.defaultButtonsContainer.activeSelf);
        }

        [Test]
        public void SetPositiveButtonText_WillSetTextCorrectly()
        {
            _dialog.SetPositiveButtonText(Title);
            
            Assert.AreEqual(Title, _dialog.buttonPositive.GetComponentInChildren<TMP_Text>().text);
        }

        [Test]
        public void SetNegativeButtonText_WillSetTextCorrectly()
        {
            _dialog.SetNegativeButtonText(Title);
            
            Assert.AreEqual(Title, _dialog.buttonNegative.GetComponentInChildren<TMP_Text>().text);
        }
        
        [Test]
        public void SetNeutralButtonText_WillSetTextCorrectly()
        {
            _dialog.SetNeutralButtonText(Title);
            
            Assert.AreEqual(Title, _dialog.buttonNeutral.GetComponentInChildren<TMP_Text>().text);
        }

        [Test]
        public void SetPositiveButtonVisible_ToTrue_WillBeVisible()
        {
            _dialog.buttonPositive.gameObject.SetActive(false);
            _dialog.SetPositiveButtonVisible(true);
            
            Assert.IsTrue(_dialog.buttonPositive.gameObject.activeSelf);
        }
        
        [Test]
        public void SetPositiveButtonVisible_ToFalse_WillBeHidden()
        {
            _dialog.buttonPositive.gameObject.SetActive(true);
            _dialog.SetPositiveButtonVisible(false);
            
            Assert.IsFalse(_dialog.buttonPositive.gameObject.activeSelf);
        }
        
        [Test]
        public void SetNegativeButtonVisible_ToTrue_WillBeVisible()
        {
            _dialog.buttonNegative.gameObject.SetActive(false);
            _dialog.SetNegativeButtonVisible(true);
            
            Assert.IsTrue(_dialog.buttonNegative.gameObject.activeSelf);
        }
        
        [Test]
        public void SetNegativeButtonVisible_ToFalse_WillBeHidden()
        {
            _dialog.buttonNegative.gameObject.SetActive(true);
            _dialog.SetNegativeButtonVisible(false);
            
            Assert.IsFalse(_dialog.buttonNegative.gameObject.activeSelf);
        }
        
        [Test]
        public void SetNeutralButtonVisible_ToTrue_WillBeVisible()
        {
            _dialog.buttonNeutral.gameObject.SetActive(false);
            _dialog.SetNeutralButtonVisible(true);
            
            Assert.IsTrue(_dialog.buttonNeutral.gameObject.activeSelf);
        }
        
        [Test]
        public void SetNeutralButtonVisible_ToFalse_WillBeHidden()
        {
            _dialog.buttonNeutral.gameObject.SetActive(true);
            _dialog.SetNeutralButtonVisible(false);
            
            Assert.IsFalse(_dialog.buttonNeutral.gameObject.activeSelf);
        }

        [Test]
        public void ClearContent_WillClearContent()
        {
            var parentTransform = _dialog.contentTransform.transform;
            var item1 = new GameObject().AddComponent<RectTransform>();
            var item2 = new GameObject().AddComponent<RectTransform>();
            item1.gameObject.SetActive(true);
            item2.gameObject.SetActive(true);
            item1.SetParent(parentTransform);
            item2.SetParent(parentTransform);
            
            _dialog.ClearContent();
            
            Assert.IsFalse(item1.gameObject.activeSelf);
            Assert.IsFalse(item2.gameObject.activeSelf);
        }

        [Test]
        public void SetCustomContent_WillSetContent()
        {
            var customContentRectTransform = new GameObject().AddComponent<RectTransform>();
            
            _dialog.SetCustomContent(customContentRectTransform);
            
            Assert.AreSame(customContentRectTransform.parent, _dialog.contentTransform);
        }

        [Test]
        public void SetCustomSprite_WillSetCustomSpriteCorrectly()
        {
            var sprite = Sprite.Create(Texture2D.redTexture, Rect.zero, Vector2.one);

            _dialog.SetCustomSprite(sprite);
            
            Assert.AreSame(sprite, _dialog.spriteCustom);
        }

        [Test]
        public void SetCloseButtonVisible_ToTrue_WillBeVisible()
        {
            _dialog.closeIconButton.gameObject.SetActive(false);

            _dialog.SetCloseButtonVisible(true);
            
            Assert.IsTrue(_dialog.closeIconButton.gameObject.activeSelf);
        }
        
        [Test]
        public void SetCloseButtonVisible_ToFalse_WillBeHidden()
        {
            _dialog.closeIconButton.gameObject.SetActive(true);

            _dialog.SetCloseButtonVisible(false);
            
            Assert.IsFalse(_dialog.closeIconButton.gameObject.activeSelf);
        }

        [Test]
        public void SetIconType_WhenIconNull_WillHideIconImage()
        {
            const DialogIconType unknownIconType = (DialogIconType) int.MaxValue;
            
            _dialog.SetIconType(unknownIconType);
            
            Assert.IsFalse(_dialog.iconImage.gameObject.activeSelf);
        }
        
        [Test]
        public void SetIconType_WhenInfo_WillSetIconSpriteInfo()
        {
            _dialog.SetIconType(DialogIconType.Info);
            
            Assert.AreSame(_dialog.spriteInfo, _dialog.iconImage.sprite);
        }
        
        [Test]
        public void SetIconType_WhenQuestion_WillSetIconSpriteQuestion()
        {
            _dialog.SetIconType(DialogIconType.Question);
            
            Assert.AreSame(_dialog.spriteQuestion, _dialog.iconImage.sprite);
        }
        
        [Test]
        public void SetIconType_WhenWarning_WillSetIconSpriteWarning()
        {
            _dialog.SetIconType(DialogIconType.Warning);
            
            Assert.AreSame(_dialog.spriteWarning, _dialog.iconImage.sprite);
        }
        
        [Test]
        public void SetIconType_WhenCustom_WillSetIconSpriteCustom()
        {
            _dialog.SetIconType(DialogIconType.Custom);
            
            Assert.AreSame(_dialog.spriteCustom, _dialog.iconImage.sprite);
        }
        
        [Test]
        public void SetIconType_WhenNone_WillSetIconSpriteNull()
        {
            _dialog.SetIconType(DialogIconType.None);
            
            Assert.IsNull(_dialog.iconImage.sprite);
        }

        [Test]
        public void GetIconType_WillReturnCorrectIconType()
        {
            _dialog.SetIconType(DialogIconType.Info);
            
            Assert.AreEqual(DialogIconType.Info, _dialog.GetIconType());
        }
    }
}