using System;
using System.Collections.Generic;
using Castle.Core;
using UniRx;

namespace SharpUI.Source.Client.UI.User.Login
{
    public class LoginModel : ILoginModel
    {
        private const string CharacterSelectSceneName = "CharacterSelection";

        private static Pair<string, string> ReadMoreTitleDescription =>
            new Pair<string, string>(
                "Maintenance",
                "This will take you to our web page. Click OK to go, or cancel to close this dialog.");

        private static Pair<string, string> ShopTitleDescription =>
            new Pair<string, string>(
                "Shop",
                "Go to our shop by clicking OK button or click cancel to dismiss this dialog.");
        
        private static List<string> Regions => new List<string>
        {
            "Americas", "Europe", "Asia", "Australia", "Africa"
        };

        public void LogIn(string email, string password)
        {
            // Implement login logic here.
        }

        public IObservable<string> GetCharacterSelectionScene()
            => Observable.Return(CharacterSelectSceneName);

        public IObservable<Pair<string, string>> GetReadMoreDialogData()
            => Observable.Return(ReadMoreTitleDescription);

        public IObservable<Pair<string, string>> GetShopDialogData()
            => Observable.Return(ShopTitleDescription);

        public IObservable<List<string>> GetRegionsData()
            => Observable.Return(Regions);
    }
}