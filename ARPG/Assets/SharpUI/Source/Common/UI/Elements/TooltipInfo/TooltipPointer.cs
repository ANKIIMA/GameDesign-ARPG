using SharpUI.Source.Common.UI.Util;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.TooltipInfo
{
    public class TooltipPointer : MonoBehaviour, ITooltipPointer
    {
        public const float LeftAngleZ = 0f;
        public const float RightAngleZ = 180f;
        public const float TopAngleZ = 270f;
        public const float BottomAngleZ = 90f;

        private static Vector2 LeftDelta => new Vector2(2.0f, 0.0f);
        private static Vector2 RightDelta => new Vector2(-2.0f, 0.0f);
        private static Vector2 TopDelta => new Vector2(2.0f, -2.0f);
        private static Vector2 BottomDelta => new Vector2(2.0f, 2.0f);

        private float _offset;
        private PointerPosition _pointerPosition;
        private RectTransform _rectTransform;
        private Vector3 _originalLocalPosition;
        private Quaternion _originalRotation;
        private readonly IUiUtil _util = new UiUtil();
        
        public float Width => _rectTransform.sizeDelta.x;
        
        public float Height => _rectTransform.sizeDelta.y;
        
        private Vector2 PointerSize => _rectTransform.sizeDelta;
        
        private Vector2 GetParentRectSize => transform.parent.GetComponent<RectTransform>().sizeDelta;
        
        private float LeftHorizontalOffset => -_util.Half(PointerSize.x);

        private float RightHorizontalOffset => GetParentRectSize.x + _util.Half(PointerSize.x);

        private float TopVerticalOffset => GetParentRectSize.y + _util.Half(PointerSize.x);
        
        private float BottomVerticalOffset => -_util.Half(PointerSize.x);

        private float PointerOffsetSize => OffsetSize() + _util.Half(PointerSize.y);

        public void Awake()
        {
            InitDefaults();
        }

        public void Start()
        {
            SetPosition(PointerPosition.Left);
        }

        private void InitDefaults()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalLocalPosition = _rectTransform.localPosition;
            _originalRotation = _rectTransform.rotation;
            _offset = 0;
        }

        public void SetPosition(PointerPosition pointerPosition)
        {
            _pointerPosition = pointerPosition;
            Position();
        }

        private void Position()
        {
            switch (_pointerPosition)
            {
                case PointerPosition.Left: PositionLeft();
                    break;
                case PointerPosition.Right: PositionRight();
                    break;
                case PointerPosition.Top: PositionTop();
                    break;
                case PointerPosition.Bottom: PositionBottom();
                    break;
            }
        }

        public void SetOffsetPercentage(float percentage)
        {
            if (!_util.PercentInRange(percentage))
                return;

            _offset = _util.ToDecimalPercentage(percentage);
            Position();
        }

        private void OffsetPositionWith(Vector2 offset)
        {
            _rectTransform.localPosition = new Vector3(
                _originalLocalPosition.x + offset.x,
                _originalLocalPosition.y + offset.y,
                _originalLocalPosition.z);
        }

        private void SetRotation(float angleZ)
        {
            _rectTransform.rotation = _originalRotation;
            _rectTransform.Rotate(0, 0, angleZ);
        }

        public float OffsetSize()
        {
            switch (_pointerPosition)
            {
                case PointerPosition.Left:
                case PointerPosition.Right:
                    return _offset * (GetParentRectSize.y - PointerSize.y);
                case PointerPosition.Bottom:
                    return _offset * (GetParentRectSize.x - PointerSize.y - 2 * BottomDelta.x);
                case PointerPosition.Top:
                    return _offset * (GetParentRectSize.x - PointerSize.y - 2 * TopDelta.x);
                default:
                    return 0;
            }
        }

        private Vector2 LeftOffset()
        {
            return new Vector2(
                LeftHorizontalOffset + LeftDelta.x,
                PointerOffsetSize + LeftDelta.y);
        }

        private Vector2 RightOffset()
        {
            return new Vector2(
                RightHorizontalOffset + RightDelta.x,
                PointerOffsetSize + RightDelta.y);

        }

        private Vector2 TopOffset()
        {
            return new Vector2(
                PointerOffsetSize + TopDelta.x,
                TopVerticalOffset + TopDelta.y);
        }

        private Vector2 BottomOffset()
        {
            return new Vector2(
                PointerOffsetSize + BottomDelta.x,
                BottomVerticalOffset + BottomDelta.y);
        }
        
        private void PositionLeft()
        {
            var offset = LeftOffset();
            OffsetPositionWith(offset);
            SetRotation(LeftAngleZ);
        }
        
        private void PositionRight()
        {
            OffsetPositionWith(RightOffset());
            SetRotation(RightAngleZ);
        }
        
        private void PositionTop()
        {
            OffsetPositionWith(TopOffset());
            SetRotation(TopAngleZ);
        }

        private void PositionBottom()
        {
            OffsetPositionWith(BottomOffset());
            SetRotation(BottomAngleZ);
        }
    }
}