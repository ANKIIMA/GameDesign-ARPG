namespace SharpUI.Source.Common.UI.Util.Layout
{
    public class Margin
    {
        public static Margin Zero => new Margin(0, 0, 0, 0);
        public float Left { get; }
        public float Right { get; }
        public float Top { get; }
        public float Bottom { get; }
        
        public Margin(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}