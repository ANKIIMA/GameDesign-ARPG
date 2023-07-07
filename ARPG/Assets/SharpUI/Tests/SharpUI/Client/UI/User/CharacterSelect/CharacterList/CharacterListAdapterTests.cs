using System.Collections.Generic;
using NUnit.Framework;
using SharpUI.Source.Client.UI.User.CharacterSelect.CharacterList;
using SharpUI.Source.Common.UI.Elements.List.ListItem;
using SharpUI.Source.Common.UI.Elements.List.Selection;
using SharpUI.Source.Common.UI.Factory;
using SharpUI.Source.Common.Util.Extensions;
using SharpUI.Source.Data.Model.Character;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Tests.SharpUI.Client.UI.User.CharacterSelect.CharacterList
{
    public class CharacterListAdapterTests
    {
        private const int DataCount = 5;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private FakeCharacterListAdapter _adapter;
        private readonly List<Character> _data = new List<Character>(DataCount)
        {
            CharacterFactory.CreateWarriorCharacter("BoneCrusher", 10),
            CharacterFactory.CreateHunterCharacter("SlayerX", 47),
            CharacterFactory.CreateWarriorCharacter("Boki", 16),
            CharacterFactory.CreateCasterCharacter("MageCaster", 17),
            CharacterFactory.CreateWarriorCharacter("Simple", 80)
        };
        
        public class FakeCharacterListAdapter : CharacterListAdapter
        {
            public CharacterItemHolder FakeCreateListItemHolder()
            {
                return (CharacterItemHolder) base.CreateListItemHolder();
            }

            public void FakeBindListItemHolder(CharacterItemHolder holder, int position)
            {
                base.BindListItemHolder(holder, position);
            }
        }
        
        [SetUp]
        public void SetUp()
        {
            _adapter = CreateFakeAdapter();
            _adapter.SetSelectionStrategy(ItemSelectionType.All);
        }

        private FakeCharacterListAdapter CreateFakeAdapter()
        {
            var gameObject = new GameObject();
            gameObject.AddComponent<FakeCharacterListAdapter>();
            var adapter = gameObject.GetComponent<FakeCharacterListAdapter>();
            adapter.itemFactory = gameObject.AddComponent<ListItemFactory>();
            adapter.characterIconFactory = gameObject.AddComponent<CharacterIconFactory>();
            
            var lipGameObject = new GameObject();
            lipGameObject.AddComponent<ItemText>();
            var liipGameObject = new GameObject();
            liipGameObject.AddComponent<ItemImage>();
            
            adapter.itemFactory.listItemPrefab = lipGameObject;
            adapter.itemFactory.listItemImagePrefab = liipGameObject;
            
            return adapter;
        }
        
        [Test]
        public void SetCharactersAndNotify_WillNotifyDataObserver()
        {
            var observed = false;
            _adapter.ObserveDataChange().SubscribeWith(_disposables, _ => observed = true);
            
            _adapter.SetCharactersAndNotify(_data);
            
            Assert.IsTrue(observed);
        }
        
        [Test]
        public void SetCharactersAndNotify_WillReturnCorrectDataCount()
        {
            _adapter.SetCharactersAndNotify(_data);
            
            Assert.AreEqual(DataCount, _adapter.DataCount());
        }
        
        [Test]
        public void CreateListItemHolder_ReturnsCorrectHolder()
        {
            var holder = _adapter.FakeCreateListItemHolder();
            
            Assert.IsInstanceOf<CharacterItemHolder>(holder);
        }
        
        [Test]
        public void BindListItemHolder_WillBindAll()
        {
            _adapter.SetCharactersAndNotify(_data);
            var itemImage = new GameObject().AddComponent<ItemImage>();
            itemImage.textTitle = new GameObject().AddComponent<TextMeshProUGUI>();
            itemImage.textDescription = new GameObject().AddComponent<TextMeshProUGUI>();
            itemImage.imageIcon = new GameObject().AddComponent<Image>();
            var characterIconFactory = new GameObject().AddComponent<CharacterIconFactory>();
            characterIconFactory.iconCasterPrefab = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.zero);
            characterIconFactory.iconHunterPrefab = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.zero);
            characterIconFactory.iconWarriorPrefab = Sprite.Create(Texture2D.blackTexture, Rect.zero, Vector2.zero);
            var holder = new CharacterItemHolder(itemImage, characterIconFactory);

            for (var i = 0; i < _data.Count; i++)
            {
                _adapter.FakeBindListItemHolder(holder, i);

                Assert.AreEqual(_data[i], holder.GetData());
            }
        }
    }
}