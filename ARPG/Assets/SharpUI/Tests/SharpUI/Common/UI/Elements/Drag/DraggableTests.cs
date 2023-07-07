using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Drag;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Drag
{
    public class DraggableTests
    {
        private readonly Vector2 _deltaDrag = new Vector2(3.4f, -6.7f);
        
        private class FakeDraggable : Draggable
        {
            public void Set(GameObject dragTrigger, GameObject dragElement)
            {
                SetDragGameObjects(dragTrigger, dragElement);
            }
        }
        
        private GameObject _gameObject;
        private GameObject _dragTrigger;
        private GameObject _dragElement;
        private FakeDraggable _draggable;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _dragTrigger = new GameObject();
            _dragElement = new GameObject();
            _dragElement.AddComponent<RectTransform>();
            _draggable = _gameObject.AddComponent<FakeDraggable>();
            _draggable.Set(_dragTrigger, _dragElement);
        }
        
        [Test]
        public void OnBeginDrag_DoesNotContainDragTrigger_WillNotBeDraggable()
        {
            var eventData = new PointerEventData(EventSystem.current) { hovered = new List<GameObject>() };
            
            _draggable.OnBeginDrag(eventData);
            
            Assert.IsFalse(_draggable.IsDraggable());
        }

        [Test]
        public void OnBeginDrag_ContainsDragTrigger_WillBeDraggable()
        {
            var eventData = new PointerEventData(EventSystem.current) { hovered = new List<GameObject> { _dragTrigger } };
            
            _draggable.OnBeginDrag(eventData);
            
            Assert.IsTrue(_draggable.IsDraggable());
        }

        [Test]
        public void OnDrag_WhenDraggableAndLeftClick_WillDrag()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                button = PointerEventData.InputButton.Left,
                hovered = new List<GameObject> { _dragTrigger },
                delta = _deltaDrag
            };
            _draggable.OnBeginDrag(eventData);
            
            _draggable.OnDrag(eventData);

            var localPosition = _dragElement.GetComponent<RectTransform>().localPosition;
            Assert.AreEqual(_deltaDrag.x, localPosition.x);
            Assert.AreEqual(_deltaDrag.y, localPosition.y);
            Assert.AreEqual(0, localPosition.z);
        }

        [Test]
        public void OnEndDrag_WillNotBeDraggable()
        {
            var eventData = new PointerEventData(EventSystem.current) { hovered = new List<GameObject> { _dragTrigger } };
            _draggable.OnBeginDrag(eventData);
            
            _draggable.OnEndDrag(eventData);
            
            Assert.IsFalse(_draggable.IsDraggable());
        }
    }
}