using System;
using System.Collections.Generic;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;
using SharpUI.Source.Data.Model.Character;

namespace SharpUI.Source.Client.UI.User.CharacterSelect
{
    public interface ICharacterSelectComponent : IBaseComponent
    {
        void ListCharacters(List<Character> characters);
        void ShowCharacterDetails(Character character);
        void DeleteSelected();
        void SetDeleteButtonEnabled(bool deleteEnabled);
        void SetEnterWorldEnabled(bool enterEnabled);
        void ShowDeleteCharacterTooltip();
        void HideDeleteCharacterTooltip();
    }

    public interface ICharacterSelectPresenter
    {
        void OnBack();
        void OnEnterWorld();
        void OnCreateNewCharacter();
        void OnDeleteCharacter(Character character);
        void OnCharacterSelected(Character character);
        void OnCharacterSelectionChanged(bool characterSelected);
        void OnPointerEnteredDeleteCharacterButton();
        void OnPointerExitedDeleteCharacterButton();
    }

    public interface ICharacterSelectModel : IBaseModel
    {
        IObservable<string> GetLoginScene();
        IObservable<string> GetCharacterCreateScene();
        IObservable<string> GetGameLoadingScene();
        IObservable<List<Character>> GetCharacters();
        void DeleteCharacter(Character character);
    }
}