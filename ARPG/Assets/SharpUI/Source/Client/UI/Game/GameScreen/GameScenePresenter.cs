using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Source.Client.UI.Game.GameScreen
{
    public class GameScenePresenter : BasePresenter<IGameSceneComponent>, IGameScenePresenter
    {
        private readonly IGameSceneModel _model;

        public GameScenePresenter()
        {
            _model = new GameSceneModel();
        }

        public GameScenePresenter(IGameSceneModel model)
        {
            _model = model;
        }

        public void OnSettingsClicked()
        {
            _model.GetSettingsScene().SubscribeWith(disposables, sceneName => ShowSceneAdditive(sceneName));
        }

        public void OnVendorClicked()
        {
            _model.GetVendorsScene().SubscribeWith(disposables, sceneName => ShowSceneAdditive(sceneName));
        }

        public void OnSkillsClicked()
        {
            _model.GetSkillTreeScene().SubscribeWith(disposables, sceneName => ShowSceneAdditive(sceneName));
        }
    }
}