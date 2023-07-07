using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Elements.ModalViews;
using SharpUI.Source.Common.Util.Extensions;
using UniRx.Triggers;
using UnityEngine;

namespace SharpUI.Source.Client.UI.Game.VendorScreen
{
    public class VendorScreenComponent : MonoBehaviourComponent<VendorScreenComponent, VendorScreenPresenter>,
        IVendorScreenComponent
    {
        [SerializeField] public ModalView vendorModalView;

        private IVendorScreenPresenter _presenter;
        
        protected override VendorScreenComponent GetComponent() => this;

        public void SetupComponent()
        {
            _presenter = GetPresenter();

            vendorModalView.OnDestroyAsObservable().SubscribeWith(this,
                _ => _presenter.OnVendorWindowDestroyed());
        }
    }
}