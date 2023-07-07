using System.Globalization;
using System.Threading;
using SharpUI.Source.Common.UI.Base.Presenter;

namespace SharpUI.Source.Common.UI.Base.Component
{
    public class BaseComponent<TPresenter, TComponent>
        where TPresenter : class, IBasePresenter<TComponent>, new()
        where TComponent : IBaseComponent
    {
        private readonly TPresenter _presenter;

        public BaseComponent()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-us")
            {
                NumberFormat = { NumberDecimalSeparator = "." }
            };
            _presenter = new TPresenter();
        }

        public BaseComponent(TPresenter presenter) => _presenter = presenter;
        
        public TPresenter GetPresenter() => _presenter;

        public void OnAwake(TComponent component) => _presenter.TakeComponent(component);

        public void OnStart() => _presenter.OnComponentStarted();

        public void OnDisable() => _presenter.DropComponent();

        public void OnDestroy() => _presenter.OnComponentDestroyed();
    }
}