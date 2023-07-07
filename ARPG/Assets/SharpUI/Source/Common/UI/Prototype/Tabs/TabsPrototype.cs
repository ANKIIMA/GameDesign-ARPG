using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Prototype.Tabs
{
    public class TabsPrototype : MonoBehaviour
    {
        [SerializeField] public ButtonSelectionGroup buttonGroup;
        [SerializeField] public TabButton tab5Button;
        [SerializeField] public Button toggleTab5Button;

        public void Start()
        {
            toggleTab5Button.OnClickAsObservable().SubscribeWith(this,
                _ =>
                {
                    if (tab5Button.GetState().IsEnabled())
                        tab5Button.DisableButton();
                    else
                        tab5Button.EnableButton();
                });
        }
    }
}