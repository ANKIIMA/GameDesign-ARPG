using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Source.Client.UI.User.Login
{
    public class LoginPresenter : BasePresenter<ILoginComponent>, ILoginPresenter
    {
        [CanBeNull] private Subject<string> _emailChangedObserver;
        [CanBeNull] private Subject<string> _passwordChangedObserver;
        [CanBeNull] private Subject<bool> _rememberLoginChangedObserver;
        [CanBeNull] private Subject<bool> _rememberEmailChangedObserver;

        private readonly ILoginModel _model;

        public LoginPresenter()
        {
            _model = new LoginModel();
        }
        
        public LoginPresenter(ILoginModel model)
        {
            _model = model;
        }

        public Subject<string> ObserveEmailChange() =>
            _emailChangedObserver ?? (_emailChangedObserver = new Subject<string>());

        public Subject<string> ObservePasswordChange() =>
            _passwordChangedObserver ?? (_passwordChangedObserver = new Subject<string>());

        public Subject<bool> ObserveRememberLoginChange() =>
            _rememberLoginChangedObserver ?? (_rememberLoginChangedObserver = new Subject<bool>());

        public Subject<bool> ObserveRememberEmailChange() =>
            _rememberEmailChangedObserver ?? (_rememberEmailChangedObserver = new Subject<bool>());

        public void OnEmailChanged(string email)
        {
            _emailChangedObserver?.OnNext(email);
        }

        public void OnPasswordChanged(string password)
        {
            _passwordChangedObserver?.OnNext(password);
        }

        public void OnRememberLoginChanged(bool loginChanged)
        {
            _rememberLoginChangedObserver?.OnNext(loginChanged);
        }

        public void OnRememberEmailChanged(bool emailChanged)
        {
            _rememberEmailChangedObserver?.OnNext(emailChanged);
        }

        public void OnLoginClicked(string email, string password)
        {
            _model.LogIn(email, password);

            _model.GetCharacterSelectionScene().SubscribeWith(disposables,
                sceneName => ShowScene(sceneName));
        }

        public void OnRegisterClicked()
        {
            // Do nothing
        }

        public void OnCantLoginClicked()
        {
            // Do nothing
        }

        public void OnSupportClicked()
        {
            // Do nothing
        }

        public void OnReadMoreClicked()
        {
            _model.GetReadMoreDialogData()
                .SubscribeWith(disposables, data => OnComponent(
                    component => component.ShowReadMoreDialog(data.First, data.Second)));
        }

        public void OnShopClicked()
        {
            _model.GetShopDialogData()
                .SubscribeWith(disposables, data => OnComponent(
                    component => component.ShowShopDialog(data.First, data.Second)));
        }

        public void OnOptionsClicked() => ShowSceneAdditive("Settings");

        public void OnExitClicked()
        {
            OnComponent(component => component.ExitApplication());
        }

        public void OnRegionsRequested()
        {
            _model.GetRegionsData()
                .SubscribeWith(disposables, data => OnComponent(
                    component => component.ShowRegions(data)));
        }
    }
}