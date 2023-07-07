using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Source.Client.UI.Game.VendorScreen
{
    public class VendorScreenPresenter : BasePresenter<IVendorScreenComponent>, IVendorScreenPresenter
    {
        private readonly IVendorScreenModel _model;

        public VendorScreenPresenter()
        {
            _model = new VendorScreenModel();
        }

        public VendorScreenPresenter(IVendorScreenModel model)
        {
            _model = model;
        }
        
        public void OnVendorWindowDestroyed()
        {
            _model.GetMySceneName().SubscribeWith(disposables, sceneName => HideScene(sceneName));
        }
    }
}