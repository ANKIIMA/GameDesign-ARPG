using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Util.Layout;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Dialogs
{
    public class Dialog : MonoBehaviour, IDialog
    {
        [SerializeField] public Sprite spriteInfo;
        [SerializeField] public Sprite spriteQuestion;
        [SerializeField] public Sprite spriteWarning;
        [SerializeField] public Sprite spriteCustom;
        [SerializeField] public TMP_Text titleText;
        [SerializeField] public TMP_Text descriptionText;
        [SerializeField] public Image iconImage;
        [SerializeField] public UnityEngine.UI.Button closeIconButton;
        [SerializeField] public RectTransform contentTransform;
        [SerializeField] public GameObject defaultButtonsContainer;
        [SerializeField] public RectButton buttonPositive;
        [SerializeField] public RectButton buttonNegative;
        [SerializeField] public RectButton buttonNeutral;

        private DialogIconType _iconType;
        private ILayoutHelper _layoutHelper = new LayoutHelper();

        public void Start()
        {
            closeIconButton.OnClickAsObservable().SubscribeWith(this, _ => DestroyDialog());
        }

        private void DestroyDialog()
        {
            DestroyImmediate(gameObject);
        }

        public void SetLayoutHelper(ILayoutHelper layoutHelper) => _layoutHelper = layoutHelper;

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Close()
        {
            DestroyDialog();
        }

        public void SetTitle(string title)
        {
            titleText.text = title;
        }

        public void SetDescription(string description)
        {
            descriptionText.text = description;
            _layoutHelper.ForceRebuildLayoutImmediate(contentTransform);
        }

        public void SetDescriptionVisibility(bool visible)
        {
            descriptionText.gameObject.SetActive(visible);
        }

        public void HideDefaultButtons() => defaultButtonsContainer.SetActive(false);

        public void ShowDefaultButtons() => defaultButtonsContainer.SetActive(true);

        public void SetPositiveButtonText(string text)
        {
            buttonPositive.GetComponentInChildren<TMP_Text>().text = text;
        }

        public void SetNegativeButtonText(string text)
        {
            buttonNegative.GetComponentInChildren<TMP_Text>().text = text;
        }

        public void SetNeutralButtonText(string text)
        {
            buttonNeutral.GetComponentInChildren<TMP_Text>().text = text;
        }

        public void SetPositiveButtonVisible(bool visible)
        {
            buttonPositive.gameObject.SetActive(visible);
        }

        public void SetNegativeButtonVisible(bool visible)
        {
            buttonNegative.gameObject.SetActive(visible);
        }

        public void SetNeutralButtonVisible(bool visible)
        {
            buttonNeutral.gameObject.SetActive(visible);
        }

        public void SetCustomContent(RectTransform customContentTransform)
        {
            ClearContent();
            customContentTransform.SetParent(contentTransform);
        }

        public void ClearContent()
        {
            foreach (Transform child in contentTransform)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void SetCustomSprite(Sprite sprite)
        {
            spriteCustom = sprite;
        }

        public void SetCloseButtonVisible(bool visible)
        {
            closeIconButton.gameObject.SetActive(visible);
        }

        public void SetIconType(DialogIconType type)
        {
            _iconType = type;
            var sprite = GetIconSprite();
            if (sprite == null)
            {
                iconImage.gameObject.SetActive(false);
            }
            else
            {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = GetIconSprite();
            }
        }

        private Sprite GetIconSprite()
        {
            switch (_iconType)
            {
                case DialogIconType.Info: return spriteInfo;
                case DialogIconType.Question: return spriteQuestion;
                case DialogIconType.Warning: return spriteWarning;
                case DialogIconType.Custom: return spriteCustom;
                case DialogIconType.None: return null;
                default: return null;
            }
        }

        public DialogIconType GetIconType() => _iconType;
    }
}