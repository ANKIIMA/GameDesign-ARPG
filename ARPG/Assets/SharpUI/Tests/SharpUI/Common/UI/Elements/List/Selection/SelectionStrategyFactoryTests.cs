using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Holder;
using SharpUI.Source.Common.UI.Elements.List.Selection;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Selection
{
    public class SelectionStrategyFactoryTests
    {
        private const ItemSelectionType WrongSelectionType = 0;
        
        [Test]
        public void WhenWrongSelectionType_WillThrowCorrectException()
        {
            Assert.Throws<UnknownSelectionStrategyException>(
                () => SelectionStrategyFactory
                    .CreateSelectionStrategy<DefaultItemHolder, string>(WrongSelectionType, 0));
        }

        [Test]
        public void CreateSelectionStrategy_TypeNone_WillCreateCorrectSelectionStrategy()
        {
            var strategy = SelectionStrategyFactory.CreateSelectionStrategy<DefaultItemHolder, string>(
                ItemSelectionType.None);
            
            Assert.IsInstanceOf<SelectionStrategyNone<DefaultItemHolder, string>>(strategy);
        }
        
        [Test]
        public void CreateSelectionStrategy_TypeSingle_WillCreateCorrectSelectionStrategy()
        {
            var strategy = SelectionStrategyFactory.CreateSelectionStrategy<DefaultItemHolder, string>(
                ItemSelectionType.Single);
            
            Assert.IsInstanceOf<SelectionStrategySingle<DefaultItemHolder, string>>(strategy);
        }
        
        [Test]
        public void CreateSelectionStrategy_TypeLimited_WillCreateCorrectSelectionStrategy()
        {
            var strategy = SelectionStrategyFactory.CreateSelectionStrategy<DefaultItemHolder, string>(
                ItemSelectionType.Limited);
            
            Assert.IsInstanceOf<SelectionStrategyLimited<DefaultItemHolder, string>>(strategy);
        }
        
        [Test]
        public void CreateSelectionStrategy_TypeAll_WillCreateCorrectSelectionStrategy()
        {
            var strategy = SelectionStrategyFactory.CreateSelectionStrategy<DefaultItemHolder, string>(
                ItemSelectionType.All);
            
            Assert.IsInstanceOf<SelectionStrategyAll<DefaultItemHolder, string>>(strategy);
        }
    }
}
