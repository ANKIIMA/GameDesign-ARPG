using NUnit.Framework;
using SharpUI.Source.Common.UI.Util;

namespace SharpUI.Tests.SharpUI.Common.UI.Util
{
    public class UiUtilTests
    {
        private UiUtil _util;

        [SetUp]
        public void SetUp()
        {
            _util = new UiUtil();
        }

        [Test]
        public void ToDecimalPercentage_WorksCorrectly()
        {
            Assert.AreEqual(0.1f, _util.ToDecimalPercentage(10));
            Assert.AreEqual(0.12f, _util.ToDecimalPercentage(12));
            Assert.AreEqual(0.7f, _util.ToDecimalPercentage(70));
            Assert.AreEqual(8.3f, _util.ToDecimalPercentage(830));
            Assert.AreEqual(-0.7f, _util.ToDecimalPercentage(-70));
            Assert.AreEqual(-8.3f, _util.ToDecimalPercentage(-830));
        }

        [Test]
        public void ToPercentage_WorksCorrectly()
        {
            Assert.AreEqual(10f, _util.ToPercentage(0.1f));
            Assert.AreEqual(12f, _util.ToPercentage(0.12f));
            Assert.AreEqual(70f, _util.ToPercentage(0.70f));
            Assert.AreEqual(830f, _util.ToPercentage(8.30f));
            Assert.AreEqual(-70f, _util.ToPercentage(-0.70f));
            Assert.AreEqual(-830f, _util.ToPercentage(-8.30f));
        }

        [Test]
        public void PercentInRange_WorksCorrectly()
        {
            Assert.IsTrue(_util.PercentInRange(45f));
            Assert.IsTrue(_util.PercentInRange(0f));
            Assert.IsTrue(_util.PercentInRange(100f));
            
            Assert.IsFalse(_util.PercentInRange(145f));
            Assert.IsFalse(_util.PercentInRange(-5f));
        }

        [Test]
        public void Half_WorksCorrectly()
        {
            Assert.AreEqual(30f, _util.Half(60f));
            Assert.AreEqual(-30f, _util.Half(-60f));
        }
    }
}