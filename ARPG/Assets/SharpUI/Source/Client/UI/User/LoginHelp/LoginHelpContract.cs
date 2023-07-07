using System;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;

namespace SharpUI.Source.Client.UI.User.LoginHelp
{
    public interface ILoginHelpComponent : IBaseComponent
    {
    }

    public interface ILoginHelpPresenter
    {
        void OnResetPasswordClicked(string email);
        void OnEmailChanged(string email);
        void GoBack();
    }

    public interface ILoginHelpModel : IBaseModel
    {
        void ResetPassword(string email);
        IObservable<string> GetLoginSceneName();
    }
}