using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.LoginHelp;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.User.LoginHelp
{
    public class LoginHelpPresenterTests
    {
        private const string LoginScene = "LoginScene";
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private LoginHelpPresenter _presenter;
        private ILoginHelpModel _model;
        private ILoginHelpComponent _component;
        
        private const string Email = "email@email.com";

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<ILoginHelpModel>();
            _model.GetLoginSceneName().Returns(Observable.Return(LoginScene));
            
            _component = Substitute.For<ILoginHelpComponent>();
            _presenter = new LoginHelpPresenter(_model);
            _presenter.TakeComponent(_component);
        }

        [Test]
        public void LoginHelpPresenter_CanCreatEmptyConstructor()
        {
            _presenter = new LoginHelpPresenter();
        }
        
        [Test]
        public void OnEmailChanged_WillObserveEmailChange()
        {
            var observedEmail = "";
            _presenter.ObserveEmailChange().SubscribeWith(_disposable, value => observedEmail = value);
            
            _presenter.OnEmailChanged(Email);
            
            Assert.AreEqual(Email, observedEmail);
        }

        [Test]
        public void OnResetPasswordClicked_WillResetPassword()
        {
            _presenter.OnResetPasswordClicked(Email);
            
            _model.Received().ResetPassword(Email);
        }

        [Test]
        public void GoBack_WillGoBackToLoginScene()
        {
            _presenter.GoBack();

            _component.Received().ShowScene(LoginScene, Arg.Any<Action>());
        }
    }
}