using NUnit.Framework;
using SharpUI.Source.Client.UI.User.Login;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Tests.SharpUI.Client.UI.User.Login
{
    public class LoginModelTests
    {
        private ILoginModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new LoginModel();
        }

        [Test]
        public void LogIn_WillDoNothing()
        {
            _model.LogIn("email", "password");
        }

        [Test]
        public void GetCharacterSelectionScene_WillReturnData()
        {
            var data = _model.GetCharacterSelectionScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetReadMoreDialogData_WillReturnData()
        {
            var data = _model.GetReadMoreDialogData().BlockingValue();
            
            Assert.IsNotEmpty(data.First);
            Assert.IsNotEmpty(data.Second);
        }
        
        [Test]
        public void GetShopDialogData_WillReturnData()
        {
            var data = _model.GetShopDialogData().BlockingValue();
            
            Assert.IsNotEmpty(data.First);
            Assert.IsNotEmpty(data.Second);
        }

        [Test]
        public void GetRegionsData_WillReturnData()
        {
            var data = _model.GetRegionsData().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }
    }
}