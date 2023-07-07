using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Drag;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.ModalViews
{
    public class ModalView : Draggable
    {
        [SerializeField] public IconButton closeButton;
        [SerializeField] public IconButton collapseButton;
        [SerializeField] public RectTransform headerRectTransform;
        [SerializeField] public RectTransform contentRectTransform;
        [SerializeField] public RectTransform backgroundRectTransform;
        [SerializeField] public bool isCollapsed;
        [SerializeField] public bool isDraggable;
        [SerializeField] public bool showBackground;

        private bool _isCollapsed;
        
        public void Start()
        {
            SetUp();
            InitCollapse();
            InitDrag();
        }

        private void InitCollapse()
        {
            _isCollapsed = isCollapsed;
            SetCollapse();
        }

        private void SetUp()
        {
            if (closeButton.gameObject.activeSelf)
                closeButton.GetEventListener().ObserveOnClicked().SubscribeWith(this,
                    _ => CloseModalView());

            if (collapseButton.gameObject.activeSelf)
                collapseButton.GetEventListener().ObserveOnClicked().SubscribeWith(this,
                    _ => ToggleCollapse());
            
            backgroundRectTransform.gameObject.SetActive(showBackground);
        }

        private void InitDrag()
        {
            if (!isDraggable) return;
            
            SetDragGameObjects(headerRectTransform.gameObject, gameObject);
        }

        private void CloseModalView()
        {
            DestroyImmediate(gameObject);
        }

        private void ToggleCollapse()
        {
            _isCollapsed = !_isCollapsed;
            SetCollapse();
        }

        private void SetCollapse()
        {
            if (_isCollapsed)
                Collapse();
            else
                Expand();
        }

        private void Collapse()
        {
            contentRectTransform.gameObject.SetActive(false);
            backgroundRectTransform.gameObject.SetActive(false);
            var angles = collapseButton.iconImage.GetComponent<RectTransform>().eulerAngles;
            angles.z = 90;
            collapseButton.iconImage.GetComponent<RectTransform>().eulerAngles = angles;
            
        }
        
        private void Expand()
        {
            contentRectTransform.gameObject.SetActive(true);
            backgroundRectTransform.gameObject.SetActive(showBackground);
            var angles = collapseButton.iconImage.GetComponent<RectTransform>().eulerAngles;
            angles.z = -90;
            collapseButton.iconImage.GetComponent<RectTransform>().eulerAngles = angles;
        }
    }
}