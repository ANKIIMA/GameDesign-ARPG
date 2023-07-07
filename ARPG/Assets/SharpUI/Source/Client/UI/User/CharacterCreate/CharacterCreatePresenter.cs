using System.Collections.Generic;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;
using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;

namespace SharpUI.Source.Client.UI.User.CharacterCreate
{
    public class CharacterCreatePresenter : BasePresenter<ICharacterCreateComponent>, ICharacterCreatePresenter
    {
        private readonly ICharacterCreateModel _model;

        public CharacterCreatePresenter()
        {
            _model = new CharacterCreateModel();
        }
        
        public CharacterCreatePresenter(ICharacterCreateModel model)
        {
            _model = model;
        }

        public void LoadData()
        {
            _model.GetBioColors().SubscribeWith(disposables, ShowBioColors);
            _model.GetDefaultStyles().SubscribeWith(disposables, ShowDefaultStyles);
        }

        private void ShowBioColors(List<BioColor> bioColors)
            => OnComponent(component => component.RenderBioColors(bioColors));

        private void ShowDefaultStyles(List<DefaultStyle> defaultStyles) =>
            OnComponent(component => component.RenderDefaultStyles(defaultStyles));

        public void OnBackClicked()
        {
            _model.GetCharacterSelectScene()
                .SubscribeWith(disposables, sceneName => ShowScene(sceneName));
        }
        
        public void OnCreateClicked()
        {
            _model.GetCharacterSelectScene()
                .SubscribeWith(disposables, sceneName => ShowScene(sceneName));
        }
    }
}