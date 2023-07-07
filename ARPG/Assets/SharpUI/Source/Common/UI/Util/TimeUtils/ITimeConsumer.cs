namespace SharpUI.Source.Common.UI.Util.TimeUtils
{
    public interface ITimeConsumer
    {
        void ConsumeSeconds(float seconds);
        void ConsumeMilliseconds(float milliseconds);
        void ConsumeMinutes(float minutes);
        void ConsumeHours(float hours);
        void ConsumeDays(float days);
    }
}