namespace SharpUI.Source.Common.UI.Util
{
    public interface IUiUtil
    {
        bool PercentInRange(float percent);
        float ToDecimalPercentage(float percentage);
        float ToPercentage(float decimalPercentage);
        float Half(float value);
    }
}