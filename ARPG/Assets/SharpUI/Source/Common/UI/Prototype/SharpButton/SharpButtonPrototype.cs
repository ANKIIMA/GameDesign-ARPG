using System.Collections.Generic;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.DropDowns;
using SharpUI.Source.Common.UI.Elements.DropDowns.Adapters;
using SharpUI.Source.Common.UI.Elements.Sliders;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Prototype.SharpButton
{
    public class SharpButtonPrototype : MonoBehaviour
    {
        // Default rect buttons
        [SerializeField] public Button enableDefaultButtons;
        [SerializeField] public Button disableDefaultButtons;
        [SerializeField] public Button toggleClickable;
        [SerializeField] public Button toggleSelectable;
        [SerializeField] public Button toggleSelected;
        [SerializeField] public RectButton buttonDefault;
        [SerializeField] public RectButton buttonGreen;
        [SerializeField] public RectButton buttonRed;

        // Link buttons
        [SerializeField] public Button enableLinkButtons;
        [SerializeField] public Button disableLinkButtons;
        [SerializeField] public RectButton buttonLinkDefault;
        [SerializeField] public RectButton buttonLinkBlue;

        // Icon buttons
        [SerializeField] public Button enableIconButtons;
        [SerializeField] public Button disableIconButtons;
        [SerializeField] public RectButton defaultIconButton;
        [SerializeField] public RectButton largeIconButton;
        
        // Round buttons
        [SerializeField] public Button enableRoundButtons;
        [SerializeField] public Button disableRoundButtons;
        [SerializeField] public IconButton roundButton;
        [SerializeField] public IconButton roundButtonClose;
        [SerializeField] public IconButton roundButtonHero;
        
        // Dropdowns
        [SerializeField] public DropDown dropDown;
        
        // SimpleSlider
        [SerializeField] public SimpleSlider _slider;
        
        public void Start()
        {
            enableDefaultButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    buttonDefault.EnableButton();
                    buttonGreen.EnableButton();
                    buttonRed.EnableButton();
                });
            
            disableDefaultButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    buttonDefault.DisableButton();
                    buttonGreen.DisableButton();
                    buttonRed.DisableButton();
                });

            toggleClickable.GetComponentInChildren<Text>().text = buttonDefault.isClickable ? "Disable clicks" : "Enable clicks";
            toggleSelectable.GetComponentInChildren<Text>().text =
                buttonDefault.isSelectable ? "Disable selection" : "Enable selection";
            toggleSelected.GetComponentInChildren<Text>().text = buttonDefault.isSelected ? "Deselect" : "Select";
            
            toggleClickable.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    if (buttonDefault.isClickable)
                    {
                        buttonDefault.SetClickable(false);
                        buttonGreen.SetClickable(false);
                        buttonRed.SetClickable(false);
                    }
                    else
                    {
                        buttonDefault.SetClickable(true);
                        buttonGreen.SetClickable(true);
                        buttonRed.SetClickable(true);
                    }
                    toggleClickable.GetComponentInChildren<Text>().text = buttonDefault.isClickable ? "Disable clicks" : "Enable clicks";
                });
            
            toggleSelectable.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    if (buttonDefault.isSelectable)
                    {
                        buttonDefault.SetSelectable(false);
                        buttonGreen.SetSelectable(false);
                        buttonRed.SetSelectable(false);
                    }
                    else
                    {
                        buttonDefault.SetSelectable(true);
                        buttonGreen.SetSelectable(true);
                        buttonRed.SetSelectable(true);
                    }
                    toggleSelectable.GetComponentInChildren<Text>().text =
                        buttonDefault.isSelectable ? "Disable selection" : "Enable selection";
                });
            
            toggleSelected.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    if (buttonDefault.isSelected)
                    {
                        buttonDefault.SetSelected(false);
                        buttonGreen.SetSelected(false);
                        buttonRed.SetSelected(false);
                    }
                    else
                    {
                        buttonDefault.SetSelected(true);
                        buttonGreen.SetSelected(true);
                        buttonRed.SetSelected(true);
                    }
                    toggleSelected.GetComponentInChildren<Text>().text = buttonDefault.isSelected ? "Deselect" : "Select";
                });
            
            
            enableLinkButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    buttonLinkDefault.EnableButton();
                    buttonLinkBlue.EnableButton();
                });
            
            disableLinkButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    buttonLinkDefault.DisableButton();
                    buttonLinkBlue.DisableButton();
                });
            
            enableIconButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    defaultIconButton.EnableButton();
                    largeIconButton.EnableButton();
                });
            
            disableIconButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    defaultIconButton.DisableButton();
                    largeIconButton.DisableButton();
                });
            
            enableRoundButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    roundButton.EnableButton();
                    roundButtonClose.EnableButton();
                    roundButtonHero.EnableButton();
                });
            
            disableRoundButtons.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    roundButton.DisableButton();
                    roundButtonClose.DisableButton();
                    roundButtonHero.DisableButton();
                });
            
            var dropDownOptions = new List<string>
            {
                "Option 1", "Option 2", "Option 3", "Option 4", "Option 5", "Option 6", "Option 7", "Option 8",
                "Option 9", "Option 10", "Option 11", "Option 12", "Option 13", "Option 14", "Option 15", "Option 16"
            };
            var dropDownStringAdapter = new DefaultDropDownAdapter();
            dropDown.SetAdapter(dropDownStringAdapter);
            dropDownStringAdapter.SetData(dropDownOptions);
        }
    }
}