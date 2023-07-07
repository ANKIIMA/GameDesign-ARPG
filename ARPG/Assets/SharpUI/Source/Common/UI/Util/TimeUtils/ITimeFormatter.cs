namespace SharpUI.Source.Common.UI.Util.TimeUtils
{
    public interface ITimeFormatter
    {
        string FormatSeconds(float second);
        string FormatMilliseconds(float milliseconds);
    }
}