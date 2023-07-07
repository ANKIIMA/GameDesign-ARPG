using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Events;
using SharpUI.Source.Common.Util.Extensions;
using UniRx;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Events
{
    public class ElementEventDispatcherTests
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private IElementEventDispatcher _dispatcher;
        private IElementEventListener _listener;
        private bool _dispatched;

        [SetUp]
        public void SetUp()
        {
            var dispatcher = new ElementEventDispatcher();
            _dispatcher = dispatcher;
            _listener = dispatcher;
            _dispatched = false;
        }

        [Test]
        public void OnPressed_WillBeDispatched()
        {
            _listener.ObserveOnPressed().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnPressed();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnReleased_WillBeDispatched()
        {
            _listener.ObserveOnReleased().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnReleased();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnClicked_WillBeDispatched()
        {
            _listener.ObserveOnClicked().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnClicked();
            
            Assert.IsTrue(_dispatched);
        }

        [Test]
        public void OnLeftClicked_WillBeDispatched()
        {
            _listener.ObserveOnLeftClicked().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnLeftClicked();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnRightClicked_WillBeDispatched()
        {
            _listener.ObserveOnRightClicked().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnRightClicked();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnMiddleClicked_WillBeDispatched()
        {
            _listener.ObserveOnMiddleClicked().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnMiddleClicked();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnEnabled_WillBeDispatched()
        {
            _listener.ObserveOnEnabled().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnEnabled();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnDisabled_WillBeDispatched()
        {
            _listener.ObserveOnDisabled().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnDisabled();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnEnter_WillBeDispatched()
        {
            _listener.ObserveOnEntered().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnEnter();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnExit_WillBeDispatched()
        {
            _listener.ObserveOnExited().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnExit();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnSelect_WillBeDispatched()
        {
            _listener.ObserveOnSelected().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnSelect();
            
            Assert.IsTrue(_dispatched);
        }
        
        [Test]
        public void OnDeselect_WillBeDispatched()
        {
            _listener.ObserveOnDeselected().SubscribeWith(_disposable, _ => _dispatched = true);
            
            _dispatcher.OnDeselect();
            
            Assert.IsTrue(_dispatched);
        }
    }
}
