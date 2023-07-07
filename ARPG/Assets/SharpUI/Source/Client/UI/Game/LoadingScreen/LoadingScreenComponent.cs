using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Loading;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Client.UI.Game.LoadingScreen
{
    public class LoadingScreenComponent : MonoBehaviourComponent<LoadingScreenComponent, LoadingScreenPresenter>,
        ILoadingScreenComponent
    {
        [SerializeField] public RectButton buttonBack;
        [SerializeField] public LoadingBar loadingBar;

        protected override LoadingScreenComponent GetComponent() => this;

        private ILoadingScreenPresenter _presenter;
        
        public void SetupComponent()
        {
            _presenter = GetPresenter();
            
            buttonBack.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ => _presenter.OnBack());
            
            loadingBar.UpdatePercentage(0f);

            _presenter.SimulateLoadingBar();
        }

        public void UpdateLoadingPercentage(float percentage)
        {
            loadingBar.UpdatePercentage(percentage);
        }
    }
}