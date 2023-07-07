using System.Collections.Generic;
using SharpUI.Source.Client.UI.User.CharacterCreate.ArrowListAdapters;
using SharpUI.Source.Common.UI.Base.Component;
using SharpUI.Source.Common.UI.Elements.ArrowLists;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Client.UI.User.CharacterCreate
{
    public class CharacterCreateComponent
        : MonoBehaviourComponent<CharacterCreateComponent, CharacterCreatePresenter>, ICharacterCreateComponent
    {
        [SerializeField] public GameObject tooltipPrefab;
        
        [SerializeField] public RectButton buttonBack;
        [SerializeField] public RectButton buttonCreate;
        [SerializeField] public ArrowList arrowListSkinColor;
        [SerializeField] public ArrowList arrowListHairStyle;
        [SerializeField] public ArrowList arrowListHairColor;
        [SerializeField] public ArrowList arrowListEyeStyle;

        private BioColorListAdapter _skinColorListAdapter;
        private BioColorListAdapter _hairColorListAdapter;
        private DefaultStyleAdapter _hairStyleAdapter;
        private DefaultStyleAdapter _eyeStyleAdapter;

        private ICharacterCreatePresenter _presenter;

        protected override CharacterCreateComponent GetComponent() => this;

        public void SetupComponent()
        {
            _presenter = GetPresenter();
            
            _skinColorListAdapter = new BioColorListAdapter();
            _hairStyleAdapter = new DefaultStyleAdapter();
            _hairColorListAdapter = new BioColorListAdapter();
            _eyeStyleAdapter = new DefaultStyleAdapter();
            
            arrowListSkinColor.SetAdapter(_skinColorListAdapter);
            arrowListHairStyle.SetAdapter(_hairStyleAdapter);
            arrowListHairColor.SetAdapter(_hairColorListAdapter);
            arrowListEyeStyle.SetAdapter(_eyeStyleAdapter);

            buttonBack.GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => _presenter.OnBackClicked());
            
            buttonCreate.GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => _presenter.OnCreateClicked());
            
            _presenter.LoadData();
        }

        public void RenderBioColors(List<BioColor> colors)
        {
            _skinColorListAdapter.SetData(colors);
            _hairColorListAdapter.SetData(colors);
        }

        public void RenderDefaultStyles(List<DefaultStyle> styles)
        {
            _hairStyleAdapter.SetData(styles);
            _eyeStyleAdapter.SetData(styles);
        }
    }
}