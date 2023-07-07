using System;
using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Base.Component;
using UniRx;

namespace SharpUI.Source.Common.UI.Base.Presenter
{
    public class BasePresenter<TComponent> : IBasePresenter<TComponent> where TComponent : IBaseComponent
    {
        protected readonly CompositeDisposable disposables;

        [CanBeNull] private TComponent _component;

        protected BasePresenter() => disposables = new CompositeDisposable();

        protected BasePresenter(CompositeDisposable disposable) => disposables = disposable;
        
        public virtual void TakeComponent(TComponent ownedComponent) => _component = ownedComponent;

        public virtual void DropComponent() => _component = default;

        public virtual void OnComponentStarted() => _component?.SetupComponent();

        public virtual void OnComponentDestroyed() => disposables.Dispose();
        
        protected void OnComponent(Action<TComponent> action) => action(_component);

        protected void HideScene(string sceneName, Action onSceneUnloadComplete = null)
            => _component?.HideScene(sceneName, onSceneUnloadComplete);

        protected void ShowScene(string sceneName, Action onSceneLoadComplete = null)
            => _component?.ShowScene(sceneName, onSceneLoadComplete);

        protected void ShowSceneAdditive(string sceneName, Action onSceneLoadComplete = null)
            => _component?.ShowSceneAdditive(sceneName, onSceneLoadComplete);
    }
}