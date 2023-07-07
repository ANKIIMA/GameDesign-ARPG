using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterCreate;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterCreate
{
    public class CharacterCreateModelTests
    {
        private ICharacterCreateModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new CharacterCreateModel();
        }

        [Test]
        public void GetCharacterSelectScene_WillReturnData()
        {
            var data = _model.GetCharacterSelectScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetBioColors_WillReturnNonemptyData()
        {
            var data = _model.GetBioColors().BlockingValue();

            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetDefaultStyles_WillReturnNonemptyData()
        {
            var data = _model.GetDefaultStyles().BlockingValue();

            Assert.IsNotEmpty(data);
        }
    }
}