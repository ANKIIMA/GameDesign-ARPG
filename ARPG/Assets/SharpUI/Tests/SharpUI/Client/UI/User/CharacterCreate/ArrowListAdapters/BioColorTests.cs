using System;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterCreate.ArrowListAdapters
{
    public class BioColorTests
    {
        private BioColor _bioColor;

        [Test]
        public void BioColor_AnyType_WillReturnStringValue()
        {
            var types = (BioColor.BioColorType[]) Enum.GetValues(typeof(BioColor.BioColorType));
            foreach (var colorType in types)
            {
                _bioColor = new BioColor(colorType);
                Assert.IsNotEmpty(_bioColor.ToString());
            }
        }

        [Test]
        public void BioColor_UnknownType_WillThrowException()
        {
            const BioColor.BioColorType unknownColorType = (BioColor.BioColorType) int.MinValue;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _bioColor = new BioColor(unknownColorType);
                var _ = _bioColor.ToString();
            });
        }
    }
}