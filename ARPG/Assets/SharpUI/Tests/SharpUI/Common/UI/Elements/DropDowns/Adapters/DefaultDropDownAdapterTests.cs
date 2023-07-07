using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Elements.DropDowns.Adapters;

namespace SharpUI.Tests.SharpUI.Common.UI.Elements.DropDowns.Adapters
{
    public class DefaultDropDownAdapterTests
    {
        private static readonly List<string> Data = new List<string> { "d1", "d2", "d3", "d4", "d5", "d6", "d7", "d8" };
        private DefaultDropDownAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _adapter = new DefaultDropDownAdapter();
            _adapter.SetData(Data);
        }

        [Test]
        public void GetItemTextAt_WillReturnCorrectData()
        {
            for (var index = 0; index < Data.Count; index++)
                Assert.AreEqual(Data[index], _adapter.GetItemTextAt(index));
        }
    }
}