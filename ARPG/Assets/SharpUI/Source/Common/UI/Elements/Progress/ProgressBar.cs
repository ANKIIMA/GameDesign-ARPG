using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Progress
{
    public class ProgressBar : MonoBehaviour
    {
        public const string DefaultBarTextFormat = "{0:0.00} %";
        public const float DefaultPercentage = 0.0f;
        
        [SerializeField] public Image backgroundImage;
        [SerializeField] public Image barImage;
        [SerializeField,CanBeNull] public TMP_Text barText;

        protected readonly IUiUtil util = new UiUtil();
        protected float percentage;

        public virtual void Start()
        {
            percentage = DefaultPercentage;
            UpdatePercentage(percentage);
        }

        public virtual void UpdatePercentage(float percentAmount)
        {
            if (!util.PercentInRange(percentAmount))
                return;
            
            percentage = percentAmount;
            SetBarFill();
            SetTextPercentage();
        }

        public float GetPercentage() => percentage;

        public void SetBarText(string text)
        {
            if (!(barText is null))
                barText.text = text;
        }

        private void SetBarFill()
        {
            barImage.fillAmount = util.ToDecimalPercentage(percentage);
        }

        private void SetTextPercentage()
        {
            if (!(barText is null))
                barText.text = string.Format(DefaultBarTextFormat, percentage);
        }
    }
}