using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Source.Client.UI.User.Register
{
    public class RegisterPresenter : BasePresenter<IRegisterComponent>, IRegisterPresenter
    {
        [CanBeNull] private Subject<string> _emailChangedObserver;
        [CanBeNull] private Subject<string> _nameChangedObserver;
        [CanBeNull] private Subject<string> _lastNameChangedObserver;
        [CanBeNull] private Subject<string> _passwordChangedObserver;
        [CanBeNull] private Subject<string> _passwordConfirmChangedObserver;
        
        private readonly IRegisterModel _model;

        public RegisterPresenter() => _model = new RegisterModel();

        public RegisterPresenter(IRegisterModel model) => _model = model;

        public Subject<string> ObserveEmailChange() =>
            _emailChangedObserver ?? (_emailChangedObserver = new Subject<string>());
        
        public Subject<string> ObserveNameChange() =>
            _nameChangedObserver ?? (_nameChangedObserver = new Subject<string>());
        
        public Subject<string> ObserveLastNameChange() =>
            _lastNameChangedObserver ?? (_lastNameChangedObserver = new Subject<string>());
        
        public Subject<string> ObservePasswordChange() =>
            _passwordChangedObserver ?? (_passwordChangedObserver = new Subject<string>());
        
        public Subject<string> ObservePasswordConfirmChange() =>
            _passwordConfirmChangedObserver ?? (_passwordConfirmChangedObserver = new Subject<string>());
        
        public void OnEmailChanged(string email) => _emailChangedObserver?.OnNext(email);

        public void OnNameChanged(string name) => _nameChangedObserver?.OnNext(name);

        public void OnLastNameChanged(string lastName) => _lastNameChangedObserver?.OnNext(lastName);

        public void OnPasswordChanged(string password) => _passwordChangedObserver?.OnNext(password);

        public void OnPasswordConfirmChanged(string passwordConfirm)
            => _passwordConfirmChangedObserver?.OnNext(passwordConfirm);

        public void OnRegisterClicked(string email, string name, string lastName, string pswd, string pswdConfirm)
            => _model.RegisterClient(email, name, lastName, pswd, pswdConfirm);

        public void OnGoBack()
        {
            _model
                .GetLoginSceneName()
                .ObserveOnMainThread()
                .SubscribeWith(disposables, sceneName => ShowScene(sceneName));
        }
    }
}