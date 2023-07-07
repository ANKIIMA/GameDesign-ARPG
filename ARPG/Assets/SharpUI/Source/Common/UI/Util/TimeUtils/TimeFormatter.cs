using System;

namespace SharpUI.Source.Common.UI.Util.TimeUtils
{
    public abstract class TimeFormatter : ITimeFormatter
    {
        public abstract string FormatSeconds(float second);
        
        public string FormatMilliseconds(float milliseconds)
            => FormatSeconds((float) TimeSpan.FromMilliseconds(milliseconds).TotalSeconds);
    }
}