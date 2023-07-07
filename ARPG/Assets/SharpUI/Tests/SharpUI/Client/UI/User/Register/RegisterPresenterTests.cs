using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.Register;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.User.Register
{
    public class RegisterPresenterTests
    {
        private const string LoginScene = "LoginScene";
        private const string Email = "email@email.com";
        private const string Name = "Name";
        private const string LastName = "LastName";
        private const string Password = "Password#1234";
        private const string PasswordConfirm = "PasswordConfirm#1234";
        
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private RegisterPresenter _presenter;
        private IRegisterModel _model;
        private IRegisterComponent _component;

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<IRegisterModel>();
            _model.GetLoginSceneName().Returns(Observable.Return(LoginScene));
            
            _component = Substitute.For<IRegisterComponent>();
            _presenter = new RegisterPresenter(_model);
        }
        
        [Test]
        public void RegisterPresenter_CanCreateEmptyConstructor()
        {
            _presenter = new RegisterPresenter();
        }
        
        [Test]
        public void OnEmailChanged_WillOobserveEmailChange()
        {
            var observedEmail = "";
            _presenter.ObserveEmailChange().SubscribeWith(_disposable, value => observedEmail = value);
            
            _presenter.OnEmailChanged(Email);
            
            Assert.AreEqual(Email, observedEmail);
        }
        
        [Test]
        public void OnNameChanged_WillObserveNameChange()
        {
            var observedName = "";
            _presenter.ObserveNameChange().SubscribeWith(_disposable, value => observedName = value);
            
            _presenter.OnNameChanged(Name);
            
            Assert.AreEqual(Name, observedName);
        }
        
        [Test]
        public void OnLastNameChanged_WillObserveLastNameChange()
        {
            var observedLastName = "";
            _presenter.ObserveLastNameChange().SubscribeWith(_disposable, value => observedLastName = value);
            
            _presenter.OnLastNameChanged(LastName);
            
            Assert.AreEqual(LastName, observedLastName);
        }

        [Test]
        public void OnPasswordChanged_WillObservePasswordChange()
        {
            var observedPassword = "";
            _presenter.ObservePasswordChange().SubscribeWith(_disposable, value => observedPassword = value);
            
            _presenter.OnPasswordChanged(Password);
            
            Assert.AreEqual(Password, observedPassword);
        }
        
        [Test]
        public void OnPasswordConfirmChanged_WillObservePasswordConfirmChange()
        {
            var observedPassword = "";
            _presenter.ObservePasswordConfirmChange().SubscribeWith(_disposable, value => observedPassword = value);
            
            _presenter.OnPasswordConfirmChanged(PasswordConfirm);
            
            Assert.AreEqual(PasswordConfirm, observedPassword);
        }

        [Test]
        public void OnRegisterClicked_WillRegisterClient()
        {
            _presenter.OnRegisterClicked(Email, Name, LastName, Password, PasswordConfirm);
            
            _model.Received().RegisterClient(Email, Name, LastName, Password, PasswordConfirm);
        }

        [Test]
        public void OnGoBack_WillShowLoginScene()
        {
            _presenter.OnGoBack();
            
            _component.ShowScene(LoginScene, Arg.Any<Action>());
        }
    }
}