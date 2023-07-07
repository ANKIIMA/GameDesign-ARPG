using System;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;

namespace SharpUI.Source.Client.UI.User.Register
{
    public interface IRegisterComponent : IBaseComponent
    {
    }
    
    public interface IRegisterPresenter
    {
        void OnEmailChanged(string email);
        void OnPasswordChanged(string password);
        void OnPasswordConfirmChanged(string passwordConfirm);
        void OnRegisterClicked(string email, string name, string lastName, string password, string passwordConfirm);
        void OnNameChanged(string name);
        void OnGoBack();
        void OnLastNameChanged(string lastName);
    }

    public interface IRegisterModel : IBaseModel
    {
        void RegisterClient(string email, string name, string lastName, string password, string passwordConfirm);
        IObservable<string> GetLoginSceneName();
    }
}