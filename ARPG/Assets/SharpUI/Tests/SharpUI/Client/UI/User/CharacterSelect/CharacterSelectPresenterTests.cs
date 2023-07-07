using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterSelect;
using SharpUI.Source.Data.Model.Character;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterSelect
{
    public class CharacterSelectPresenterTests
    {
        private CharacterSelectPresenter _presenter;
        private ICharacterSelectModel _model;
        private ICharacterSelectComponent _component;
        
        private const string LoginSceneName = "LoginSceneName";
        private const string CharacterCreateScene = "CharacterCreateScene";
        private const string GameLoadingScene = "GameLoadingScene";
        
        private static readonly Character Character = new Character("Character", 123, ClassType.Hunter);
        private readonly List<Character> _characterList = new List<Character> {Character, Character, Character};

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<ICharacterSelectModel>();
            _component = Substitute.For<ICharacterSelectComponent>();
            _presenter = new CharacterSelectPresenter(_model);
            
            _presenter.TakeComponent(_component);
            _model.GetCharacterCreateScene().Returns(Observable.Return(CharacterCreateScene));
            _model.GetLoginScene().Returns(Observable.Return(LoginSceneName));
            _model.GetGameLoadingScene().Returns(Observable.Return(GameLoadingScene));
            _model.GetCharacters().Returns(Observable.Return(_characterList));
        }

        [Test]
        public void CharacterSelectPresenter_CanCreateEmptyConstructor()
        {
            _presenter = new CharacterSelectPresenter();
        }

        [Test]
        public void OnComponentStarted_WillListCharacters()
        {
            _presenter.OnComponentStarted();
            
            _component.Received().ListCharacters(_characterList);
        }

        [Test]
        public void OnBack_WillLoadLoginScene()
        {
            _presenter.OnBack();

            _component.Received().ShowScene(LoginSceneName, Arg.Any<Action>());
        }

        [Test]
        public void OnEnterWorld_WillShowGameLoadingScene()
        {
            _presenter.OnEnterWorld();

            _component.Received().ShowScene(GameLoadingScene, Arg.Any<Action>());
        }

        [Test]
        public void OnCreateNewCharacter_WillShowCharacterCreateScene()
        {
            _presenter.OnCreateNewCharacter();

            _component.Received().ShowScene(CharacterCreateScene, Arg.Any<Action>());
        }

        [Test]
        public void OnDeleteCharacter_WillDeleteCorrectCharacter()
        {
            _presenter.OnDeleteCharacter(Character);
            
            _model.Received().DeleteCharacter(Character);
        }

        [Test]
        public void OnDeleteCharacter_WillDeleteSelectedCharacter()
        {
            _presenter.OnDeleteCharacter(Character);
            
            _component.Received().DeleteSelected();
        }

        [Test]
        public void OnDeleteCharacter_WillDisableDeleteButton()
        {
            _presenter.OnDeleteCharacter(Character);
            
            _component.Received().SetDeleteButtonEnabled(false);
        }

        [Test]
        public void OnCharacterSelected_WillShowCharacterDetails()
        {
            _presenter.OnCharacterSelected(Character);
            
            _component.Received().ShowCharacterDetails(Character);
        }

        [Test]
        public void OnCharacterSelectionChanged_ToTrue_WillEnableDeleteButton()
        {
            _presenter.OnCharacterSelectionChanged(true);
            
            _component.Received().SetDeleteButtonEnabled(true);
        }
        
        [Test]
        public void OnCharacterSelectionChanged_ToFalse_WillDisableDeleteButton()
        {
            _presenter.OnCharacterSelectionChanged(false);
            
            _component.Received().SetDeleteButtonEnabled(false);
        }
        
        [Test]
        public void OnCharacterSelectionChanged_ToTrue_WillEnableEnterWorldButton()
        {
            _presenter.OnCharacterSelectionChanged(true);
            
            _component.Received().SetEnterWorldEnabled(true);
        }
        
        [Test]
        public void OnCharacterSelectionChanged_ToFalse_WillDisableEnterWorldButton()
        {
            _presenter.OnCharacterSelectionChanged(false);
            
            _component.Received().SetEnterWorldEnabled(false);
        }

        [Test]
        public void OnPointerEnteredDeleteCharacterButton_WillShowTooltip()
        {
            _presenter.OnCharacterSelectionChanged(true);
            
            _presenter.OnPointerEnteredDeleteCharacterButton();
            
            _component.Received().ShowDeleteCharacterTooltip();
        }

        [Test]
        public void OnPointerExitedDeleteCharacterButton_WillHideTooltip()
        {
            _presenter.OnPointerExitedDeleteCharacterButton();
            
            _component.Received().HideDeleteCharacterTooltip();
        }
    }
}