using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.UI.Util.Event;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Util.Keyboard
{
    public class SimpleKeyListener : MonoBehaviour, IKeyListener
    {
        public const string DefaultInputTextTag = "InputField";
        private static IEnumerable<string> DefaultGameObjectFilterTags => new List<string> { DefaultInputTextTag };
        private readonly List<string> _gameObjectFilterTags = new List<string>(DefaultGameObjectFilterTags);
        private readonly Subject<Unit> _allKeysDownObserver = new Subject<Unit>();
        private readonly Subject<Unit> _anyKeyUpObserver = new Subject<Unit>();

        [CanBeNull] private BaseButton _baseButton;
        [CanBeNull] private Button _button;
        private IKeyInputState _state = new KeyInputState();
        private ICurrentGameObjectProvider _currentGameObjectProvider = new CurrentGameObjectProvider();
        private KeyCode _keyCode;
        private bool _requireShift;
        private bool _shiftDown;
        private bool _requireControl;
        private bool _controlDown;

        public void Update()
        {
            if (!ShouldRecordKeyEvents())
                return;

            CheckDown();
            CheckUp();
        }

        public void SetKeyInputState(IKeyInputState state) => _state = state;

        public void SetCurrentGameObjectProvider(ICurrentGameObjectProvider provider) =>
            _currentGameObjectProvider = provider;

        public void TakeButton(BaseButton button)
        {
            if (button == null) return;
            
            _baseButton = button;
            _button = _baseButton.GetComponent<Button>();
        }

        private bool ShouldRecordKeyEvents()
        {
            var currentGameObject = _currentGameObjectProvider.GetCurrentSelectedGameObject();
            foreach (var filterTag in _gameObjectFilterTags)
                if (currentGameObject && currentGameObject.CompareTag(filterTag))
                    return false;

            return true;
        }

        private void CheckDown()
        {
            if (_state.IsKeyPressed(KeyCode.LeftShift) ||
                _state.IsKeyPressed(KeyCode.RightShift)) _shiftDown = true;
            if (_state.IsKeyPressed(KeyCode.LeftControl) ||
                _state.IsKeyPressed(KeyCode.RightControl)) _controlDown = true;

            if (!RequiredExtrasValid()) return;
            if (!_state.IsKeyPressed(_keyCode)) return;
            
            _allKeysDownObserver.OnNext(Unit.Default);
            KeyDown();
        }

        private void CheckUp()
        {
            if (_state.IsKeyReleased(KeyCode.LeftShift) ||
                _state.IsKeyReleased(KeyCode.RightShift)) _shiftDown = false;
            if (_state.IsKeyReleased(KeyCode.LeftControl) ||
                _state.IsKeyReleased(KeyCode.RightControl)) _controlDown = false;

            if (!RequiredExtrasValid()) return;
            if (!_state.IsKeyReleased(_keyCode)) return;
            
            _anyKeyUpObserver.OnNext(Unit.Default);
            KeyUp();
        }
        
        private void KeyDown()
        {
            if (_baseButton == null || _button == null) return;
            
            _baseButton.GetDecorators().OnPressed();
            _button.onClick.Invoke();
        }

        private void KeyUp()
        {
            if (_baseButton == null) return;
            
            _baseButton.GetDecorators().OnReleased();
        }

        private bool RequiredExtrasValid()
        {
            if (_requireShift && !_shiftDown || !_requireShift && _shiftDown) return false;
            if (_requireControl && !_controlDown || !_requireControl && _controlDown) return false;

            return true;
        }

        public void RequireAnyShift(bool require = true) => _requireShift = require;

        public void RequireAnyControl(bool require = true) => _requireControl = require;

        public void AddFilterTag(string filterTag) => _gameObjectFilterTags.Add(filterTag);

        public void RemoveFilterTag(string filterTag) => _gameObjectFilterTags.Remove(filterTag);

        public IObservable<Unit> ObserveDown() => _allKeysDownObserver;
        
        public IObservable<Unit> ObserveUp() => _anyKeyUpObserver;
        
        public void ListenToKey(KeyCode keyCode) => _keyCode = keyCode;
    }
}