using System;

namespace SharpUI.Source.Data.Model.Character
{
    [Serializable]
    public class Character
    {
        public string Name { get; }
        public int Level { get; }
        public ClassType ClassType { get; }

        public Character(string name, int level, ClassType classType)
        {
            Name = name;
            Level = level;
            ClassType = classType;
        }
    }
}