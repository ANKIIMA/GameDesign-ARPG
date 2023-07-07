using System;
using SharpUI.Source.Common.Util.Collision;
using SharpUI.Source.Common.Util.Collision.Data;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Button
{
    public class CircleButton : IconButton, ICanvasRaycastFilter
    {
        private CircleCollider2D _collider;
        private RectTransform _rectTransform;

        protected override void SetupElement()
        {
            base.SetupElement();
            _rectTransform = GetComponent<RectTransform>();
            _collider = gameObject.AddComponent<CircleCollider2D>();
        }

        protected override void SetupUI()
        {
            base.SetupUI();
            _collider.radius = ComputeRadius();
        }

        private float ComputeRadius()
        {
            var rect = _rectTransform.rect;
            return Math.Max(rect.width, rect.height) / 2.0f;
        }

        private ColliderData GetColliderData(Vector2 position, Camera eventCamera)
        {
            return new ColliderData
            {
                Point = position,
                Transform = _rectTransform,
                Camera = eventCamera,
                Collider = _collider
            };
        }

        public bool IsRaycastLocationValid(Vector2 position, Camera eventCamera)
        {
            return ColliderUtils.IsPointInsideCollider(GetColliderData(position, eventCamera));
        }
    }
}