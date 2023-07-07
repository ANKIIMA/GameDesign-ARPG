using System;
using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements.Toggle
{
    public class ToggleButton : BaseButton
    {
        [SerializeField] public Image checkedImage;
        [SerializeField] public bool isOn;
        
        [CanBeNull] private Subject<bool> _toggleObserver;

        public override void Awake()
        {
            base.Awake();
            SetToggleState();
            ObserveToggle();
        }

        private void SetToggleState()
        {
            checkedImage.enabled = isOn;
        }
        
        private void ObserveToggle()
        {
            dispatcher.ObserveOnClicked().SubscribeWith(this, _ => ToggleState());
        }

        public void SetIsOn(bool on)
        {
            isOn = on;
            SetToggleState();
        }

        private void ToggleState()
        {
            isOn = !isOn;
            _toggleObserver?.OnNext(isOn);
            SetToggleState();
        }

        public void ToggleOn()
        {
            if (isOn) return;
            ToggleState();
        }

        public void ToggleOff()
        {
            if (!isOn) return;
            ToggleState();
        }

        public void EnableToggle()
        {
            if (button.interactable) return;
            
            button.interactable = true;
            state.Enable();
            dispatcher.OnEnabled();
            decorators.OnEnabled();
        }

        public void DisableToggle()
        {
            if (button.interactable == false) return;
            
            button.interactable = false;
            state.Disable();
            dispatcher.OnDisabled();
            decorators.OnDisabled();
        }

        public IObservable<bool> ObserveToggleStateChange()
            => _toggleObserver ?? (_toggleObserver = new Subject<bool>());
    }
}