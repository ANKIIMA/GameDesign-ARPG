using System;
using System.Collections.Generic;
using SharpUI.Source.Data.Model.Character;
using UniRx;

namespace SharpUI.Source.Client.UI.User.CharacterSelect
{
    public class CharacterSelectModel : ICharacterSelectModel
    {
        private const string LoginSceneName = "Login";
        private const string CharacterCreateScene = "CharacterCreate";
        private const string GameLoadingScene = "LoadingScene";
        
        private readonly List<Character> _characters = new List<Character>
        { // Mocked characters
            CharacterFactory.CreateWarriorCharacter("BoneCrusher", 10),
            CharacterFactory.CreateHunterCharacter("SlayerX", 47),
            CharacterFactory.CreateWarriorCharacter("Boki", 16),
            CharacterFactory.CreateCasterCharacter("MageCaster", 17),
            CharacterFactory.CreateWarriorCharacter("Simple", 80),
            CharacterFactory.CreateHunterCharacter("Terminator", 34),
            CharacterFactory.CreateWarriorCharacter("Ruller", 61),
            CharacterFactory.CreateCasterCharacter("Hack", 27),
            CharacterFactory.CreateWarriorCharacter("Tim", 100),
            CharacterFactory.CreateHunterCharacter("Diablo", 57),
            CharacterFactory.CreateWarriorCharacter("Lakii", 96),
            CharacterFactory.CreateCasterCharacter("Little", 39)
        };

        public CharacterSelectModel() { }

        public CharacterSelectModel(List<Character> characters) => _characters = characters;

        public IObservable<string> GetLoginScene() => Observable.Return(LoginSceneName);

        public IObservable<string> GetCharacterCreateScene() => Observable.Return(CharacterCreateScene);

        public IObservable<string> GetGameLoadingScene() => Observable.Return(GameLoadingScene);

        public IObservable<List<Character>> GetCharacters() => Observable.Return(_characters);

        public void DeleteCharacter(Character character)
        {
            _characters.Remove(character);
        }
    }
}