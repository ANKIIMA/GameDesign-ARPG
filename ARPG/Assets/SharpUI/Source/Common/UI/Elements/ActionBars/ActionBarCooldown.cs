using System;
using SharpUI.Source.Common.UI.Util.TimeUtils;
using TMPro;
using UniRx;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.ActionBars
{
    public class ActionBarCooldown : TimeConsumer, IActionBarCooldown
    {
        private readonly Subject<Unit> _observeCooldownFinished = new Subject<Unit>();
        private readonly ITimeFormatter _timeFormatter = new CooldownTimeFormatter();
        private Image _slicedCooldownImage;
        private TMP_Text _cooldownText;
        private float _cooldownTime;
        private float _remainingTime;
        private bool _isConsumingTime;
        
        public void TakeCooldownText(TMP_Text text) => _cooldownText = text;
        
        public void TakeCooldownImage(Image image)
        {
            _slicedCooldownImage = image;
            _slicedCooldownImage.fillClockwise = false;
            _slicedCooldownImage.fillMethod = Image.FillMethod.Radial360;
        }

        public IObservable<Unit> ObserveCooldownFinished() => _observeCooldownFinished;

        public void SetFillMethod(Image.FillMethod method) => _slicedCooldownImage.fillMethod = method;

        public bool IsConsumingTime() => _isConsumingTime;
        
        public void CoolDown(float seconds)
        {
            _isConsumingTime = true;
            _cooldownTime = seconds;
            _remainingTime = seconds;
            _slicedCooldownImage.fillAmount = 1;
            _slicedCooldownImage.gameObject.SetActive(true);
            _cooldownText.gameObject.SetActive(true);
        }
        
        public void Expire()
        {
            CooldownFinished();
        }

        public override void ConsumeSeconds(float seconds)
        {
            if (!_isConsumingTime) return;
            
            _remainingTime -= seconds;
        }

        public void Update()
        {
            if (_remainingTime < 0)
            {
                CooldownFinished();
                return;
            }
            
            SetCooldownText();
            SetImageFill();
        }

        private void SetCooldownText() => _cooldownText.text = _timeFormatter.FormatSeconds(_remainingTime);

        private void SetImageFill() => _slicedCooldownImage.fillAmount = _remainingTime / _cooldownTime;

        private void CooldownFinished()
        {
            _isConsumingTime = false;
            _slicedCooldownImage.fillAmount = 0;
            _slicedCooldownImage.gameObject.SetActive(false);
            _cooldownText.gameObject.SetActive(false);
            _observeCooldownFinished.OnNext(Unit.Default);
        }
    }
}