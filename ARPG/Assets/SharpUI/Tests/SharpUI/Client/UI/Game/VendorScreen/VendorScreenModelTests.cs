using NUnit.Framework;
using SharpUI.Source.Client.UI.Game.VendorScreen;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Tests.SharpUI.Client.UI.Game.VendorScreen
{
    public class VendorScreenModelTests
    {
        private VendorScreenModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new VendorScreenModel();
        }

        [Test]
        public void GetMySceneName_ReturnsNonemptyData()
        {
            var data = _model.GetMySceneName().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }
    }
}