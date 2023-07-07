using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Decorators
{
    public class DecoratorExtensionsTests
    {
        private const int DecoratorsCount = 25;
        private List<IDecorator> _decorators;

        [SetUp]
        public void SetUp()
        {
            _decorators = new List<IDecorator>(DecoratorsCount);
            for (var i = 0; i < DecoratorsCount; i++)
            {
                var decorator = Substitute.For<IDecorator>();
                _decorators.Add(decorator);
            }
        }

        [Test]
        public void OnSelected_WillSelectAllDecorators()
        {
            _decorators.OnSelected();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnSelected();
        }
        
        [Test]
        public void OnDeselected_WillDeselectAllDecorators()
        {
            _decorators.OnDeselected();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnDeselected();
        }
        
        [Test]
        public void OnEnter_WillEnterAllDecorators()
        {
            _decorators.OnEnter();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnEnter();
        }
        
        [Test]
        public void OnExit_WillExitAllDecorators()
        {
            _decorators.OnExit();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnExit();
        }
        
        [Test]
        public void OnEnabled_WillEnableAllDecorators()
        {
            _decorators.OnEnabled();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnEnabled();
        }
        
        [Test]
        public void OnDisabled_WillDisableAllDecorators()
        {
            _decorators.OnDisabled();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnDisabled();
        }
        
        [Test]
        public void OnPressed_WillPressAllDecorators()
        {
            _decorators.OnPressed();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnPressed();
        }
        
        [Test]
        public void OnReleased_WillReleaseAllDecorators()
        {
            _decorators.OnReleased();
            
            foreach (var decorator in _decorators)
                decorator.Received().OnReleased();
        }
    }
}
