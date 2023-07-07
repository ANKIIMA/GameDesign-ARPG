using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Util.TimeUtils;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.ActionBars
{
    public class ActionBarButton : RectButton
    {
        [SerializeField] public Image iconImage;
        [SerializeField] public Image cooldownFillImage;
        [SerializeField] public TMP_Text cooldownText;
        [SerializeField] public TMP_Text topText;
        [SerializeField] public TMP_Text bottomText;
        [SerializeField] public Image.FillMethod fillMethod = Image.FillMethod.Radial360;
        [SerializeField] public bool canOverrideCooldown;

        [CanBeNull] private IActionBarCooldown _cooldown;
        [CanBeNull] private ManagedUiUpdate _managedUiUpdate;
        private ITimeProvider _timeProvider = new TimeProvider();
        private bool _isCoolingDown;

        protected override void SetupUI()
        {
            base.SetupUI();
            InitCooldown();
            InitUiUpdater();
        }

        public void SetTimeProvider(ITimeProvider timeProvider) => _timeProvider = timeProvider;

        private void InitCooldown()
        {
            _cooldown = new ActionBarCooldown();
            _cooldown.TakeCooldownImage(cooldownFillImage);
            _cooldown.TakeCooldownText(cooldownText);
            _cooldown.SetFillMethod(fillMethod);
            _cooldown.ObserveCooldownFinished().SubscribeWith(this, _ => CoolDownFinished());
            _cooldown?.Expire();
        }

        private void InitUiUpdater()
        {
            _managedUiUpdate = new ManagedUiUpdate();
            _managedUiUpdate.SetUiUpdateTimeout(0.02f);
            _managedUiUpdate.ObserveUiUpdate().SubscribeWith(this, elapsedTime => _cooldown.Update());
        }

        public void SetActionBarCooldown(IActionBarCooldown cooldown) => _cooldown = cooldown;
        
        public void Update()
        {
            if (!_isCoolingDown) return;

            var deltaTime = _timeProvider.GetDeltaTime();
            _cooldown?.ConsumeSeconds(deltaTime);
            _managedUiUpdate?.ConsumeDeltaTime(deltaTime);
        }

        public void CoolDown(float seconds)
        {
            if (!CanCoolDown()) return;
            
            _isCoolingDown = true;
            _cooldown?.CoolDown(seconds);
        }

        public bool IsCoolingDown() => _isCoolingDown;

        private bool CanCoolDown()
        {
            if (!IsAbilityBound())
                return false;
            
            return canOverrideCooldown || !_isCoolingDown;
        }

        private bool IsAbilityBound() => iconImage.gameObject.activeSelf;

        private void CoolDownFinished()
        {
            _isCoolingDown = false;
        }

        public void SetFillMethod(Image.FillMethod method) => _cooldown?.SetFillMethod(method);
    }
}