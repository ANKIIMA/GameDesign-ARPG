using System;
using System.Collections.Generic;
using SharpUI.Source.Client.UI.User.CharacterSelect.CharacterList;
using SharpUI.Source.Common.UI.Elements.List.Adapter;
using SharpUI.Source.Common.UI.Prototype.SharpButton;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Data.Model.Character;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Prototype.SharpList
{
    public class SharpListPrototype : MonoBehaviour
    {
        [FormerlySerializedAs("defaultList")] [SerializeField] public Elements.List.ListView defaultListView;
        [FormerlySerializedAs("iconList")] [SerializeField] public Elements.List.ListView iconListView;
        [FormerlySerializedAs("descriptionList")] [SerializeField] public Elements.List.ListView descriptionListView;
        [FormerlySerializedAs("textList")] [SerializeField] public Elements.List.ListView textListView;
        [FormerlySerializedAs("customList")] [SerializeField] public Elements.List.ListView customListView;
        [SerializeField] public Button enableToggle;
        [SerializeField] public Button clickableToggle;
        [SerializeField] public Button selectableToggle;
        [SerializeField] public Button clearAll;
        [SerializeField] public Button loadAll;
        
        private DefaultListAdapter _defaultAdapter;
        private DefaultListAdapter _textAdapter;
        private DescriptionListAdapter _descriptionAdapter;
        private CharacterListAdapter _charactersAdapter;
        private CustomListAdapter _customListAdapter;
        
        private readonly List<string> _textStrings = new List<string>
        {
            "First",
            "Second",
            "And last one as a really long text description!",
            "Fourth",
            "Fifth",
            "And last one as a really long text description!"
        };
        
        private readonly List<Tuple<string, string>> _descriptionData = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("Title", "Description"),
            new Tuple<string, string>("Hello World", "The description of the world!"),
            new Tuple<string, string>("Info", "There is no place like home."),
            new Tuple<string, string>("Long description", "This text should represent a very long description that spans over two lines."),
            new Tuple<string, string>("Title", "Description"),
            new Tuple<string, string>("Hello World", "The description of the world!"),
            new Tuple<string, string>("Info", "There is no place like home."),
            new Tuple<string, string>("Long description", "This text should represent a very long description that spans over two lines."),
            new Tuple<string, string>("Title", "Description"),
            new Tuple<string, string>("Hello World", "The description of the world!"),
            new Tuple<string, string>("Info", "There is no place like home."),
            new Tuple<string, string>("Long description", "This text should represent a very long description that spans over two lines.")
        };
        
        private readonly List<Character> _characters = new List<Character>
        {
            CharacterFactory.CreateWarriorCharacter("BoneCrusher", 10),
            CharacterFactory.CreateHunterCharacter("SlayerX", 47),
            CharacterFactory.CreateWarriorCharacter("Boki", 16),
            CharacterFactory.CreateCasterCharacter("MageCaster", 17),
            CharacterFactory.CreateWarriorCharacter("Simple", 80),
            CharacterFactory.CreateHunterCharacter("Terminator", 34),
            CharacterFactory.CreateWarriorCharacter("Ruller", 61),
            CharacterFactory.CreateCasterCharacter("Hack", 27),
            CharacterFactory.CreateWarriorCharacter("Tim", 100),
            CharacterFactory.CreateHunterCharacter("Diablo", 57),
            CharacterFactory.CreateWarriorCharacter("Lakii", 96),
            CharacterFactory.CreateCasterCharacter("Little", 39)
        };
        
        private readonly List<Tuple<string, float>> _customData = new List<Tuple<string, float>>
        {
            new Tuple<string, float>("Strength", 240.3f),
            new Tuple<string, float>("Stamina", 182.6f),
            new Tuple<string, float>("Resistance", 34.9f)
        };
        
        public void Start()
        {
            _defaultAdapter = defaultListView.GetComponent<DefaultListAdapter>();
            _textAdapter = textListView.GetComponent<DefaultListAdapter>();
            _descriptionAdapter = descriptionListView.GetComponent<DescriptionListAdapter>();
            _charactersAdapter = iconListView.GetComponent<CharacterListAdapter>();
            _customListAdapter = customListView.GetComponent<CustomListAdapter>();
            
            _defaultAdapter.SetItemsAndNotify(_textStrings);
            _textAdapter.SetItemsAndNotify(_textStrings);
            _descriptionAdapter.SetItemsAndNotify(_descriptionData);
            _charactersAdapter.SetCharactersAndNotify(_characters);
            _customListAdapter.SetItemsAndNotify(_customData);
            
            enableToggle.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    iconListView.SetItemsEnabled(!iconListView.enableItems);
                    enableToggle.GetComponentInChildren<Text>().text =
                        iconListView.enableItems ? "Disable items" : "Enable items";
                });
            
            clickableToggle.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    iconListView.SetItemsClickable(!iconListView.canClickItems);
                    clickableToggle.GetComponentInChildren<Text>().text =
                        iconListView.canClickItems ? "Disable clicks" : "Enable clicks";
                });
            
            selectableToggle.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    iconListView.SetItemsSelectable(!iconListView.canSelectItems);
                    selectableToggle.GetComponentInChildren<Text>().text =
                        iconListView.canSelectItems ? "Disable selection" : "Enable selection";
                });
            
            clearAll.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    _charactersAdapter.ClearAll();
                });
            
            loadAll.OnClickAsObservable()
                .SubscribeWith(this, _ =>
                {
                    _charactersAdapter.SetCharactersAndNotify(_characters);
                });
        }
    }
}