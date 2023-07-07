using NUnit.Framework;
using SharpUI.Source.Client.UI.User.LoginHelp;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Tests.SharpUI.Client.UI.User.LoginHelp
{
    public class LoginHelpModelTests
    {
        private const string Email = "email@email.com";
        private ILoginHelpModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new LoginHelpModel();
        }

        [Test]
        public void ResetPassword_WillDoNothing()
        {
            _model.ResetPassword(Email);
        }

        [Test]
        public void GetLoginSceneName_WillReturnData()
        {
            var data = _model.GetLoginSceneName().BlockingValue();
            
            Assert.IsNotNull(data);
        }
    }
}