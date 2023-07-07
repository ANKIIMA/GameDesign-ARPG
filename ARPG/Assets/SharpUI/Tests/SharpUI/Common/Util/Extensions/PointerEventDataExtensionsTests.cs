using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SharpUI.Tests.SharpUI.Common.Util.Extensions
{
    public class PointerEventDataExtensionsTests
    {
        [Test]
        public void CanDrag_WhenButtonLeftAndDragTriggerNull_IsTrue()
        {
            var data = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Left };

            var canDrag = data.CanDrag();
            
            Assert.IsTrue(canDrag);
        }
        
        [Test]
        public void CanDrag_WhenButtonNotLeftAndDragTriggerNull_IsFalse()
        {
            var data = new PointerEventData(EventSystem.current) { button = PointerEventData.InputButton.Middle };

            var canDrag = data.CanDrag();
            
            Assert.IsFalse(canDrag);
        }

        [Test]
        public void CanDrag_WhenButtonLeftAndDragTriggerNotFound_IsFalse()
        {
            var dragTrigger = new GameObject();
            var data = new PointerEventData(EventSystem.current)
            {
                button = PointerEventData.InputButton.Left,
                hovered = new List<GameObject>()
            };
            
            var canDrag = data.CanDrag(dragTrigger);
            
            Assert.IsFalse(canDrag);
        }
        
        [Test]
        public void CanDrag_WhenButtonLeftAndDragTriggerFound_IsTrue()
        {
            var dragTrigger = new GameObject();
            var data = new PointerEventData(EventSystem.current)
            {
                button = PointerEventData.InputButton.Left,
                hovered = new List<GameObject> { dragTrigger }
            };
            
            var canDrag = data.CanDrag(dragTrigger);
            
            Assert.IsTrue(canDrag);
        }
    }
}