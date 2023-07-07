using System;

namespace SharpUI.Source.Common.UI.Util.TimeUtils
{
    public abstract class TimeConsumer : ITimeConsumer
    {
        public void ConsumeMilliseconds(float milliseconds)
            => ConsumeSeconds((float) TimeSpan.FromMilliseconds(milliseconds).TotalSeconds);

        public void ConsumeMinutes(float minutes)
            => ConsumeSeconds((float) TimeSpan.FromMinutes(minutes).TotalSeconds);

        public void ConsumeHours(float hours)
            => ConsumeSeconds((float) TimeSpan.FromHours(hours).TotalSeconds);

        public void ConsumeDays(float days)
            => ConsumeSeconds((float) TimeSpan.FromDays(days).TotalSeconds);
        
        public abstract void ConsumeSeconds(float seconds);
    }
}