using SharpUI.Source.Common.UI.Elements.Progress;
using SharpUI.Source.Common.UI.Elements.Toggle;
using SharpUI.Source.Common.UI.Util;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.SkillTrees
{
    public class SkillArrowLine : MonoBehaviour
    {
        [SerializeField] public ProgressBar progressBar;
        [SerializeField] public Image triangleImage;
        [SerializeField] public ToggleButton nodeOwner;
        [SerializeField] public Color activeColor;
        [SerializeField] public Color disabledColor;

        public void Start()
        {
            InitDefaults();
            ObserveToggleNodeChanges();
            Deactivate();
        }

        private void ObserveToggleNodeChanges()
        {
            nodeOwner.ObserveToggleStateChange().SubscribeWith(this, active =>
            {
                if (active) Activate();
                else Deactivate();
            });
        }

        private void InitDefaults()
        {
            progressBar.backgroundImage.color = disabledColor;
            progressBar.barImage.color = activeColor;
        }

        private void Activate()
        {
            progressBar.UpdatePercentage(UiUtil.Percent100);
            triangleImage.color = activeColor;
        }

        private void Deactivate()
        {
            progressBar.UpdatePercentage(UiUtil.Percent0);
            triangleImage.color = disabledColor;
        }
    }
}