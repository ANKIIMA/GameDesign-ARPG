using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Decorators
{
    public class ScaleDecorator : BaseDecorator<float>
    {
        public enum ScaleType { ScaleXY, ScaleXYZ }

        [SerializeField] public bool isEnabled = true;
        [SerializeField] public ScaleType scaleType = ScaleType.ScaleXY;
        [SerializeField] public DecoratorMode decoratorMode = DecoratorMode.OnPressRelease;
        [SerializeField][Range(0.0f, 1.0f)] public float defaultScale;
        [SerializeField][Range(0.0f, 1.0f)] public float activeScale;

        private RectTransform _rectTransform;

        public void Awake()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
        }

        protected override void Decorate(float scale)
        {
            if (!isEnabled) return;
            
            _rectTransform.localScale = GetScaleVector(scale);
        }

        private Vector3 GetScaleVector(float scale)
        {
            return scaleType == ScaleType.ScaleXYZ
                ? new Vector3(scale, scale, scale)
                : new Vector3(scale, scale, _rectTransform.localScale.z);
        }

        protected override void DecoratePressed()
        {
            if (decoratorMode == DecoratorMode.OnPressRelease)
                Decorate(activeScale);
        }

        protected override void DecorateReleased()
        {
            if (decoratorMode == DecoratorMode.OnPressRelease)
                Decorate(defaultScale);
        }

        protected override void DecorateSelected()
        {
            if (decoratorMode == DecoratorMode.OnSelectDeselect)
                Decorate(activeScale);
        }

        protected override void DecorateDeselected()
        {
            if (decoratorMode == DecoratorMode.OnSelectDeselect)
                Decorate(defaultScale);
        }

        protected override void DecorateEnter()
        {
            if (decoratorMode == DecoratorMode.OnEnterExit)
                Decorate(activeScale);
        }

        protected override void DecorateExit()
        {
            if (decoratorMode == DecoratorMode.OnEnterExit)
                Decorate(defaultScale);
        }

        protected override void DecorateEnabled()
        {
            if (decoratorMode == DecoratorMode.OnEnableDisable)
                Decorate(activeScale);
        }

        protected override void DecorateDisabled()
        {
            if (decoratorMode == DecoratorMode.OnEnableDisable)
                Decorate(defaultScale);
        }
    }
}