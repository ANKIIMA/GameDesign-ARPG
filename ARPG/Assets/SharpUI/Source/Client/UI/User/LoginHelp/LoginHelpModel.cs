using System;
using UniRx;

namespace SharpUI.Source.Client.UI.User.LoginHelp
{
    public class LoginHelpModel : ILoginHelpModel
    {
        private const string LoginSceneName = "LoginScene";
        
        public void ResetPassword(string email)
        {
        }

        public IObservable<string> GetLoginSceneName()
        {
            return Observable.Return(LoginSceneName);
        }
    }
}