using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Common.Util.Reactive;
using UniRx;

namespace SharpUI.Source.Client.UI.Game.LoadingScreen
{
    public class LoadingScreenPresenter : BasePresenter<ILoadingScreenComponent>, ILoadingScreenPresenter
    {
        private const long BarSimulationDelay = 6;
        private const int BarSimulationSteps = 500;
        private readonly ILoadingScreenModel _model;
        private readonly IDelayObserver _delayObserver;

        public LoadingScreenPresenter()
        {
            _model = new LoadingScreenModel();
            _delayObserver = new DelayObserver();
        }
        
        public LoadingScreenPresenter(ILoadingScreenModel model, IDelayObserver delayObserver)
        {
            _model = model;
            _delayObserver = delayObserver;
        }

        public void SimulateLoadingBar()
        {
            _delayObserver.DelayMilliseconds(BarSimulationDelay, Scheduler.MainThread, BarSimulationSteps)
                .Finally(OnProgressCompleted)
                .SubscribeWith(disposables, value => UpdatePercentage(value/5f));
        }

        private void UpdatePercentage(float percentage)
            => OnComponent(component => component.UpdateLoadingPercentage(percentage+1));

        private void OnProgressCompleted()
            => _model.GetGamePlaygroundScene().SubscribeWith(disposables,
                sceneName => ShowScene(sceneName));

        public void OnBack()
            => _model.GetCharacterSelectScene().SubscribeWith(disposables,
                sceneName => ShowScene(sceneName));
    }
}