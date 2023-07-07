using System;
using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Elements.ActionBars;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.SkillBars;
using SharpUI.Source.Common.UI.Util.Keyboard;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;
using Notification = SharpUI.Source.Common.UI.Elements.Notifications.Notification;

namespace SharpUI.Source.Client.UI.Game.GameScreen
{
    public class GameSceneComponent : MonoBehaviourComponent<GameSceneComponent, GameScenePresenter>,
        IGameSceneComponent
    {
        [SerializeField] public GameObject skillBarPrefab;
        [SerializeField] public Notification notificationAchievement;
        [SerializeField] public Notification notificationSkills;
        [SerializeField] public GameObject castingSkillBarContainer;
        
        // Settings
        [SerializeField] public IconButton settingsButton;
        [SerializeField] public IconButton vendorButton;
        [SerializeField] public IconButton skillsButton;

        // Debuffs
        [SerializeField] public SkillBar queensVenom;
        [SerializeField] public SkillBar deadlyPoison;
        [SerializeField] public SkillBar thunderstruck;
        [SerializeField] public SkillBar backstab;
        
        // Action bar top buttons
        [SerializeField] public ActionBarButton actionBarTop1;
        [SerializeField] public ActionBarButton actionBarTop2;
        [SerializeField] public ActionBarButton actionBarTop3;
        [SerializeField] public ActionBarButton actionBarTop4;
        [SerializeField] public ActionBarButton actionBarTop5;
        [SerializeField] public ActionBarButton actionBarTop6;
        [SerializeField] public ActionBarButton actionBarTop7;
        [SerializeField] public ActionBarButton actionBarTop8;
        [SerializeField] public ActionBarButton actionBarTop9;
        [SerializeField] public ActionBarButton actionBarTop0;
        
        // Action bar bottom buttons
        [SerializeField] public ActionBarButton actionBarBottom1;
        [SerializeField] public ActionBarButton actionBarBottom2;
        [SerializeField] public ActionBarButton actionBarBottom3;
        [SerializeField] public ActionBarButton actionBarBottom4;
        [SerializeField] public ActionBarButton actionBarBottom5;
        [SerializeField] public ActionBarButton actionBarBottom6;
        [SerializeField] public ActionBarButton actionBarBottom7;
        [SerializeField] public ActionBarButton actionBarBottom8;
        [SerializeField] public ActionBarButton actionBarBottom9;
        [SerializeField] public ActionBarButton actionBarBottom0;
        
        protected override GameSceneComponent GetComponent() => this;
        [CanBeNull] private IGameScenePresenter _presenter;
        private SkillBar _activeSkillBar;

        public void SetupComponent()
        {
            _presenter = GetPresenter();
            
            notificationAchievement.SetTitle("Achievement");
            notificationAchievement.SetSubtitle("Reckless & Furious");
            notificationAchievement.ObserveOnClick().SubscribeWith(this,
                _ => { /* Show achievements */ });
            
            notificationSkills.SetTitle("Click to open Skills");
            notificationSkills.SetSubtitle("New Skills Unlocked!");
            notificationSkills.ObserveOnClick().SubscribeWith(this,
                _ => _presenter?.OnSkillsClicked());
            
            settingsButton.GetEventListener().ObserveOnClicked().SubscribeWith(this,
                _ => _presenter?.OnSettingsClicked());
            vendorButton.GetEventListener().ObserveOnClicked().SubscribeWith(this,
                _ => _presenter?.OnVendorClicked());
            skillsButton.GetEventListener().ObserveOnClicked().SubscribeWith(this,
                _ => _presenter?.OnSkillsClicked());

            StartDebuffs();
            ObserveKeysAndCoolDownBottom();
            ObserveKeysAndCoolDownTop();
            ObserveEscapeKey();
        }

        private void StartDebuffs()
        {
            queensVenom.ObserveCooldownFinished().SubscribeWith(this, _ => queensVenom.RestartCooldown());
            deadlyPoison.ObserveCooldownFinished().SubscribeWith(this, _ => deadlyPoison.RestartCooldown());
            thunderstruck.ObserveCooldownFinished().SubscribeWith(this, _ => thunderstruck.RestartCooldown());
            backstab.ObserveCooldownFinished().SubscribeWith(this, _ => backstab.RestartCooldown());
            
            queensVenom.StartCooldown();
            deadlyPoison.StartCooldown();
            thunderstruck.StartCooldown();
            backstab.StartCooldown();
        }

        private void ShowSkillBarCast(
            string skillName,
            float castTime,
            Color skillColor,
            Sprite skillSprite,
            Action action)
        {
            // If already casting this skill, don't restart :-)
            if (_activeSkillBar && _activeSkillBar.skillName == skillName)
                return;
            
            if (_activeSkillBar)
                CancelActiveSkillBarCast();

            castingSkillBarContainer.SetActive(false);
            _activeSkillBar = Instantiate(skillBarPrefab, castingSkillBarContainer.transform).GetComponent<SkillBar>();
            _activeSkillBar.skillName = skillName;
            _activeSkillBar.skillCooldown = castTime;
            _activeSkillBar.skillCooldownRemaining = castTime;
            _activeSkillBar.skillBarImage.color = skillColor;
            _activeSkillBar.skillIconImage.sprite = skillSprite;
            _activeSkillBar.skillIconImage.color = skillColor;
            _activeSkillBar.consumeType = SkillBar.CooldownConsumeType.Fill;
            _activeSkillBar.depleteWhenCompleted = true;
            _activeSkillBar.ObserveCooldownFinished().SubscribeWith(this, _ => action.Invoke());
            castingSkillBarContainer.SetActive(true);
            _activeSkillBar.StartCooldown();
        }

        private void CancelActiveSkillBarCast()
        {
            if (_activeSkillBar) _activeSkillBar.Cancel();
        }

        private void ObserveEscapeKey()
        {
            var escKeyListener = new GameObject().AddComponent<SimpleKeyListener>();
            escKeyListener.ListenToKey(KeyCode.Escape);
            escKeyListener.ObserveDown().SubscribeWith(this, _ => CancelActiveSkillBarCast());
        }

        private void ObserveKeysAndCoolDownBottom()
        {
            ObserveActionBarBottom1();
            ObserveActionBarBottom2();
            ObserveActionBarBottom3();
            ObserveActionBarBottom4();
            ObserveActionBarBottom5();
            ObserveActionBarBottom6();
            ObserveActionBarBottom7();
            ObserveActionBarBottom8();
            ObserveActionBarBottom9();
            ObserveActionBarBottom0();
        }
        
        private void ObserveKeysAndCoolDownTop()
        {
            ObserveActionBarTop1();
            ObserveActionBarTop2();
            ObserveActionBarTop3();
            ObserveActionBarTop4();
            ObserveActionBarTop5();
            ObserveActionBarTop6();
            ObserveActionBarTop7();
            ObserveActionBarTop8();
            ObserveActionBarTop9();
            ObserveActionBarTop0();
        }
        
        private void ObserveActionBarBottom1()
        {
            actionBarBottom1.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    if (actionBarBottom1.IsCoolingDown()) return;
                    ShowSkillBarCast("Quick Stab", 1f, actionBarBottom1.iconImage.color,
                        actionBarBottom1.iconImage.sprite, () => actionBarBottom1.CoolDown(3.8f));
                });
            actionBarBottom1.GetKeyListener().ListenToKey(KeyCode.Alpha1);
        }
        
        private void ObserveActionBarBottom2()
        {
            actionBarBottom2.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    if (actionBarBottom2.IsCoolingDown()) return;
                    ShowSkillBarCast("Fireball", 3.7f, actionBarBottom2.iconImage.color,
                        actionBarBottom2.iconImage.sprite, () => actionBarBottom2.CoolDown(9.6f));
                });
            actionBarBottom2.GetKeyListener().ListenToKey(KeyCode.Alpha2);
        }
        
        private void ObserveActionBarBottom3()
        {
            actionBarBottom3.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    if (actionBarBottom3.IsCoolingDown()) return;
                    ShowSkillBarCast("North Wind", 2.8f, actionBarBottom3.iconImage.color,
                        actionBarBottom3.iconImage.sprite, () => actionBarBottom3.CoolDown(180f));
                });
            actionBarBottom3.GetKeyListener().ListenToKey(KeyCode.Alpha3);
        }
        
        private void ObserveActionBarBottom4()
        {
            actionBarBottom4.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    if (actionBarBottom4.IsCoolingDown()) return;
                    ShowSkillBarCast("Lightning Coil", 1f, actionBarBottom4.iconImage.color,
                        actionBarBottom4.iconImage.sprite, () => actionBarBottom4.CoolDown(45f));
                });
            actionBarBottom4.GetKeyListener().ListenToKey(KeyCode.Alpha4);
        }
        
        private void ObserveActionBarBottom5()
        {
            actionBarBottom5.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    if (actionBarBottom5.IsCoolingDown()) return;
                    ShowSkillBarCast("On Guard", 6f, actionBarBottom5.iconImage.color,
                        actionBarBottom5.iconImage.sprite, () => actionBarBottom5.CoolDown(120f));
                });
            actionBarBottom5.GetKeyListener().ListenToKey(KeyCode.Alpha5);
        }

        private void ObserveActionBarBottom6()
        {
            actionBarBottom6.GetKeyListener().ListenToKey(KeyCode.Alpha6);
        }
        
        private void ObserveActionBarBottom7()
        {
            actionBarBottom7.GetKeyListener().ListenToKey(KeyCode.Alpha7);
        }
        
        private void ObserveActionBarBottom8()
        {
            actionBarBottom8.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarBottom8.CoolDown(120f);
                });
            actionBarBottom8.GetKeyListener().ListenToKey(KeyCode.Alpha8);
        }

        private void ObserveActionBarBottom9()
        {
            actionBarBottom9.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarBottom9.CoolDown(60f);
                });
            actionBarBottom9.GetKeyListener().ListenToKey(KeyCode.Alpha9);
        }
        
        private void ObserveActionBarBottom0()
        {
            actionBarBottom0.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarBottom0.CoolDown(30f);
                });
            actionBarBottom0.GetKeyListener().ListenToKey(KeyCode.Alpha0);
        }

        private void ObserveActionBarTop1()
        {
            actionBarTop1.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop1.CoolDown(25f);
                });
            actionBarTop1.GetKeyListener().ListenToKey(KeyCode.Q);
        }
        
        private void ObserveActionBarTop2()
        {
            actionBarTop2.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop2.CoolDown(125f);
                });
            actionBarTop2.GetKeyListener().ListenToKey(KeyCode.E);
        }
        
        private void ObserveActionBarTop3()
        {
            actionBarTop3.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop3.CoolDown(15f);
                });
            actionBarTop3.GetKeyListener().ListenToKey(KeyCode.R);
        }
        
        private void ObserveActionBarTop4()
        {
            actionBarTop4.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop4.CoolDown(5f);
                });
            actionBarTop4.GetKeyListener().ListenToKey(KeyCode.T);
        }
        
        private void ObserveActionBarTop5()
        {
            actionBarTop5.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop5.CoolDown(2f);
                });
            actionBarTop5.GetKeyListener().ListenToKey(KeyCode.F);
        }
        
        private void ObserveActionBarTop6()
        {
            actionBarTop6.GetKeyListener().ListenToKey(KeyCode.G);
        }
        
        private void ObserveActionBarTop7()
        {
            actionBarTop7.GetKeyListener().ListenToKey(KeyCode.C);
        }
        
        private void ObserveActionBarTop8()
        {
            actionBarTop8.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop8.CoolDown(120f);
                });
            actionBarTop8.GetKeyListener().ListenToKey(KeyCode.Q);
            actionBarTop8.GetKeyListener().RequireAnyShift();
            actionBarTop8.GetKeyListener().RequireAnyControl();
        }

        private void ObserveActionBarTop9()
        {
            actionBarTop9.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop9.CoolDown(60f);
                });
            actionBarTop9.GetKeyListener().ListenToKey(KeyCode.E);
            actionBarTop9.GetKeyListener().RequireAnyShift();
        }
        
        private void ObserveActionBarTop0()
        {
            actionBarTop0.GetEventListener().ObserveOnClicked()
                .SubscribeWith(this, _ =>
                {
                    CancelActiveSkillBarCast();
                    actionBarTop0.CoolDown(30f);
                });
            actionBarTop0.GetKeyListener().ListenToKey(KeyCode.R);
            actionBarTop0.GetKeyListener().RequireAnyControl();
        }
    }
}