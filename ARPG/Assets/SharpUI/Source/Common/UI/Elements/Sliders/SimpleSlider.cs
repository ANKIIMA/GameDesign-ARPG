using SharpUI.Source.Common.UI.Util;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Sliders
{
    public class SimpleSlider : MonoBehaviour
    {
        public enum SliderValueDisplayType
        {
            Percentage, RawIntValue,RawFloatValue, None
        }
        
        public const string DefaultPercentageTextFormat = "{0:0.0} %";
        public const string RawValueIntTextFormat = "{0:0}";
        public const string RawValueFloatTextFormat = "{0:0.0}";

        [SerializeField] public Slider slider;
        [SerializeField] public GameObject handle;
        [SerializeField] public TMP_Text percentageText;
        [SerializeField] public SliderValueDisplayType textDisplayType = SliderValueDisplayType.Percentage;

        private readonly IUiUtil _uiUtil = new UiUtil();

        public void Start()
        {
            ObserveValueChange();
            OnValueChanged(slider.value);
        }

        private void ObserveValueChange()
        {
            slider.OnValueChangedAsObservable().SubscribeWith(this, OnValueChanged);
        }

        private void OnValueChanged(float value)
        {
            SetFormattedText(value);
        }

        private void SetFormattedText(float value)
        {
            percentageText.gameObject.SetActive(true);
            switch (textDisplayType)
            {
                case SliderValueDisplayType.Percentage:
                    var percentage = _uiUtil.ToPercentage(value) / SliderRange();
                    percentageText.text = string.Format(DefaultPercentageTextFormat, percentage);
                    break;
                case SliderValueDisplayType.RawIntValue:
                    percentageText.text = string.Format(RawValueIntTextFormat, value);
                    break;
                case SliderValueDisplayType.RawFloatValue:
                    percentageText.text = string.Format(RawValueFloatTextFormat, value);
                    break;
                default:
                    percentageText.gameObject.SetActive(false);
                    break;
            }
        }

        private float SliderRange() => slider.maxValue - slider.minValue;

        public void ShowHandle() => handle.SetActive(true);

        public void HideHandle() => handle.SetActive(false);

        public void ShowPercentage() => percentageText.gameObject.SetActive(true);

        public void HidePercentage() => percentageText.gameObject.SetActive(false);
    }
}