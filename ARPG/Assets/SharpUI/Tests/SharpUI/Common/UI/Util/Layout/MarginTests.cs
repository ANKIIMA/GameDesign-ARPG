using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Layout;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Layout
{
    public class MarginTests
    {
        private const float Left = 1.23f;
        private const float Right = 2.23f;
        private const float Top = -4.23f;
        private const float Bottom = -11.23f;
        private Margin _margin;

        [SetUp]
        public void SetUp()
        {
            _margin = new Margin(Left, Right, Top, Bottom);
        }

        [Test]
        public void Margin_WillSetLeftMarginCorrectly()
        {
            Assert.AreEqual(Left, _margin.Left);
        }
        
        [Test]
        public void Margin_WillSetRightMarginCorrectly()
        {
            Assert.AreEqual(Right, _margin.Right);
        }
        
        [Test]
        public void Margin_WillSetTopMarginCorrectly()
        {
            Assert.AreEqual(Top, _margin.Top);
        }
        
        [Test]
        public void Margin_WillSetBottomMarginCorrectly()
        {
            Assert.AreEqual(Bottom, _margin.Bottom);
        }

        [Test]
        public void Margin_Zero_HaveCorrectValues()
        {
            var zero = new Margin(0, 0, 0, 0);
            
            Assert.AreEqual(zero.Left, Margin.Zero.Left);
            Assert.AreEqual(zero.Right, Margin.Zero.Right);
            Assert.AreEqual(zero.Top, Margin.Zero.Top);
            Assert.AreEqual(zero.Bottom, Margin.Zero.Bottom);
        }
    }
}