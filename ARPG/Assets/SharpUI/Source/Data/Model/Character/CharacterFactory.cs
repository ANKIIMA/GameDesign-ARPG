namespace SharpUI.Source.Data.Model.Character
{
    public static class CharacterFactory
    {
        public static Character CreateWarriorCharacter(string name, int level)
            => new Character(name, level, ClassType.Warrior);
        
        public static Character CreateHunterCharacter(string name, int level)
            => new Character(name, level, ClassType.Hunter);
        
        public static Character CreateCasterCharacter(string name, int level)
            => new Character(name, level, ClassType.Caster);
    }
}