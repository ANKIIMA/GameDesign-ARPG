using SharpUI.Source.Common.UI.Base.Presenter;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Data.Model.Character;

namespace SharpUI.Source.Client.UI.User.CharacterSelect
{
    public class CharacterSelectPresenter : BasePresenter<ICharacterSelectComponent>, ICharacterSelectPresenter
    {
        private readonly ICharacterSelectModel _model;
        private bool _isCharacterSelected;

        public CharacterSelectPresenter()
        {
            _model = new CharacterSelectModel();
        }
        
        public CharacterSelectPresenter(ICharacterSelectModel model)
        {
            _model = model;
        }
        
        public override void OnComponentStarted()
        {
            base.OnComponentStarted();
            LoadData();
        }

        private void LoadData()
        {
            LoadCharacters();
        }

        private void LoadCharacters()
        {
            _model.GetCharacters()
                .SubscribeWith(disposables, characters =>
                    OnComponent(component => component.ListCharacters(characters)));
        }

        public void OnBack()
        {
            _model.GetLoginScene().SubscribeWith(disposables, sceneName => ShowScene(sceneName));
        }

        public void OnEnterWorld()
        {
            _model.GetGameLoadingScene().SubscribeWith(disposables, sceneName => ShowScene(sceneName));
        }

        public void OnCreateNewCharacter()
        {
            _model.GetCharacterCreateScene().SubscribeWith(disposables, sceneName => ShowScene(sceneName));
        }

        public void OnDeleteCharacter(Character character)
        {
            _model.DeleteCharacter(character);
            OnComponent(component =>
            {
                component.DeleteSelected();
                component.SetDeleteButtonEnabled(false);
                component.HideDeleteCharacterTooltip();
            });
        }

        public void OnCharacterSelected(Character character)
        {
            OnComponent(component => component.ShowCharacterDetails(character));
        }

        public void OnCharacterSelectionChanged(bool characterSelected)
        {
            _isCharacterSelected = characterSelected;
            OnComponent(component =>
            {
                component.SetDeleteButtonEnabled(characterSelected);
                component.SetEnterWorldEnabled(characterSelected);
            });
        }

        public void OnPointerEnteredDeleteCharacterButton()
        {
            OnComponent(component =>
                {
                    if (_isCharacterSelected)
                        component.ShowDeleteCharacterTooltip();
                }
            );
        }

        public void OnPointerExitedDeleteCharacterButton()
        {
            OnComponent(component => component.HideDeleteCharacterTooltip());
        }
    }
}