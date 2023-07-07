using System.Globalization;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.TimeUtils;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.TimeUtils
{
    public class TimeFormatterTests
    {
        private const float Milliseconds = 34612f;
        private const float Seconds = 34.612f;

        private class FakeTimeFormatter : TimeFormatter
        {
            public override string FormatSeconds(float seconds)
            {
                return seconds.ToString(CultureInfo.CurrentCulture);
            }
        }

        private TimeFormatter _formatter;

        [SetUp]
        public void SetUp()
        {
            _formatter = new FakeTimeFormatter();
        }

        [Test]
        public void FormatSeconds_WillFormatSeconds()
        {
            var formatted = _formatter.FormatSeconds(Seconds);

            var expected = Seconds.ToString(CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, formatted);
        }
        
        [Test]
        public void FormatMilliseconds_WillFormatSeconds()
        {
            var formatted = _formatter.FormatMilliseconds(Milliseconds);

            var expected = Seconds.ToString(CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, formatted);
        }
    }
}