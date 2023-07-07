using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Elements.Dialogs;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Prototype.Dialog
{
    public class DialogPrototype : MonoBehaviour
    {
        private const string _description =
            "Dialog description, Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus id posuere augue."
            +" Pellentesque eget ex laoreet, tincidunt elit sit amet, varius lacus. Quisque faucibus feugiat felis eu"
            +" porta. Praesent feugiat tincidunt magna.";
        
        [SerializeField] public GameObject dialogDefaultPrefab;
        [SerializeField] public Sprite customDialogSprite;
        [SerializeField] public RectTransform customDialogContentTransform;
        [SerializeField] public Transform panelTransform;
        [SerializeField] public Button buttonShow;
        [SerializeField] public Button buttonHide;
        [SerializeField] public Button buttonShowClose;
        [SerializeField] public Button buttonHideClose;
        [SerializeField] public Button buttonCreateDialog;
        [SerializeField] public Button buttonDestroyDialog;
        [SerializeField] public Button buttonAddContent;
        [SerializeField] public Button buttonRemoveContent;
        [SerializeField] public Button buttonSetButtonTexts;
        [SerializeField] public Button buttonTogglePositiveVisible;
        [SerializeField] public Button buttonToggleNegativeVisible;
        [SerializeField] public Button buttonToggleNeutralVisible;
        [SerializeField] public Button buttonToggleDefaultButtonsContainer;
        [SerializeField] public Button buttonToggleDescriptionVisible;
        [SerializeField] public Button buttonSetDescriptionText;
        [SerializeField] public TMP_Dropdown dropdownIcon;

        [CanBeNull] private IDialog _dialog;

        private bool _positiveButtonVisible = true, _negativeButtonVisible = true, _neutralButtonVisible = true,
            _defaultButtonsContainerVisible = true, _descriptionVisible = true, _descriptionToggle = true;
        
        public void Start()
        {
            buttonCreateDialog.OnClickAsObservable().SubscribeWith(this, _ => CreateDialog());
            buttonDestroyDialog.OnClickAsObservable().SubscribeWith(this, _ => DestroyDialog());

            buttonShow.OnClickAsObservable().SubscribeWith(this, _ => _dialog?.Show());
            buttonHide.OnClickAsObservable().SubscribeWith(this, _ => _dialog?.Hide());
            buttonShowClose.OnClickAsObservable().SubscribeWith(this,
                _ => _dialog?.SetCloseButtonVisible(true));
            buttonHideClose.OnClickAsObservable().SubscribeWith(this,
                _ => _dialog?.SetCloseButtonVisible(false));
            
            dropdownIcon.onValueChanged.AsObservable().SubscribeWith(this, SetIcon);
            
            buttonAddContent.OnClickAsObservable().SubscribeWith(this,
                _ => _dialog?.SetCustomContent(customDialogContentTransform));
            buttonRemoveContent.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    _dialog?.ClearContent();
                    customDialogContentTransform.parent = buttonShow.transform.parent;
                    customDialogContentTransform.transform.localPosition = new Vector3(400, 400, 0);
                });
            
            buttonSetButtonTexts.OnClickAsObservable().SubscribeWith(this,
                _ => SetButtonTexts());
            
            buttonTogglePositiveVisible.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    _positiveButtonVisible = !_positiveButtonVisible;
                    _dialog?.SetPositiveButtonVisible(_positiveButtonVisible);
                });
            
            buttonToggleNegativeVisible.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    _negativeButtonVisible = !_negativeButtonVisible;
                    _dialog?.SetNegativeButtonVisible(_negativeButtonVisible);
                });
            
            buttonToggleNeutralVisible.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    _neutralButtonVisible = !_neutralButtonVisible;
                    _dialog?.SetNeutralButtonVisible(_neutralButtonVisible);
                });
            
            buttonToggleDefaultButtonsContainer.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    _defaultButtonsContainerVisible = !_defaultButtonsContainerVisible;
                    if (_defaultButtonsContainerVisible)
                        _dialog?.ShowDefaultButtons();
                    else
                        _dialog?.HideDefaultButtons();
                });
            
            buttonToggleDescriptionVisible.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    _descriptionVisible = !_descriptionVisible;
                    _dialog?.SetDescriptionVisibility(_descriptionVisible);
                });
            
            buttonSetDescriptionText.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    _descriptionToggle = !_descriptionToggle;
                    if (_descriptionToggle)
                        _dialog?.SetDescription(_description);
                    else
                        _dialog?.SetDescription("Description text...");
                });
        }

        private void SetButtonTexts()
        {
            _dialog?.SetPositiveButtonText("Yes");
            _dialog?.SetNegativeButtonText("No");
            _dialog?.SetNeutralButtonText("OK");
        }
        
        private void CreateDialog()
        {
            if (_dialog != null) return;
            
            var dialogInstance = Instantiate(dialogDefaultPrefab, panelTransform);
            _dialog = dialogInstance.GetComponent<IDialog>();
            _dialog?.SetCustomSprite(customDialogSprite);
            _dialog?.SetIconType(GetDialogIconType());
        }

        private void DestroyDialog()
        {
            if (_dialog == null) return;
            
            _dialog.Close();
            _dialog = null;
        }

        private void SetIcon(int index)
        {
            _dialog?.SetIconType(GetDialogIconType());
        }

        private DialogIconType GetDialogIconType()
        {
            switch (dropdownIcon.value)
            {
                case 0: return DialogIconType.Info;
                case 1: return DialogIconType.Question;
                case 2: return DialogIconType.Warning;
                case 3: return DialogIconType.Custom;
                case 4: return DialogIconType.None;
                default: return DialogIconType.None;
            }
        }
    }
}