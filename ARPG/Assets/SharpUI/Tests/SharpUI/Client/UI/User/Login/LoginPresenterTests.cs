using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.Login;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.User.Login
{
    public class LoginPresenterTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private LoginPresenter _presenter;
        private ILoginModel _model;
        private ILoginComponent _component;
        private readonly Pair<string, string> _pairData = new Pair<string, string>("FirstData", "SecondData");
        private List<string> _regionsData = new List<string> { "Region 1", "Region 2"};
        private readonly string CharacterSceneName = "CharacterSceneName";
        
        private const string Email = "email@email.com";
        private const string Password = "myBadPassword1234";

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<ILoginModel>();
            _presenter = new LoginPresenter(_model);
            _component = Substitute.For<ILoginComponent>();
            _presenter.TakeComponent(_component);

            _model.GetCharacterSelectionScene().Returns(Observable.Return(CharacterSceneName));
            _model.GetShopDialogData().Returns(Observable.Return(_pairData));
            _model.GetReadMoreDialogData().Returns(Observable.Return(_pairData));
            _model.GetRegionsData().Returns(Observable.Return(_regionsData));
        }

        [Test]
        public void LoginPresenter_CanCreateEmptyConstructor()
        {
            _presenter = new LoginPresenter();
        }

        [Test]
        public void ObserveEmailChange_WillObserve()
        {
            var observedEmail = "";
            _presenter.ObserveEmailChange().SubscribeWith(_disposable, value => observedEmail = value);
            
            _presenter.OnEmailChanged(Email);
            
            Assert.AreEqual(Email, observedEmail);
        }

        [Test]
        public void ObservePasswordChange_WillObserve()
        {
            var observedPassword = "";
            _presenter.ObservePasswordChange().SubscribeWith(_disposable, value => observedPassword = value);
            
            _presenter.OnPasswordChanged(Password);
            
            Assert.AreEqual(Password, observedPassword);
        }

        [Test]
        public void ObserveRememberLoginChange_WillObserve()
        {
            var observedRememberLogin = false;
            _presenter.ObserveRememberLoginChange().SubscribeWith(_disposable, 
                value => observedRememberLogin = value);
            
            _presenter.OnRememberLoginChanged(true);
            
            Assert.IsTrue(observedRememberLogin);
        }

        [Test]
        public void ObserveRememberEmailChange_WillObserve()
        {
            var observedRememberEmail = false;
            _presenter.ObserveRememberEmailChange().SubscribeWith(_disposable,
                value => observedRememberEmail = value);
            
            _presenter.OnRememberEmailChanged(true);
            
            Assert.IsTrue(observedRememberEmail);
        }

        [Test]
        public void OnLoginClicked_WillInformModel()
        {
            _presenter.OnLoginClicked(Email, Password);
            
            _model.Received().LogIn(Email, Password);
        }

        [Test]
        public void OnLoginClicked_WillShowCharacterSelectionScene()
        {
            _presenter.OnLoginClicked(Email, Password);
            
            _component.Received().ShowScene(CharacterSceneName, Arg.Any<Action>());
        }

        [Test]
        public void OnRegisterClicked_WillDoNothing()
        {
            _presenter.OnRegisterClicked();
            
            Assert.AreEqual(0, _model.ReceivedCalls().Count());
        }
        
        [Test]
        public void OnCantLoginClicked_WillDoNothing()
        {
            _presenter.OnCantLoginClicked();
            
            Assert.AreEqual(0, _model.ReceivedCalls().Count());
        }
        
        [Test]
        public void OnSupportClicked_WillDoNothing()
        {
            _presenter.OnSupportClicked();
            
            Assert.AreEqual(0, _model.ReceivedCalls().Count());
        }

        [Test]
        public void OnReadMoreClicked_WillShowReadMoreDialog()
        {
            _presenter.OnReadMoreClicked();

            _component.Received().ShowReadMoreDialog(_pairData.First, _pairData.Second);
        }

        [Test]
        public void OnShopClicked_WillShowShopDialog()
        {
            _presenter.OnShopClicked();

            _component.Received().ShowShopDialog(_pairData.First, _pairData.Second);
        }

        [Test]
        public void OnOptionsClicked_WillDoNothing()
        {
            _presenter.OnOptionsClicked();
            
            Assert.AreEqual(0, _model.ReceivedCalls().Count());
        }

        [Test]
        public void OnExitClicked_WillExitApplication()
        {
            _presenter.OnExitClicked();
            
            _component.Received().ExitApplication();
        }

        [Test]
        public void OnRegionsRequested_WillShowRegions()
        {
            _presenter.OnRegionsRequested();
            
            _component.Received().ShowRegions(_regionsData);
        }
    }
}