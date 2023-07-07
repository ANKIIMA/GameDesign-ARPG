using System;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.Util.Extensions
{
    public class ObservableExtensionsTests
    {
        private const string RandomStringValue = "Some random string value 1234.";
        
        [Test]
        public void SubscribeWith_Component_WillSubscribeAction()
        {
            var component = new Component();
            var observable = Substitute.For<IObservable<Unit>>();

            observable.SubscribeWith(component, _ => {});
            
            observable.Received().Subscribe(Arg.Any<IObserver<Unit>>());
        }
        
        [Test]
        public void SubscribeWith_Component_WillSubscribeActionAndError()
        {
            var component = new Component();
            var observable = Substitute.For<IObservable<Unit>>();

            observable.SubscribeWith(component, _ => {}, exception => {});
            
            observable.Received().Subscribe(Arg.Any<IObserver<Unit>>());
        }
        
        [Test]
        public void SubscribeWith_CompositeDisposable_WillSubscribeAction()
        {
            var disposable = new CompositeDisposable();
            var observable = Substitute.For<IObservable<Unit>>();

            observable.SubscribeWith(disposable, _ => {});
            
            observable.Received().Subscribe(Arg.Any<IObserver<Unit>>());
        }
        
        [Test]
        public void SubscribeWith_CompositeDisposable_WillSubscribeActionAndError()
        {
            var disposable = new CompositeDisposable();
            var observable = Substitute.For<IObservable<Unit>>();

            observable.SubscribeWith(disposable, _ => {}, exception => {});
            
            observable.Received().Subscribe(Arg.Any<IObserver<Unit>>());
        }

        [Test]
        public void BlockingValue_WillReturnRawValue()
        {
            var observable = Observable.Return(RandomStringValue);

            var rawValue = observable.BlockingValue();
            
            Assert.AreEqual(RandomStringValue, rawValue);
        }
    }
}