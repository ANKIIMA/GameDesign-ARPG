using SharpUI.Source.Common.UI.Base.Component;

namespace SharpUI.Source.Common.UI.Base.Presenter
{
    public interface IBasePresenter<in TComponent> where TComponent: IBaseComponent
    {
        void TakeComponent(TComponent component);

        void DropComponent();

        void OnComponentStarted();

        void OnComponentDestroyed();
    }
}