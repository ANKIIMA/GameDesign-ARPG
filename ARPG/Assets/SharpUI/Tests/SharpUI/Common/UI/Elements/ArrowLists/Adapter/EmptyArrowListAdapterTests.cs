using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.ArrowLists.Adapter
{
    public class EmptyArrowListAdapterTests
    {
        private EmptyArrowListAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _adapter = new EmptyArrowListAdapter();
        }

        [Test]
        public void CurrentItem_IsEmpty()
        {
            Assert.IsEmpty(_adapter.CurrentItem());
        }
        
        [Test]
        public void PreviousItem_IsEmpty()
        {
            Assert.IsEmpty(_adapter.PreviousItem());
        }
        
        [Test]
        public void NextItem_IsEmpty()
        {
            Assert.IsEmpty(_adapter.NextItem());
        }
    }
}