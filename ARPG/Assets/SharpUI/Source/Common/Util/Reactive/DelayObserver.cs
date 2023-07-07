using System;
using UniRx;

namespace SharpUI.Source.Common.Util.Reactive
{
    public class DelayObserver : IDelayObserver
    {
        public IObservable<long> DelayMilliseconds(long delay, IScheduler scheduler, int takeAmount = 1)
            => Observable
                .Interval(TimeSpan.FromMilliseconds(delay), scheduler)
                .Take(takeAmount);
    }
}