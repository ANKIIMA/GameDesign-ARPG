using System;

namespace SharpUI.Source.Common.UI.Util.TimeUtils
{
    public class CooldownTimeFormatter : TimeFormatter
    {
        private const float Seconds10 = 10.0f;
        private const float MinuteInSeconds = 60.0f;
        private const float HourInSeconds = 3600.0f;
        private const float DayInSeconds = 86400.0f;
        
        private const string FormatSecondLessThan10 = "{0:0.0}";
        private const string FormatSecondLessThan60 = "{0:0 s}";
        private const string FormatMinutes = "{0:0 m}";
        private const string FormatHours = "{0:0 h}";
        private const string FormatDays = "{0:0 d}";

        public override string FormatSeconds(float seconds)
        {
            if (seconds < Seconds10)
                return string.Format(FormatSecondLessThan10, seconds);
            
            if (seconds < MinuteInSeconds)
                return string.Format(FormatSecondLessThan60, seconds);

            var minutes = Math.Ceiling(seconds / MinuteInSeconds);
            if (seconds < HourInSeconds)
                return string.Format(FormatMinutes, minutes);

            var hours = Math.Ceiling(seconds / HourInSeconds);
            if (seconds < DayInSeconds)
                return string.Format(FormatHours, hours);

            var days = Math.Ceiling(seconds / DayInSeconds);
            return string.Format(FormatDays, days);
        }
    }
}