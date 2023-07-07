using UnityEngine;

namespace SharpUI.Source.Common.Util.Collision.Data
{
    public class ColliderData
    {
        public Vector2 Point { get; set; }
        public RectTransform Transform { get; set; }
        public Camera Camera { get; set; }
        public Collider2D Collider { get; set; }
    }
}