using System;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Util.Scenes;

namespace SharpUI.Tests.SharpUI.Common.UI.Util.Scene
{
    public class SceneUtilsTests
    {
        private const string LoginSceneName = "Login";
        private SceneUtils _utils;

        [SetUp]
        public void SetUp()
        {
            _utils = new SceneUtils();
        }

        [Test]
        public void LoadSceneAsync_OutsidePlayMode_WillFail()
        {
            var scene = _utils.LoadSceneAsync(LoginSceneName);

            Assert.Throws<InvalidOperationException>(() => CoroutineHelper.RunSynchronously(scene));
        }
    }
}