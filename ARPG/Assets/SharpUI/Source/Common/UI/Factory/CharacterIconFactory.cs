using System;
using SharpUI.Source.Data.Model.Character;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Factory
{
    public class CharacterIconFactory : MonoBehaviour
    {
        [SerializeField] public Sprite iconWarriorPrefab;
        [SerializeField] public Sprite iconHunterPrefab;
        [SerializeField] public Sprite iconCasterPrefab;

        private Sprite CreateWarriorIcon() => Instantiate(iconWarriorPrefab);

        private Sprite CreateHunterIcon() => Instantiate(iconHunterPrefab);

        private Sprite CreateCasterIcon() => Instantiate(iconCasterPrefab);

        public Sprite CreateSpriteFor(ClassType classType)
        {
            switch (classType)
            {
                case ClassType.Warrior: return CreateWarriorIcon();
                case ClassType.Hunter: return CreateHunterIcon();
                case ClassType.Caster: return CreateCasterIcon();
                default:
                    throw new ArgumentOutOfRangeException(nameof(classType), classType, "Unhandled classType");
            }
        }
    }
}
