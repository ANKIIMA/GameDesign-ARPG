using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.Game.GameScreen;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.Game.GameScreen
{
    public class GameScenePresenterTests
    {
        private const string SceneSettings = "Settings";
        private const string SceneVendor = "Vendor";
        private const string SceneSkills = "Skills";
        private IGameSceneModel _model;
        private IGameSceneComponent _component;
        private GameScenePresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<IGameSceneModel>();
            _model.GetSettingsScene().Returns(Observable.Return(SceneSettings));
            _model.GetVendorsScene().Returns(Observable.Return(SceneVendor));
            _model.GetSkillTreeScene().Returns(Observable.Return(SceneSkills));
            _component = Substitute.For<IGameSceneComponent>();
            
            _presenter = new GameScenePresenter(_model);
            _presenter.TakeComponent(_component);
        }

        [Test]
        public void GameScenePresenter_CanCreateEmptyConstructor()
        {
            _presenter = new GameScenePresenter();
        }

        [Test]
        public void OnSettingsClicked_WillLoadSettingsSceneAdditive()
        {
            _presenter.OnSettingsClicked();
            
            _component.Received().ShowSceneAdditive(SceneSettings, Arg.Any<Action>());
        }
        
        [Test]
        public void OnVendorClicked_WillLoadVendorSceneAdditive()
        {
            _presenter.OnVendorClicked();
            
            _component.Received().ShowSceneAdditive(SceneVendor, Arg.Any<Action>());
        }
        
        [Test]
        public void OnSkillsClicked_WillLoadSkillsSceneAdditive()
        {
            _presenter.OnSkillsClicked();
            
            _component.Received().ShowSceneAdditive(SceneSkills, Arg.Any<Action>());
        }
    }
}