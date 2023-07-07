using NUnit.Framework;
using SharpUI.Source.Client.UI.Game.LoadingScreen;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Tests.SharpUI.Client.UI.Game.LoadingScreen
{
    public class LoadingScreenModelTests
    {
        private ILoadingScreenModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new LoadingScreenModel();
        }

        [Test]
        public void GetCharacterSelectScene_WillReturnData()
        {
            var data = _model.GetCharacterSelectScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetGamePlaygroundScene_WillReturnData()
        {
            var data = _model.GetGamePlaygroundScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }
    }
}