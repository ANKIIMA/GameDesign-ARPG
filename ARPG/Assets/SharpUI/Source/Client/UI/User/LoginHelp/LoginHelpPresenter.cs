using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Source.Client.UI.User.LoginHelp
{
    public class LoginHelpPresenter : BasePresenter<ILoginHelpComponent>, ILoginHelpPresenter
    {
        [CanBeNull] private Subject<string> _emailChangedObserver;
        
        private readonly ILoginHelpModel _model;

        public LoginHelpPresenter()
        {
            _model = new LoginHelpModel();
        }

        public LoginHelpPresenter(ILoginHelpModel model)
        {
            _model = model;
        }
        
        public Subject<string> ObserveEmailChange() =>
            _emailChangedObserver ?? (_emailChangedObserver = new Subject<string>());
        
        public void OnResetPasswordClicked(string email)
        {
            _model.ResetPassword(email);
        }

        public void OnEmailChanged(string email)
        {
            _emailChangedObserver?.OnNext(email);
        }

        public void GoBack()
        {
            _model
                .GetLoginSceneName()
                .SubscribeWith(disposables, sceneName => ShowScene(sceneName));
        }
    }
}