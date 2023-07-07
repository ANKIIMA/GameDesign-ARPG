using SharpUI.Source.Common.Util.Collision.Data;
using UnityEngine;

namespace SharpUI.Source.Common.Util.Collision
{
    public static class ColliderUtils
    {
        public static bool IsPointInsideCollider(ColliderData data)
        {
            var isIntersecting = RectTransformUtility.ScreenPointToWorldPointInRectangle(
                data.Transform,
                data.Point,
                data.Camera,
                out var intersectPoint
            );
            
            if (isIntersecting)
                isIntersecting = data.Collider.OverlapPoint(intersectPoint);
 
            return isIntersecting;
        }
    }
}