using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SharpUI.Source.Common.UI.Elements.Drag
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private GameObject _dragTrigger;
        private GameObject _dragElement;
        private bool _isDraggable;

        protected void SetDragGameObjects(GameObject dragTrigger, GameObject dragElement)
        {
            _dragTrigger = dragTrigger;
            _dragElement = dragElement;
        }

        public bool IsDraggable() => _isDraggable;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDraggable = eventData.hovered.Contains(_dragTrigger);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isDraggable || !eventData.CanDrag()) return;
            
            OnDragged(eventData.delta);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDraggable = false;
        }

        private void OnDragged(Vector2 size)
        {
            var pos = _dragElement.GetComponent<RectTransform>().localPosition + new Vector3(size.x, size.y, 0);
            _dragElement.GetComponent<RectTransform>().localPosition = pos;
        }
    }
}