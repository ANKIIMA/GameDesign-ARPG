using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.List.Holder;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.List.Holder
{
    public class ListItemHolderTests
    {
        private ItemHolder<string> _holder;
        private const string Data = "string_data";

        [SetUp]
        public void SetUp()
        {
            _holder = new ItemHolder<string>(null);
        }

        [Test]
        public void BindData_WillSaveData()
        {
            _holder.BindData(Data);
            
            Assert.AreEqual(_holder.GetData(), Data);
        }
    }
}
