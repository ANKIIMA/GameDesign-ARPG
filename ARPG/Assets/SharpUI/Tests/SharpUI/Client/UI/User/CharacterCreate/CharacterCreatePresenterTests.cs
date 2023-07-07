using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterCreate;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterCreate
{
    public class CharacterCreatePresenterTests
    {
        private const string CharacterSelectScene = "CharacterSelectScene";
        private CharacterCreatePresenter _presenter;
        private ICharacterCreateModel _model;
        private ICharacterCreateComponent _component;

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<ICharacterCreateModel>();
            _model.GetCharacterSelectScene().Returns(Observable.Return(CharacterSelectScene));
            
            _component = Substitute.For<ICharacterCreateComponent>();
            _presenter = new CharacterCreatePresenter(_model);
            _presenter.TakeComponent(_component);
        }
        
        [Test]
        public void CharacterCreatePresenter_CanCreateEmptyConstructor()
        {
            _presenter = new CharacterCreatePresenter();
        }

        [Test]
        public void LoadData_WillShowBioColors()
        {
            _presenter.LoadData();
            
            _component.Received().RenderBioColors(Arg.Any<List<BioColor>>());
        }
        
        [Test]
        public void LoadData_WillShowDefaultStyles()
        {
            _presenter.LoadData();
            
            _component.Received().RenderDefaultStyles(Arg.Any<List<DefaultStyle>>());
        }

        [Test]
        public void OnBackClicked_WillShowCharacterSelectScene()
        {
            _presenter.OnBackClicked();

            _component.ShowScene(CharacterSelectScene, Arg.Any<Action>());
        }
        
        [Test]
        public void OnCreateClicked_WillShowCharacterSelectScene()
        {
            _presenter.OnCreateClicked();

            _component.ShowScene(CharacterSelectScene, Arg.Any<Action>());
        }
    }
}