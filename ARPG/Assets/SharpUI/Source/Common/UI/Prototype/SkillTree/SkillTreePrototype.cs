using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.TooltipInfo;
using SharpUI.Source.Common.UI.Util;
using SharpUI.Source.Common.UI.Util.Layout;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Prototype.SkillTree
{
    public class SkillTreePrototype : MonoBehaviour
    {
        [SerializeField] public TabButton utilityTabButton;
        [SerializeField] public GameObject utilityTooltipMessage;

        [SerializeField] public SkillTreeButton northWindSkillButton;
        [SerializeField] public SkillTreeButton reckoningShieldSkillButton;
        [SerializeField] public SkillTreeButton queensVenomSkillButton;
        [SerializeField] public SkillTreeButton quickStabSkillButton;
        [SerializeField] public SkillTreeButton dualWieldSkillButton;
        [SerializeField] public SkillTreeButton lightningCoilSkillButton;
        [SerializeField] public SkillTreeButton recklessSkiesSkillButton;
        [SerializeField] public SkillTreeButton onGuardSkillButton;
        [SerializeField] public SkillTreeButton focusedAttacksSkillButton;
        [SerializeField] public SkillTreeButton thunderstruckSkillButton;
        [SerializeField] public SkillTreeButton goWithTheWindSkillButton;
        [SerializeField] public SkillTreeButton fireballSkillButton;
        [SerializeField] public SkillTreeButton pierceThroughSkillButton;
        [SerializeField] public SkillTreeButton backstabSkillButton;
        [SerializeField] public GameObject northWindContentGameObject;
        [SerializeField] public GameObject reckoningShieldGameObject;
        [SerializeField] public GameObject queensVenomGameObject;
        [SerializeField] public GameObject quickStabGameObject;
        [SerializeField] public GameObject dualWieldGameObject;
        [SerializeField] public GameObject lightningCoilGameObject;
        [SerializeField] public GameObject recklessSkiesGameObject;
        [SerializeField] public GameObject onGuardGameObject;
        [SerializeField] public GameObject focusedAttacksGameObject;
        [SerializeField] public GameObject thunderstruckGameObject;
        [SerializeField] public GameObject goWithTheWindGameObject;
        [SerializeField] public GameObject fireballGameObject;
        [SerializeField] public GameObject pierceThroughGameObject;
        [SerializeField] public GameObject backstabGameObject;
        [SerializeField] public GameObject abilityTooltip;
        
        private ITooltip _abilityTooltip;
        
        public void Awake()
        {
            InitTooltips();
        }

        private void InitTooltips()
        {
            _abilityTooltip = abilityTooltip.GetComponent<ITooltip>();
            _abilityTooltip.SetShowDelayTimeMillis(300);
        }

        public void Start()
        {
            northWindSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnNorthWindEnter());
            northWindSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            reckoningShieldSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnReckoningShieldEnter());
            reckoningShieldSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            queensVenomSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnQueensVenomEnter());
            queensVenomSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            quickStabSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnQuickStabEnter());
            quickStabSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            dualWieldSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnDualWieldEnter());
            dualWieldSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            lightningCoilSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnLightningCoilEnter());
            lightningCoilSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            recklessSkiesSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnRecklessSkiesEnter());
            recklessSkiesSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            onGuardSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnOnGuardEnter());
            onGuardSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            focusedAttacksSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnFocusedAttacksEnter());
            focusedAttacksSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            thunderstruckSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnThunderstruckEnter());
            thunderstruckSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            goWithTheWindSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnGoWithTheWindEnter());
            goWithTheWindSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            fireballSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnFireballEnter());
            fireballSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            pierceThroughSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnPierceThroughEnter());
            pierceThroughSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            backstabSkillButton.GetEventListener().ObserveOnEntered().SubscribeWith(this, 
                _ => OnBackstabEnter());
            backstabSkillButton.GetEventListener().ObserveOnExited().SubscribeWith(this, 
                _ => _abilityTooltip.Hide());
            
            utilityTabButton.GetEventListener().ObserveOnEntered().SubscribeWith(this,
                _ => OnUtilityTabEntered());
            utilityTabButton.GetEventListener().ObserveOnExited().SubscribeWith(this,
                _ => _abilityTooltip.Hide());
        }

        private void OnUtilityTabEntered()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(-76, 0, 0, 106));
            _abilityTooltip.BindContent(utilityTooltipMessage.GetComponent<RectTransform>());
            _abilityTooltip.ShowToLeftOf(utilityTabButton.GetComponent<RectTransform>());
        }

        private void OnNorthWindEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent0);
            _abilityTooltip.SetMargins(new Margin(-64, 0, 0, 0));
            _abilityTooltip.BindContent(northWindContentGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowAbove(northWindSkillButton.GetComponent<RectTransform>());
        }

        private void OnReckoningShieldEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(reckoningShieldGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(reckoningShieldSkillButton.GetComponent<RectTransform>());
        }

        private void OnQueensVenomEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(queensVenomGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(queensVenomSkillButton.GetComponent<RectTransform>());
        }

        private void OnQuickStabEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(quickStabGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToLeftOf(quickStabSkillButton.GetComponent<RectTransform>());
        }

        private void OnDualWieldEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent0);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(dualWieldGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToLeftOf(dualWieldSkillButton.GetComponent<RectTransform>());
        }
        
        private void OnLightningCoilEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent0);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(lightningCoilGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(lightningCoilSkillButton.GetComponent<RectTransform>());
        }

        private void OnRecklessSkiesEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent0);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(recklessSkiesGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToLeftOf(recklessSkiesSkillButton.GetComponent<RectTransform>());
        }

        private void OnOnGuardEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent0);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(onGuardGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToLeftOf(onGuardSkillButton.GetComponent<RectTransform>());
        }

        private void OnFocusedAttacksEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent0);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(focusedAttacksGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToLeftOf(focusedAttacksSkillButton.GetComponent<RectTransform>());
        }

        private void OnThunderstruckEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent0);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(thunderstruckGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(thunderstruckSkillButton.GetComponent<RectTransform>());
        }

        private void OnGoWithTheWindEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(goWithTheWindGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(goWithTheWindSkillButton.GetComponent<RectTransform>());
        }

        private void OnFireballEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(fireballGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(fireballSkillButton.GetComponent<RectTransform>());
        }

        private void OnPierceThroughEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(pierceThroughGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(pierceThroughSkillButton.GetComponent<RectTransform>());
        }

        private void OnBackstabEnter()
        {
            _abilityTooltip.OffsetPointerByPercentage(UiUtil.Percent100);
            _abilityTooltip.SetMargins(new Margin(0, 0, 0, -64));
            _abilityTooltip.BindContent(backstabGameObject.GetComponent<RectTransform>());
            _abilityTooltip.ShowToRightOf(backstabSkillButton.GetComponent<RectTransform>());
        }
    }
}