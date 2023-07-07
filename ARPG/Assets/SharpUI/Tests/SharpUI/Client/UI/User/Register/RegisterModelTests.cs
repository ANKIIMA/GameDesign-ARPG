using NUnit.Framework;
using SharpUI.Source.Client.UI.User.Register;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Tests.SharpUI.Client.UI.User.Register
{
    public class RegisterModelTests
    {
        private const string Email = "email@email.com";
        private const string Name = "Name";
        private const string LastName = "LastName";
        private const string Password = "Password#1234";
        private const string PasswordConfirm = "PasswordConfirm#1234";
        
        private IRegisterModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new RegisterModel();
        }

        [Test]
        public void RegisterClient_WillDoNothing()
        {
            _model.RegisterClient(Email, Name, LastName, Password, PasswordConfirm);
        }

        [Test]
        public void GetLoginSceneName_WillReturnData()
        {
            var data = _model.GetLoginSceneName().BlockingValue();
            
            Assert.IsNotNull(data);
        }
    }
}