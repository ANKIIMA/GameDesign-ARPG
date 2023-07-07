using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterCreate.ArrowListAdapters
{
    public class DefaultStyleAdapterTests
    {
        private static readonly DefaultStyle Style1 = new DefaultStyle(DefaultStyle.DefaultStyleType.Style1);
        private static readonly DefaultStyle Style2 = new DefaultStyle(DefaultStyle.DefaultStyleType.Style2);
        private static readonly DefaultStyle Style3 = new DefaultStyle(DefaultStyle.DefaultStyleType.Style3);
        private readonly List<DefaultStyle> _data = new List<DefaultStyle> { Style1, Style2, Style3 };
        private DefaultStyleAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _adapter = new DefaultStyleAdapter();
            _adapter.SetData(_data);
        }

        [Test]
        public void Init_CurrentItem_IsFirstItem()
        {
            Assert.AreSame(_data[0].ToString(), _adapter.CurrentItem());
        }
        
        [Test]
        public void Init_PreviousItem_IsNotAvailable()
        {
            Assert.IsNull(_adapter.PreviousItem());
        }

        [Test]
        public void Init_NextItem_IsSecondItem()
        {
            Assert.AreSame(_data[1].ToString(), _adapter.NextItem());
        }
    }
}