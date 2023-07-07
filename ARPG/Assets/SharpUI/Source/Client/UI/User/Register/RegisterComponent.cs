using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Client.UI.User.Register
{
    public class RegisterComponent : MonoBehaviourComponent<RegisterComponent, RegisterPresenter>, IRegisterComponent
    {
        [SerializeField] public TMP_InputField textEmail;
        [SerializeField] public TMP_InputField textName;
        [SerializeField] public TMP_InputField textLastName;
        [SerializeField] public TMP_InputField textPassword;
        [SerializeField] public TMP_InputField textPasswordConfirm;
        [SerializeField] public Button buttonLogin;
        [SerializeField] public Button buttonBack;

        protected override RegisterComponent GetComponent() => this;

        private IRegisterPresenter _presenter;
        
        public void SetupComponent()
        {
            _presenter = GetPresenter();
            
            buttonLogin
                .OnClickAsObservable()
                .SubscribeWith(this, _ =>
                    _presenter.OnRegisterClicked(
                        textEmail.text,
                        textName.text,
                        textLastName.text,
                        textPassword.text,
                        textPasswordConfirm.text));
            
            buttonBack
                .OnClickAsObservable()
                .SubscribeWith(this, _ => _presenter.OnGoBack());

            textEmail.onValueChanged
                .AsObservable()
                .SubscribeWith(
                    this,
                    email => _presenter.OnEmailChanged(email));
            
            textName.onValueChanged
                .AsObservable()
                .SubscribeWith(
                    this,
                    clientName => _presenter.OnNameChanged(clientName));
            
            textLastName.onValueChanged
                .AsObservable()
                .SubscribeWith(this,
                    lastName => _presenter.OnLastNameChanged(lastName));

            textPassword.onValueChanged.AsObservable()
                .SubscribeWith(
                    this,
                    clientPassword => _presenter.OnPasswordChanged(clientPassword));

            textPasswordConfirm.onValueChanged.AsObservable()
                .SubscribeWith(
                    this,
                    clientPasswordConfirm => _presenter.OnPasswordConfirmChanged(clientPasswordConfirm));
        }
    }
}