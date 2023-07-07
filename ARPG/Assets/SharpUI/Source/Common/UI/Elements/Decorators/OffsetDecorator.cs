using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    public class OffsetDecorator : BaseDecorator<Vector3>
    {
        [SerializeField] public bool isEnabled = true;
        [SerializeField] public DecoratorMode decoratorMode = DecoratorMode.OnPressRelease;
        [SerializeField] public Vector3 offset;

        private RectTransform _rectTransform;
        private Vector3 _originalPosition;
        
        public void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            SaveState();
        }

        protected override void Decorate(Vector3 amount)
        {
            if (!isEnabled) return;
            
            _rectTransform.localPosition = _originalPosition + amount;
        }

        protected override void DecoratePressed()
        {
            SaveState();
            if (decoratorMode == DecoratorMode.OnPressRelease)
                Decorate(offset);
        }

        protected override void DecorateReleased()
        {
            if (decoratorMode == DecoratorMode.OnPressRelease)
                Decorate(Vector3.zero);
        }

        protected override void DecorateSelected()
        {
            SaveState();
            if (decoratorMode == DecoratorMode.OnSelectDeselect)
                Decorate(offset);
        }

        protected override void DecorateDeselected()
        {
            if (decoratorMode == DecoratorMode.OnSelectDeselect)
                Decorate(Vector3.zero);
        }

        protected override void DecorateEnter()
        {
            SaveState();
            if (decoratorMode == DecoratorMode.OnEnterExit)
                Decorate(offset);
        }

        protected override void DecorateExit()
        {
            if (decoratorMode == DecoratorMode.OnEnterExit)
                Decorate(Vector3.zero);
        }

        protected override void DecorateEnabled()
        {
            SaveState();
            if (decoratorMode == DecoratorMode.OnEnableDisable)
                Decorate(offset);
        }

        protected override void DecorateDisabled()
        {
            if (decoratorMode == DecoratorMode.OnEnableDisable)
                Decorate(Vector3.zero);
        }

        private void SaveState()
        {
            _originalPosition = _rectTransform.localPosition;
        }
    }
}