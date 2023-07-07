using System;
using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.UI.Util.Scenes;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Base.Component
{
    public abstract class MonoBehaviourComponent<TComponent, TPresenter> : MonoBehaviour
        where TPresenter : class, IBasePresenter<TComponent>, new()
        where TComponent : IBaseComponent
    {
        private ISceneUtils _sceneUtils = new SceneUtils();
        private BaseComponent<TPresenter, TComponent> _component;

        protected MonoBehaviourComponent()
        {
            _component = new BaseComponent<TPresenter, TComponent>();
        }

        protected TPresenter GetPresenter() => _component.GetPresenter();
        
        protected abstract TComponent GetComponent();

        public void SetComponent(BaseComponent<TPresenter, TComponent> component) => _component = component;

        public void SetSceneUtils(ISceneUtils sceneUtils) => _sceneUtils = sceneUtils;
        
        public virtual void Awake() => _component.OnAwake(GetComponent());

        public virtual void Start() => _component.OnStart();
        
        public virtual void OnDisable() => _component.OnDisable();
        
        public virtual void OnDestroy() => _component.OnDestroy();

        public void ShowScene(string sceneName, Action onCompleteAction = null)
        {
            StartCoroutine(_sceneUtils.LoadSceneAsync(sceneName, () => onCompleteAction?.Invoke()));
        }

        public void ShowSceneAdditive(string sceneName, Action onCompleteAction)
        {
            StartCoroutine(_sceneUtils.LoadSceneAdditiveAsync(sceneName, () => onCompleteAction?.Invoke()));
        }

        public void HideScene(string sceneName, Action onCompleteAction)
        {
            StartCoroutine(_sceneUtils.UnloadSceneAsync(sceneName, () => onCompleteAction?.Invoke()));
        }
    }
}