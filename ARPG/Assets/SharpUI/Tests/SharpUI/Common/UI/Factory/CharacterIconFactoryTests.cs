using System;
using NUnit.Framework;
using SharpUI.Source.Common.UI.Factory;
using SharpUI.Source.Data.Model.Character;
using UnityEngine;

namespace SharpUI.Tests.SharpUI.Common.UI.Factory
{
    public class CharacterIconFactoryTests
    {
        private const ClassType WrongClassType = (ClassType)int.MaxValue;
        
        private Sprite _spriteWarrior;
        private Sprite _spriteHunter;
        private Sprite _SpriteCaster;
        private CharacterIconFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new GameObject().AddComponent<CharacterIconFactory>();
            _spriteWarrior = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.zero);
            _spriteHunter = Sprite.Create(Texture2D.redTexture, Rect.zero, Vector2.left);
            _SpriteCaster = Sprite.Create(Texture2D.whiteTexture, Rect.zero, Vector2.one);
            _factory.iconWarriorPrefab = _spriteWarrior;
            _factory.iconHunterPrefab = _spriteHunter;
            _factory.iconCasterPrefab = _SpriteCaster;
        }

        [Test]
        public void CreateCharacterFor_WithWrongType_WillThrowException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _factory.CreateSpriteFor(WrongClassType));
        }

        [Test]
        public void CreateCharacterFor_Warrior_WillCreateCorrectSprite()
        {
            var sprite = _factory.CreateSpriteFor(ClassType.Warrior);
            
            Assert.AreSame(_spriteWarrior.texture, sprite.texture);
        }
        
        [Test]
        public void CreateCharacterFor_Hunter_WillCreateCorrectSprite()
        {
            var sprite = _factory.CreateSpriteFor(ClassType.Hunter);
            
            Assert.AreSame(_spriteHunter.texture, sprite.texture);
        }
        
        [Test]
        public void CreateCharacterFor_Caster_WillCreateCorrectSprite()
        {
            var sprite = _factory.CreateSpriteFor(ClassType.Caster);
            
            Assert.AreSame(_SpriteCaster.texture, sprite.texture);
        }
    }
}