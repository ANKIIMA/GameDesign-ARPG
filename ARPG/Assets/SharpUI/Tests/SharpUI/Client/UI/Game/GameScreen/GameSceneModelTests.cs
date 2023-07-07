using NUnit.Framework;
using SharpUI.Source.Client.UI.Game.GameScreen;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Tests.SharpUI.Client.UI.Game.GameScreen
{
    public class GameSceneModelTests
    {
        private GameSceneModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new GameSceneModel();
        }

        [Test]
        public void GetSettingsScene_ReturnsNonemptyData()
        {
            var data = _model.GetSettingsScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetSkillTreeScene_ReturnsNonemptyData()
        {
            var data = _model.GetSkillTreeScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }
        
        [Test]
        public void GetVendorsScene_ReturnsNonemptyData()
        {
            var data = _model.GetVendorsScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }
    }
}