using System;
using System.Collections.Generic;
using Castle.Core;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;
using UniRx;

namespace SharpUI.Source.Client.UI.User.Login
{
    public interface ILoginComponent : IBaseComponent
    {
        void ShowReadMoreDialog(string title, string description);
        void ShowShopDialog(string title, string description);
        void ShowRegions(IEnumerable<string> regions);
        void ExitApplication();
    }
    
    public interface ILoginPresenter
    {
        Subject<string> ObserveEmailChange();
        Subject<string> ObservePasswordChange();
        Subject<bool> ObserveRememberLoginChange();
        Subject<bool> ObserveRememberEmailChange();
        void OnEmailChanged(string email);
        void OnPasswordChanged(string password);
        void OnRememberLoginChanged(bool loginChanged);
        void OnRememberEmailChanged(bool emailChanged);
        void OnLoginClicked(string email, string password);
        void OnRegisterClicked();
        void OnCantLoginClicked();
        void OnSupportClicked();
        void OnReadMoreClicked();
        void OnShopClicked();
        void OnOptionsClicked();
        void OnExitClicked();
        void OnRegionsRequested();
    }

    public interface ILoginModel : IBaseModel
    {
        void LogIn(string email, string password);
        IObservable<string> GetCharacterSelectionScene();
        IObservable<Pair<string, string>> GetReadMoreDialogData();
        IObservable<Pair<string, string>> GetShopDialogData();
        IObservable<List<string>> GetRegionsData();
    }
}