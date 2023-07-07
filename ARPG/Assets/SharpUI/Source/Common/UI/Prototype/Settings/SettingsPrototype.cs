using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Util.Scenes;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Prototype.Settings
{
    public class SettingsPrototype : MonoBehaviour
    {
        [SerializeField] public IconButton closeButton;
        [SerializeField] public RectButton saveButton;
        [SerializeField] public RectButton applyButton;

        private readonly ISceneUtils _sceneUtils = new SceneUtils();
        
        public void Start()
        {
            saveButton.GetEventListener().ObserveOnClicked().SubscribeWith(this, _ => UnloadScene());
            closeButton.GetEventListener().ObserveOnClicked().SubscribeWith(this, _ => UnloadScene());
        }

        private void UnloadScene()
        {
            StartCoroutine(_sceneUtils.UnloadSceneAsync("Settings"));
        }
    }
}