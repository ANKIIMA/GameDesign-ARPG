using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterSelect;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Data.Model.Character;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterSelect
{
    public class CharacterSelectModelTests
    {
        private ICharacterSelectModel _model;
        
        private readonly List<Character> _characters = new List<Character>
        {
            CharacterFactory.CreateWarriorCharacter("BoneCrusher", 10),
            CharacterFactory.CreateHunterCharacter("SlayerX", 47),
            CharacterFactory.CreateWarriorCharacter("Boki", 16),
            CharacterFactory.CreateCasterCharacter("MageCaster", 17),
            CharacterFactory.CreateWarriorCharacter("Simple", 80)
        };

        [SetUp]
        public void SetUp()
        {
            _model = new CharacterSelectModel(_characters);
        }

        [Test]
        public void CharacterSelectModel_CanCreateEmptyConstructor()
        {
            _model = new CharacterSelectModel();
        }

        [Test]
        public void GetLoginScene_WillReturnData()
        {
            var data = _model.GetLoginScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetCharacterCreateScene_WillReturnData()
        {
            var data = _model.GetCharacterCreateScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetGameLoadingScene_WillReturnData()
        {
            var data = _model.GetGameLoadingScene().BlockingValue();
            
            Assert.IsNotEmpty(data);
        }

        [Test]
        public void GetCharacters_WillReturnData()
        {
            var data = _model.GetCharacters().BlockingValue();

            Assert.IsNotEmpty(data);
        }

        [Test]
        public void DeleteCharacter_WillDeleteCorrectAmountOfCharacters()
        {
            var size = _characters.Count;
            var character = _characters[1];
            var character2 = _characters[2];
            
            _model.DeleteCharacter(character);
            _model.DeleteCharacter(character2);
            
            Assert.AreEqual(size-2, _characters.Count);
        }

        [Test]
        public void DeleteCharacter_WillDeleteCorrectCharacter()
        {
            var character = _characters[2];
            
            Assert.IsTrue(_characters.Contains(character));
            _model.DeleteCharacter(character);

            Assert.IsFalse(_characters.Contains(character));
        }
    }
}