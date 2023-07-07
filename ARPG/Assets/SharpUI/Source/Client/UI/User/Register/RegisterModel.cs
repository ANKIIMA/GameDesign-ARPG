using System;
using UniRx;

namespace SharpUI.Source.Client.UI.User.Register
{
    public class RegisterModel : IRegisterModel
    {
        private const string LoginSceneName = "LoginScene";
        
        public void RegisterClient(string email, string name, string lastName, string password, string passwordConfirm)
        {
        }

        public IObservable<string> GetLoginSceneName()
        {
            return Observable.Return(LoginSceneName);
        }
    }
}