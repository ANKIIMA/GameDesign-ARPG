using System.Collections.Generic;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Dialogs;
using SharpUI.Source.Common.UI.Elements.DropDowns;
using SharpUI.Source.Common.UI.Elements.DropDowns.Adapters;
using SharpUI.Source.Common.UI.Elements.Toggle;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Client.UI.User.Login
{
    public class LoginComponent : MonoBehaviourComponent<LoginComponent, LoginPresenter>, ILoginComponent
    {
        [SerializeField] public GameObject dialogPrefab;
        
        [SerializeField] public TMP_InputField inputEmail;
        [SerializeField] public TMP_InputField inputPassword;
        [SerializeField] public ToggleButton toggleRememberLogin;
        [SerializeField] public ToggleButton toggleRememberEmail;
        [SerializeField] public RectButton buttonLogin;
        [SerializeField] public Button buttonCreateAccount;
        [SerializeField] public Button buttonLoginHelp;
        [SerializeField] public Button buttonSupport;
        [SerializeField] public Button buttonReadMore;
        [SerializeField] public Button buttonShop;
        [SerializeField] public RectButton buttonOptions;
        [SerializeField] public RectButton buttonExit;
        [SerializeField] public DropDown regionDropdown;

        protected override LoginComponent GetComponent() => this;
        private ILoginPresenter _presenter;

        public void SetupComponent()
        {
            _presenter = GetPresenter();
            
            inputEmail.onValueChanged.AsObservable()
                .SubscribeWith(this, email => _presenter.OnEmailChanged(email));

            inputPassword.onValueChanged.AsObservable()
                .SubscribeWith(this, password => _presenter.OnPasswordChanged(password));
            
            toggleRememberLogin.ObserveToggleStateChange()
                .SubscribeWith(this, rememberLogin => _presenter.OnRememberLoginChanged(rememberLogin));
            
            toggleRememberEmail.ObserveToggleStateChange()
                .SubscribeWith(this, rememberEmail => _presenter.OnRememberEmailChanged(rememberEmail));
            
            buttonLogin.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ => _presenter.OnLoginClicked(inputEmail.text, inputPassword.text));
            
            buttonCreateAccount.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnRegisterClicked());
            
            buttonLoginHelp.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnCantLoginClicked());
            
            buttonSupport.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnSupportClicked());
            
            buttonReadMore.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnReadMoreClicked());
            
            buttonShop.OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnShopClicked());
            
            buttonOptions.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ => _presenter.OnOptionsClicked());
            
            buttonExit.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ => _presenter.OnExitClicked());
            
            _presenter.OnRegionsRequested();
        }

        public void ShowReadMoreDialog(string title, string description)
        {
            var dialogInstance = Instantiate(dialogPrefab, GetComponent<RectTransform>());
            var dialog = dialogInstance.GetComponent<Dialog>();
            dialog.SetIconType(DialogIconType.Info);
            dialog.SetNeutralButtonVisible(false);
            dialog.SetPositiveButtonText("OK");
            dialog.SetNegativeButtonText("Cancel");
            dialog.SetTitle(title);
            dialog.SetDescription(description);
            dialog.buttonPositive.GetComponent<RectButton>()
                .GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => dialog.Close());
            dialog.buttonNegative.GetComponent<RectButton>()
                .GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => dialog.Close());
        }

        public void ShowShopDialog(string title, string description)
        {
            var dialogInstance = Instantiate(dialogPrefab, GetComponent<RectTransform>());
            var dialog = dialogInstance.GetComponent<Dialog>();
            dialog.SetIconType(DialogIconType.Info);
            dialog.SetNeutralButtonVisible(false);
            dialog.SetPositiveButtonText("OK");
            dialog.SetNegativeButtonText("Cancel");
            dialog.SetTitle(title);
            dialog.SetDescription(description);
            dialog.buttonPositive.GetComponent<RectButton>()
                .GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => dialog.Close());
            dialog.buttonNegative.GetComponent<RectButton>()
                .GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => dialog.Close());
        }

        public void ShowRegions(IEnumerable<string> regions)
        {
            var adapter = new DefaultDropDownAdapter();
            regionDropdown.SetAdapter(adapter);
            adapter.SetData(regions);
            
            regionDropdown.SelectAtIndex(1);
        }

        public void ExitApplication()
        {
            #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}