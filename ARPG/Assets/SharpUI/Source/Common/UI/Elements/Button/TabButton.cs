using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Button
{
    public class TabButton : RectButton
    {
        [SerializeField] public GameObject content;

        public override void Awake()
        {
            base.Awake();
            
            GetEventListener().ObserveOnSelected().SubscribeWith(this,
                _ => SetContentVisibility(true));
            GetEventListener().ObserveOnDeselected().SubscribeWith(this,
                _ => SetContentVisibility(false));
        }

        private void SetContentVisibility(bool visible)
        {
            if (content != null)
                content.SetActive(visible);
        }
    }
}