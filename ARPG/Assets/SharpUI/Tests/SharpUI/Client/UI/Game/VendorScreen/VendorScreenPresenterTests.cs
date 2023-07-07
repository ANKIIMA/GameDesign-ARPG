using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Client.UI.Game.VendorScreen;
using UniRx;

namespace SharpUI.Tests.SharpUI.Client.UI.Game.VendorScreen
{
    public class VendorScreenPresenterTests
    {
        private const string VendorSceneName = "VendorScene";
        private IVendorScreenModel _model;
        private IVendorScreenComponent _component;
        private VendorScreenPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _model = Substitute.For<IVendorScreenModel>();
            _model.GetMySceneName().Returns(Observable.Return(VendorSceneName));
            _component = Substitute.For<IVendorScreenComponent>();
            _presenter = new VendorScreenPresenter(_model);
            _presenter.TakeComponent(_component);
        }

        [Test]
        public void VendorScreenPresenter_CanCreateEmptyConstructor()
        {
            _presenter = new VendorScreenPresenter();
        }

        [Test]
        public void OnVendorWindowDestroyed_WillHideVendorScene()
        {
            _presenter.OnVendorWindowDestroyed();
            
            _component.Received().HideScene(VendorSceneName, Arg.Any<Action>());
        }
    }
}