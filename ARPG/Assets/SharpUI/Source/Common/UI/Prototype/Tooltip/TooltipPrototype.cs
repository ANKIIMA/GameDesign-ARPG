using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Toggle;
using SharpUI.Source.Common.UI.Elements.TooltipInfo;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Prototype.Tooltip
{
    public class TooltipPrototype : MonoBehaviour
    {
        [SerializeField] public GameObject defaultTooltipGameObject;
        [SerializeField] public GameObject tooltipHoverGameObject;
        [SerializeField] public GameObject advancedTooltipGameObject;
        [SerializeField] public GameObject weaponTooltipGameObject;
        [SerializeField] public Button pointerLeft;
        [SerializeField] public Button pointerRight;
        [SerializeField] public Button pointerTop;
        [SerializeField] public Button pointerBottom;
        [SerializeField] public Slider pointerOffset;
        [SerializeField] public IconButton hoverButton;
        [SerializeField] public ToggleButton showOnLeft;
        [SerializeField] public ToggleButton showOnRight;
        [SerializeField] public ToggleButton showAbove;
        [SerializeField] public ToggleButton showBelow;
        [SerializeField] public Slider hoverOffset;
        [SerializeField] public TMP_Text textHoverTooltip;
        [SerializeField] public IconButton advancedButton;
        [SerializeField] public RectTransform advancedContent;
        [SerializeField] public IconButton weaponButton;
        [SerializeField] public RectTransform weaponContent;

        private ITooltip _defaultTooltip;
        private ITooltip _hoverTooltip;
        private ITooltip _advancedTooltip;
        private ITooltip _weaponTooltip;

        public void Awake()
        {
            _defaultTooltip = defaultTooltipGameObject.GetComponent<ITooltip>();
            _hoverTooltip = tooltipHoverGameObject.GetComponent<ITooltip>();
            _advancedTooltip = advancedTooltipGameObject.GetComponent<ITooltip>();
            _weaponTooltip = weaponTooltipGameObject.GetComponent<ITooltip>();
        }

        public void Start()
        {
            _defaultTooltip.OffsetPointerByPercentage(pointerOffset.value);
            _hoverTooltip.OffsetPointerByPercentage(hoverOffset.value);
            _hoverTooltip.BindContent(textHoverTooltip.GetComponent<RectTransform>());
            
            _weaponTooltip.OffsetPointerByPercentage(85f);
            _weaponTooltip.BindContent(weaponContent);
            
            _advancedTooltip.OffsetPointerByPercentage(25f);
            _advancedTooltip.BindContent(advancedContent);

            pointerLeft.OnClickAsObservable().SubscribeWith(this,
                _ => _defaultTooltip.PositionPointerTo(PointerPosition.Left));
            
            pointerRight.OnClickAsObservable().SubscribeWith(this,
                _ => _defaultTooltip.PositionPointerTo(PointerPosition.Right));
            
            pointerTop.OnClickAsObservable().SubscribeWith(this,
                _ => _defaultTooltip.PositionPointerTo(PointerPosition.Top));
            
            pointerBottom.OnClickAsObservable().SubscribeWith(this,
                _ => _defaultTooltip.PositionPointerTo(PointerPosition.Bottom));
            
            pointerOffset.OnValueChangedAsObservable().SubscribeWith(this,
                value => _defaultTooltip.OffsetPointerByPercentage(value));
            
            hoverOffset.OnValueChangedAsObservable().SubscribeWith(this,
                value => _hoverTooltip.OffsetPointerByPercentage(value));

            hoverButton.GetEventListener().ObserveOnEntered().SubscribeWith(this,
                _ => OnHoverButtonEntered());
            
            hoverButton.GetEventListener().ObserveOnExited().SubscribeWith(this,
                _ => OnHoverButtonExited());

            advancedButton.GetEventListener().ObserveOnEntered().SubscribeWith(this,
                _ => OnAdvancedHoverEntered());
            
            advancedButton.GetEventListener().ObserveOnExited().SubscribeWith(this,
                _ => OnAdvancedHoverExited());
            
            weaponButton.GetEventListener().ObserveOnEntered().SubscribeWith(this,
                _ => OnWeaponEntered());
            
            weaponButton.GetEventListener().ObserveOnExited().SubscribeWith(this,
                _ => OnWeaponExited());
        }

        private void OnWeaponEntered()
        {
            var rectTransform = weaponButton.GetComponent<RectTransform>();
            _weaponTooltip.ShowAbove(rectTransform);
        }

        private void OnWeaponExited()
        {
            _weaponTooltip.Hide();
        }

        private void OnAdvancedHoverEntered()
        {
            var rectTransform = advancedButton.GetComponent<RectTransform>();
            _advancedTooltip.ShowAbove(rectTransform);
        }

        private void OnAdvancedHoverExited()
        {
            _advancedTooltip.Hide();
        }

        private void OnHoverButtonEntered()
        {
            var rectTransform = hoverButton.GetComponent<RectTransform>();
            if (showOnLeft.isOn) _hoverTooltip.ShowToLeftOf(rectTransform);
            else if (showOnRight.isOn) _hoverTooltip.ShowToRightOf(rectTransform);
            else if (showAbove.isOn) _hoverTooltip.ShowAbove(rectTransform);
            else if (showBelow.isOn) _hoverTooltip.ShowBelow(rectTransform);
        }
        
        private void OnHoverButtonExited()
        {
            _hoverTooltip.Hide();
        }
    }
}