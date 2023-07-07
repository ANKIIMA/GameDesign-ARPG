using System;
using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Util.TimeUtils;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.SkillBars
{
    public class SkillBar : MonoBehaviour
    {
        public enum CooldownConsumeType { Fill, Drain }

        private const float SkillBarMargin = 2f;
        private const string DefaultBarTextFormat = "{0:0.0} sec";
        
        [SerializeField] public Image skillBarImage;
        [SerializeField] public Image skillIconImage;
        [SerializeField] public TMP_Text skillNameText;
        [SerializeField] public TMP_Text skillRemainingCooldownText;
        [SerializeField] public float skillCooldown;
        [SerializeField] public float skillCooldownRemaining;
        [SerializeField] public string skillName;
        [SerializeField] public CooldownConsumeType consumeType = CooldownConsumeType.Fill;
        [SerializeField] public bool depleteWhenCompleted;
        
        [CanBeNull] private Subject<Unit> _castFinishedObserver = new Subject<Unit>();
        private float _cooldownRemaining;
        private float _cooldown;
        private bool _isFinished;
        private bool _isCoolingDown;
        private RectTransform _rectTransform;
        private RectTransform _skillBarImageTransform;
        private ITimeProvider _timeProvider = new TimeProvider();
        [CanBeNull] private ManagedUiUpdate _managedUiUpdate;

        public void Start()
        {
            InitUiUpdater();
            InitDefaultValues();
            UpdateProgress(0);
        }

        public void OnDestroy()
        {
            _managedUiUpdate?.StopUiUpdates();
        }

        public void Cancel()
        {
            Deplete();
        }

        public void SetTimeProvider(ITimeProvider timeProvider) => _timeProvider = timeProvider;

        public void StartCooldown()
        {
            _isCoolingDown = true;
        }

        public void RestartCooldown()
        {
            InitDefaultValues();
            StartCooldown();
        }

        private void InitUiUpdater()
        {
            _managedUiUpdate = new ManagedUiUpdate();
            _managedUiUpdate.ObserveUiUpdate().SubscribeWith(this, UpdateProgress);
        }
        
        private void InitDefaultValues()
        {
            if (!IsValidSetup()) return;

            _isFinished = false;
            _cooldown = skillCooldown;
            _cooldownRemaining = skillCooldownRemaining;
            skillNameText.text = skillName;
            _rectTransform = GetComponent<RectTransform>();
            _skillBarImageTransform = skillBarImage.GetComponent<RectTransform>();
            InitConsumeType();
            SetRemainingCooldownText();
        }

        public void Update()
        {
            if (!_isCoolingDown) return;

            var deltaTime = _timeProvider.GetDeltaTime();
            _cooldownRemaining = Math.Max(0, _cooldownRemaining - deltaTime);
            _managedUiUpdate?.ConsumeDeltaTime(deltaTime);
        }

        public IObservable<Unit> ObserveCooldownFinished() => _castFinishedObserver;

        public bool IsFinished() => _isFinished;

        public bool IsUpdatingUi() => _managedUiUpdate != null && !_managedUiUpdate.IsDepleted();

        private void InitConsumeType()
        {
            var scale = consumeType == CooldownConsumeType.Drain ? -1 : 1;
            skillBarImage.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        }

        private bool IsValidSetup()
        {
            return skillCooldownRemaining > skillCooldown
                ? throw new OverflowException("Remaining cooldown should not be larger than cooldown.")
                : true;
        }

        public void UpdateProgress(float elapsedTime)
        {
            CheckStatus();

            if (_isFinished || !_isCoolingDown) return;
            
            SetRemainingCooldownText();
            SetBarLength();
        }
        
        private void CheckStatus()
        {
            if (_cooldownRemaining <= 0 && !_isFinished)
                SkillFinished();
        }
        
        private void SkillFinished()
        {
            _isFinished = true;
            _isCoolingDown = false;
            _castFinishedObserver?.OnNext(Unit.Default);

            if (depleteWhenCompleted)
                Deplete();
        }

        private void Deplete()
        {
            _castFinishedObserver = null;
            DestroyImmediate(gameObject);
        }

        private void SetRemainingCooldownText()
        {
            skillRemainingCooldownText.text = string.Format(DefaultBarTextFormat, _cooldownRemaining);
        }

        private void SetBarLength()
        {
            if (consumeType == CooldownConsumeType.Fill)
                FillBar();
            else
                DrainBar();
        }

        private void FillBar()
        {
            var progressPercentage = CurrentPercentage();
            var barProgressWidth = GetBarWidth() * progressPercentage;
            _skillBarImageTransform
                .SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, SkillBarMargin, barProgressWidth);
        }

        private void DrainBar()
        {
            var progressPercentage = 1f - CurrentPercentage();
            var barProgressWidth = GetBarWidth() * progressPercentage;
            _skillBarImageTransform
                .SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, SkillBarMargin, barProgressWidth);
        }

        private float CurrentPercentage() => (_cooldown - _cooldownRemaining) / _cooldown;

        private float GetBarWidth() => _rectTransform.rect.size.x - SkillBarMargin * 2;
    }
}