using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.UI.Elements.Events;
using SharpUI.Source.Common.UI.Elements.State;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.Events
{
    public class ItemEventPromoterTests
    {
        private const int DecoratorsCount = 25;
        
        private IElementEventDispatcher _eventDispatcher;
        private IElementState _state;
        private List<IDecorator> _decorators;
        
        private ItemEventPromoter _promoter;
        
        [SetUp]
        public void SetUp()
        {
            _eventDispatcher = Substitute.For<IElementEventDispatcher>();
            _state = Substitute.For<IElementState>();
            _decorators = CreateMockedDecorators();
            _promoter = new ItemEventPromoter(_eventDispatcher, _state, _decorators);
        }
        
        private static List<IDecorator> CreateMockedDecorators()
        {
            var decorators = new List<IDecorator>(DecoratorsCount);
            decorators.AddRange(Enumerable.Range(1, DecoratorsCount)
                    .Select(_ => Substitute.For<IDecorator>()));
            return decorators;
        }

        [Test]
        public void SelectItem_WillPromoteStateToSelected()
        {
            _promoter.SelectItem();
            
            _state.Received().SelectIfSelectable();
        }
        
        [Test]
        public void SelectItem_WhenStateSelected_WillDispatchAndDecorateSelected()
        {
            _state.IsSelected().Returns(true);
            
            _promoter.SelectItem();
            
            _eventDispatcher.Received().OnSelect();
            _decorators.ForEach(decorator => decorator.Received().OnSelected());
        }
        
        [Test]
        public void SelectItem_WhenStateDeselected_WillNotDispatchNorDecorateSelected()
        {
            _state.IsDeselected().Returns(true);
            
            _promoter.SelectItem();
            
            _eventDispatcher.DidNotReceive().OnSelect();
            _decorators.ForEach(decorator => decorator.DidNotReceive().OnSelected());
        }
        
        [Test]
        public void DeselectItem_WillPromoteStateToDeselected()
        {
            _promoter.DeselectItem();
            
            _state.Received().DeselectIfSelectable();
        }
        
        [Test]
        public void DeselectItem_WhenStateNotSelected_WillDispatchAndDecorateDeselected()
        {
            _state.IsSelected().Returns(false);
            
            _promoter.DeselectItem();
            
            _eventDispatcher.Received().OnDeselect();
            _decorators.ForEach(decorator => decorator.Received().OnDeselected());
        }
        
        [Test]
        public void DeselectItem_WhenStateSelected_WillNotDispatchNorDecorateDeselected()
        {
            _state.IsSelected().Returns(true);
            _promoter.DeselectItem();
            
            _eventDispatcher.DidNotReceive().OnDeselect();
            _decorators.ForEach(decorator => decorator.DidNotReceive().OnDeselected());
        }
    }
}
