using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterCreate.ArrowListAdapters
{
    public class BioColorListAdapterTests
    {
        private static readonly BioColor BlackColorType = new BioColor(BioColor.BioColorType.Black);
        private static readonly BioColor BlueColorType = new BioColor(BioColor.BioColorType.Blue);
        private static readonly BioColor RedColorType = new BioColor(BioColor.BioColorType.Red);
        private readonly List<BioColor> _data = new List<BioColor>
        {
            BlackColorType, BlueColorType, RedColorType
        };
        private BioColorListAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _adapter = new BioColorListAdapter();
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