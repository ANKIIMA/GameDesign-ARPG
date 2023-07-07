using NUnit.Framework;
using SharpUI.Source.Data.Model.Character;

namespace SharpUI.Tests.SharpUI.Data.Model.Character
{
    public class CharacterTests
    {
        private global::SharpUI.Source.Data.Model.Character.Character _character;
        private const string Name = "Name";
        private const int Level = 123;
        private const ClassType Type = ClassType.Warrior;

        [SetUp]
        public void SetUp()
        {
            _character = new global::SharpUI.Source.Data.Model.Character.Character(Name, Level, Type);
        }
        
        [Test]
        public void Character_NameSat()
        {
            Assert.AreEqual(Name, _character.Name);
        }

        [Test]
        public void Character_LevelSat()
        {
            Assert.AreEqual(Level, _character.Level);
        }

        [Test]
        public void Character_TypeSat()
        {
            Assert.AreEqual(Type, _character.ClassType);
        }
    }
}
