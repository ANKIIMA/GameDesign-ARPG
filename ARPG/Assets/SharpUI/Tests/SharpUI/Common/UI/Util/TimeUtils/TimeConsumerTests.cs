using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.TimeUtils;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.TimeUtils
{
    public class TimeConsumerTests
    {
        private const float Days = 3f;
        private const float Hours = 8f;
        private const float Minutes = 14f;
        private const float Seconds = 45f;
        private const float Milliseconds = 3500f;
        
        private class FakeTimeConsumer : TimeConsumer
        {
            public float consumedSeconds;
            
            public override void ConsumeSeconds(float seconds)
            {
                consumedSeconds = seconds;
            }
        }

        private FakeTimeConsumer _consumer;

        [SetUp]
        public void SetUp()
        {
            _consumer = new FakeTimeConsumer();
        }

        [Test]
        public void ConsumeSeconds_WillConsumeSeconds()
        {
            _consumer.ConsumeSeconds(Seconds);
            
            Assert.AreEqual(Seconds, _consumer.consumedSeconds);
        }
        
        [Test]
        public void ConsumeMilliseconds_WillConsumeSeconds()
        {
            _consumer.ConsumeMilliseconds(Milliseconds);

            const float expected = Milliseconds / 1000f;
            Assert.AreEqual(expected, _consumer.consumedSeconds);
        }
        
        [Test]
        public void ConsumeMinutes_WillConsumeSeconds()
        {
            _consumer.ConsumeMinutes(Minutes);

            const float expected = Minutes * 60f;
            Assert.AreEqual(expected, _consumer.consumedSeconds);
        }
        
        [Test]
        public void ConsumeHours_WillConsumeSeconds()
        {
            _consumer.ConsumeHours(Hours);

            const float expected = Hours * 60f * 60f;
            Assert.AreEqual(expected, _consumer.consumedSeconds);
        }
        
        [Test]
        public void ConsumeDays_WillConsumeSeconds()
        {
            _consumer.ConsumeDays(Days);

            const float expected = Days * 60f * 60f * 24f;
            Assert.AreEqual(expected, _consumer.consumedSeconds);
        }
    }
}