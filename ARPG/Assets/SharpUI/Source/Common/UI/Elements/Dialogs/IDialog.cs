using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Dialogs
{
    public interface IDialog
    {
        void Show();
        void Hide();
        void Close();
        void SetCloseButtonVisible(bool visible);
        void SetCustomSprite(Sprite sprite);
        void SetIconType(DialogIconType type);
        DialogIconType GetIconType();
        void SetCustomContent(RectTransform customContentTransform);
        void ClearContent();
        void SetPositiveButtonText(string text);
        void SetNegativeButtonText(string text);
        void SetNeutralButtonText(string text);
        void SetPositiveButtonVisible(bool visible);
        void SetNegativeButtonVisible(bool visible);
        void SetNeutralButtonVisible(bool visible);
        void HideDefaultButtons();
        void ShowDefaultButtons();
        void SetTitle(string title);
        void SetDescription(string description);
        void SetDescriptionVisibility(bool visible);
    }
}