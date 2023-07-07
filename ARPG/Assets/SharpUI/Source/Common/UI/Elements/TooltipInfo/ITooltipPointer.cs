namespace SharpUI.Source.Common.UI.Elements.TooltipInfo
{
    public interface ITooltipPointer
    {
        float Width { get; }
        float Height { get; }

        void SetPosition(PointerPosition pointerPosition);
        void SetOffsetPercentage(float percentage);
        float OffsetSize();
    }
}