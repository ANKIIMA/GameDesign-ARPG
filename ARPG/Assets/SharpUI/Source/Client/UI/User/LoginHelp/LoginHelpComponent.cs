using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Client.UI.User.LoginHelp
{
    public class LoginHelpComponent : MonoBehaviourComponent<LoginHelpComponent, LoginHelpPresenter>, ILoginHelpComponent
    {
        [SerializeField] public TMP_InputField textEmail;
        [SerializeField] public Button buttonResetPassword;
        [SerializeField] public Button buttonBack;

        protected override LoginHelpComponent GetComponent() => this;

        private ILoginHelpPresenter _presenter;

        public void SetupComponent()
        {
            _presenter = GetPresenter();
            
            textEmail.onValueChanged
                .AsObservable()
                .SubscribeWith(this, email => _presenter.OnEmailChanged(email));
            
            buttonResetPassword
                .OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnResetPasswordClicked(textEmail.text));
            
            buttonBack
                .OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.GoBack());
        }
    }
}