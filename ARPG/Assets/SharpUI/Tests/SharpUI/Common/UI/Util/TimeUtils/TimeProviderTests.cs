using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.TimeUtils;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.TimeUtils
{
    public class TimeProviderTests
    {
        private TimeProvider _timeProvider;

        [SetUp]
        public void SetUp()
        {
            _timeProvider = new TimeProvider();
        }

        [Test]
        public void GetDeltaTime_ReturnsPositiveValue()
        {
            var value = _timeProvider.GetDeltaTime();
            
            Assert.LessOrEqual(0, value);
        }

        [Test]
        public void GetFixedDeltaTime_ReturnsPositiveValue()
        {
            var value = _timeProvider.GetFixedDeltaTime();
            
            Assert.LessOrEqual(0, value);
        }
    }
}