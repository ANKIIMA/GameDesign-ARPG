using System;
using UniRx;

namespace SharpUI.Source.Common.Util.Reactive
{
    public interface IDelayObserver
    {
        IObservable<long> DelayMilliseconds(long delay, IScheduler scheduler, int takeAmount = 1);
    }
}