using System;
using UniRx;

namespace SharpUI.Source.Common.UI.Util.TimeUtils
{
    public class ManagedUiUpdate
    {
        private const float DefaultUiUpdateTimeoutSeconds = 0.01f;
        private readonly Subject<float> _uiUpdateObservable = new Subject<float>();
        private float _timeoutSeconds = DefaultUiUpdateTimeoutSeconds;
        private float _consumedTime;
        private bool _depleted;

        public IObservable<float> ObserveUiUpdate() => _uiUpdateObservable;

        public void ConsumeDeltaTime(float deltaTime)
        {
            ConsumeSeconds(deltaTime);
        }

        public bool IsDepleted() => _depleted;

        public void StopUiUpdates() => _depleted = true;

        public void SetUiUpdateTimeout(float seconds)
        {
            _timeoutSeconds = seconds;
            _consumedTime = 0;
        }

        private void ConsumeSeconds(float seconds)
        {
            if (_depleted) return;
            
            _consumedTime += seconds;

            if (!(_timeoutSeconds <= 0) && !IsAllConsumed()) return;
            
            UpdateUiAndReset();
        }

        private bool IsAllConsumed() => _consumedTime > _timeoutSeconds;

        private void UpdateUiAndReset()
        {
            _uiUpdateObservable.OnNext(_consumedTime);
            _consumedTime = 0;
        }
    }
}