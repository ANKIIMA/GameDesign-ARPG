using System;
using System.Collections.Generic;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;
using UniRx;

namespace SharpUI.Source.Client.UI.User.CharacterCreate
{
    public class CharacterCreateModel : ICharacterCreateModel
    {
        private const string CharacterSelectScene = "CharacterSelection";
        private static List<BioColor> BioColors => new List<BioColor>
        {
            new BioColor(BioColor.BioColorType.Red),
            new BioColor(BioColor.BioColorType.White),
            new BioColor(BioColor.BioColorType.Yellow),
            new BioColor(BioColor.BioColorType.Purple),
            new BioColor(BioColor.BioColorType.Orange),
            new BioColor(BioColor.BioColorType.Green),
            new BioColor(BioColor.BioColorType.Black),
            new BioColor(BioColor.BioColorType.Blue)
        };
        private static List<DefaultStyle> DefaultStyles => new List<DefaultStyle>
        {
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style1),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style2),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style3),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style4),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style5),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style6),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style7),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style8),
            new DefaultStyle(DefaultStyle.DefaultStyleType.Style9)
        };

        public IObservable<string> GetCharacterSelectScene() => Observable.Return(CharacterSelectScene);

        public IObservable<List<BioColor>> GetBioColors() => Observable.Return(BioColors);
        
        public IObservable<List<DefaultStyle>> GetDefaultStyles() => Observable.Return(DefaultStyles);
    }
}