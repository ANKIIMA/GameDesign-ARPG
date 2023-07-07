using System;

namespace SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters
{
    public class BioColor
    {
        public enum BioColorType { Red, Green, Blue, Yellow, Black, White, Orange, Purple }

        private readonly BioColorType _type;

        public BioColor(BioColorType type)
        {
            _type = type;
        }

        public override string ToString()
        {
            switch (_type)
            {
                case BioColorType.Red: return "Red";
                case BioColorType.Green: return "Green";
                case BioColorType.Blue: return "Blue";
                case BioColorType.Yellow: return "Yellow";
                case BioColorType.Black: return "Black";
                case BioColorType.White: return "White";
                case BioColorType.Orange: return "Orange";
                case BioColorType.Purple: return "Purple";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}