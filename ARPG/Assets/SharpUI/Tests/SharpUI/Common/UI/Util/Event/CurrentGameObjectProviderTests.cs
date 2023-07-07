using System;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Event;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Event
{
    public class CurrentGameObjectProviderTests
    {
        private CurrentGameObjectProvider _provider;

        [SetUp]
        public void SetUp()
        {
            _provider = new CurrentGameObjectProvider();
        }

        [Test]
        public void GetCurrentSelectedGameObject_ReturnsNullException()
        {
            Assert.Throws<NullReferenceException>(() => _provider.GetCurrentSelectedGameObject());
        }
    }
}