using NUnit.Framework;
using SharpUI.Source.Common.Util.Reactive;
using UniRx;

namespace SharpUI.Tests.SharpUI.Common.Util.Reactive
{
    public class DelayObserverTests
    {
        private DelayObserver _delayObserver;

        [SetUp]
        public void SetUp()
        {
            _delayObserver = new DelayObserver();
        }

        [Test]
        public void DelayMilliseconds_WillReturnCorrectType()
        {
            var expected = Observable.Return(3L).Take(34).GetType();
            var delay = _delayObserver.DelayMilliseconds(123, Scheduler.Immediate, 3);
            
            Assert.AreEqual(expected, delay.GetType());
        }
    }
}