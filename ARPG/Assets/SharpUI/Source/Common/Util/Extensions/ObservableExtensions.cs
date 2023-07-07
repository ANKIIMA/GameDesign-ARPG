using System;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.Util.Extensions
{
    public static class ObservableExtensions
    {
        public static void SubscribeWith<T>(
            this IObservable<T> observable,
            Component component,
            Action<T> action)
        {
            observable.Subscribe(action).AddTo(component);
        }
        
        public static void SubscribeWith<T>(
            this IObservable<T> observable,
            Component component,
            Action<T> action,
            Action<Exception> errorAction)
        {
            observable.Subscribe(action, errorAction).AddTo(component);
        }
        
        public static void SubscribeWith<T>(
            this IObservable<T> observable,
            CompositeDisposable disposable,
            Action<T> action)
        {
            observable.Subscribe(action).AddTo(disposable);
        }
        
        public static void SubscribeWith<T>(
            this IObservable<T> observable,
            CompositeDisposable disposable,
            Action<T> action,
            Action<Exception> errorAction)
        {
            observable.Subscribe(action, errorAction).AddTo(disposable);
        }
        
        public static T BlockingValue<T>(this IObservable<T> observable)
        {
            return observable.ToTask().Result;
        }
    }
}