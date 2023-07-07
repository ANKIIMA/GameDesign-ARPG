using System.Collections.Generic;
using System.Linq;
using SharpUI.Source.Client.UI.User.CharacterSelect.CharacterList;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Elements.List;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.UI.Elements.TooltipInfo;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Data.Model.Character;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Client.UI.User.CharacterSelect
{
    public class CharacterSelectComponent : MonoBehaviourComponent<CharacterSelectComponent, CharacterSelectPresenter>,
        ICharacterSelectComponent
    {
        [SerializeField] public GameObject tooltipPrefab;
        [SerializeField] public Button buttonBack;
        [SerializeField] public Button buttonEnterWorld;
        [SerializeField] public Button buttonCreateNewCharacter;
        [SerializeField] public Button buttonDeleteCharacter;
        [SerializeField] public ListView characterListView;
        [SerializeField] public TMP_Text deleteCharacterInfoText;
        
        private CharacterListAdapter _adapter;
        private ISelectionChangeListener<Character> _selectionChangeListener;
        private ITooltip _tooltipDeleteCharacter;

        protected override CharacterSelectComponent GetComponent() => this;

        private ICharacterSelectPresenter _presenter;

        public void SetupComponent()
        {
            _presenter = GetPresenter();
            
            _tooltipDeleteCharacter = Instantiate(tooltipPrefab).GetComponent<ITooltip>();
            _tooltipDeleteCharacter.SetShowDelayTimeMillis(450);
            _tooltipDeleteCharacter.OffsetPointerByPercentage(80f);
            _tooltipDeleteCharacter.BindContent(deleteCharacterInfoText.GetComponent<RectTransform>());

            _adapter = characterListView.GetComponent<CharacterListAdapter>();
            _selectionChangeListener = _adapter.GetSelectionChangeListener();
            
            _selectionChangeListener.ObserveItemSelected()
                .SubscribeWith(this, _presenter.OnCharacterSelected);
            
            _selectionChangeListener.ObserveSelectionChange()
                .SubscribeWith(this, _ =>
                    _presenter.OnCharacterSelectionChanged(_adapter.HasSelectedItems()));

            buttonBack.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnBack());
            
            buttonEnterWorld.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnEnterWorld());
            
            buttonCreateNewCharacter.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnCreateNewCharacter());
            
            buttonDeleteCharacter.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    var character = _adapter.GetSelectedData().First();
                    _presenter.OnDeleteCharacter(character);
                });
            
            buttonDeleteCharacter.OnPointerEnterAsObservable().SubscribeWith(this,
                _ => _presenter.OnPointerEnteredDeleteCharacterButton());
            buttonDeleteCharacter.OnPointerExitAsObservable().SubscribeWith(this,
                _ => _presenter.OnPointerExitedDeleteCharacterButton());
        }

        public void DeleteSelected()
        {
            _adapter.RemoveSelected();
        }

        public void SetDeleteButtonEnabled(bool deleteEnabled)
        {
            buttonDeleteCharacter.interactable = deleteEnabled;
        }

        public void SetEnterWorldEnabled(bool enterEnabled)
        {
            buttonEnterWorld.interactable = enterEnabled;
        }

        public void ShowCharacterDetails(Character character)
        {
            // Show character details
        }

        public void ListCharacters(List<Character> characters)
        {
            _adapter.SetCharactersAndNotify(characters);
        }

        public void ShowDeleteCharacterTooltip()
        {
            _tooltipDeleteCharacter.ShowBelow(buttonDeleteCharacter.GetComponent<RectTransform>());
        }

        public void HideDeleteCharacterTooltip()
        {
            _tooltipDeleteCharacter.Hide();
        }
    }
}