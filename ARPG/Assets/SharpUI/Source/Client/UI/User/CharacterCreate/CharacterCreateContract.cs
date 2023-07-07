using System;
using System.Collections.Generic;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Base.Model;

namespace SharpUI.Source.Client.UI.User.CharacterCreate
{
    public interface ICharacterCreateComponent : IBaseComponent
    {
        void RenderBioColors(List<BioColor> colors);
        void RenderDefaultStyles(List<DefaultStyle> styles);
    }

    public interface ICharacterCreatePresenter
    {
        void OnBackClicked();
        void OnCreateClicked();
        void LoadData();
    }

    public interface ICharacterCreateModel : IBaseModel
    {
        IObservable<string> GetCharacterSelectScene();
        IObservable<List<BioColor>> GetBioColors();
        IObservable<List<DefaultStyle>> GetDefaultStyles();
    }
}