using System;
using SharpUI.Source.Common.UI.Elements.Progress;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Loading
{
    public class LoadingBar : ProgressBar
    {
        private const float PivotOffset = 8.0f;
        [SerializeField] public Image pivotImage;

        public override void UpdatePercentage(float percentAmount)
        {
            base.UpdatePercentage(percentAmount);
            SetPivotPosition();
        }

        private void SetPivotPosition()
        {
            var position = pivotImage.GetComponent<RectTransform>().anchoredPosition;
            var barWidth = barImage.GetComponent<RectTransform>().rect.width;
            var newWidth = util.ToDecimalPercentage(percentage) * barWidth;
            position.x = Math.Min(newWidth, barWidth - PivotOffset);
            position.x = Math.Max(PivotOffset, position.x);
            pivotImage.GetComponent<RectTransform>().anchoredPosition = position;
        }
    }
}