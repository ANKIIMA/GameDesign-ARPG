using System;

namespace SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters
{
    public class DefaultStyle
    {
        public enum DefaultStyleType { Style1, Style2, Style3, Style4, Style5, Style6, Style7, Style8, Style9 }

        private readonly DefaultStyleType _type;

        public DefaultStyle(DefaultStyleType type)
        {
            _type = type;
        }

        public override string ToString()
        {
            switch (_type)
            {
                case DefaultStyleType.Style1: return "Style 1";
                case DefaultStyleType.Style2: return "Style 2";
                case DefaultStyleType.Style3: return "Style 3";
                case DefaultStyleType.Style4: return "Style 4";
                case DefaultStyleType.Style5: return "Style 5";
                case DefaultStyleType.Style6: return "Style 6";
                case DefaultStyleType.Style7: return "Style 7";
                case DefaultStyleType.Style8: return "Style 8";
                case DefaultStyleType.Style9: return "Style 9";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}