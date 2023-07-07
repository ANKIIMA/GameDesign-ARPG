using System;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterCreate.ArrowListAdapters
{
    public class DefaultStyleTests
    {
        private DefaultStyle _defaultStyle;
        
        [Test]
        public void DefaultStyle_AnyType_WillReturnStringValue()
        {
            var types = (DefaultStyle.DefaultStyleType[]) Enum.GetValues(typeof(DefaultStyle.DefaultStyleType));
            foreach (var styleType in types)
            {
                _defaultStyle = new DefaultStyle(styleType);
                Assert.IsNotEmpty(_defaultStyle.ToString());
            }
        }
        
        [Test]
        public void DefaultStyle_UnknownType_WillThrowException()
        {
            const DefaultStyle.DefaultStyleType unknownStyleType = (DefaultStyle.DefaultStyleType) int.MinValue;
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _defaultStyle = new DefaultStyle(unknownStyleType);
                var _ = _defaultStyle.ToString();
            });
        }
    }
}