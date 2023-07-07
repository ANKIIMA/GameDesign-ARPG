namespace SharpUI.Source.Common.UI.Util
{
    public class UiUtil : IUiUtil
    {
        public const float Percent100 = 100f;
        public const float Percent0 = 0f;
        private const float HalfMultiplier = 0.5f;

        public float ToDecimalPercentage(float percentage) => percentage / Percent100;

        public float ToPercentage(float decimalPercentage) => decimalPercentage * Percent100;

        public bool PercentInRange(float percent) => percent >= Percent0 && percent <= Percent100;
        
        public float Half(float value) => HalfMultiplier * value;
    }
}