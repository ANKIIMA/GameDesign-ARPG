using JetBrains.Annotations;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Toggle
{
    public class ToggleGroup : MonoBehaviour
    {
        [SerializeField] public bool selectByIndex;
        [SerializeField] public ushort selectedIndex;
        
        private ToggleButton[] _toggleButtons;
        private ushort _selectedIndex = ushort.MaxValue;

        public void Awake()
        {
            LoadToggleButtons();
            InitToggleButtons();
        }

        public void Start()
        {
            SelectIfRequested();
        }

        private void SelectIfRequested()
        {
            if (selectByIndex && HasToggleAtIndex(selectedIndex))
                ToggleButtonSelected(selectedIndex);
        }

        private void LoadToggleButtons()
        {
            _toggleButtons = GetComponentsInChildren<ToggleButton>();
        }

        private void InitToggleButtons()
        {
            for (var i = 0; i < _toggleButtons.Length; i++)
            {
                var index = (ushort) i;
                var toggleButton = _toggleButtons[index];
                toggleButton.ToggleOff();
                toggleButton.ObserveToggleStateChange().SubscribeWith(this,
                    _ => ToggleButtonSelected(index));
            }
        }

        private void ToggleButtonSelected(ushort buttonIndex)
        {
            ToggleCurrentOff();
            _selectedIndex = buttonIndex;
            _toggleButtons[buttonIndex].SetIsOn(true);
        }

        private void ToggleCurrentOff()
        {
            if (_selectedIndex != ushort.MaxValue)
                _toggleButtons[_selectedIndex].SetIsOn(false);
        }

        public ushort GetSelectedIndex() => _selectedIndex;

        private bool HasToggleAtIndex(ushort index) => index < _toggleButtons.Length;

        [CanBeNull] public ToggleButton GetSelectedButton()
            => _selectedIndex != ushort.MaxValue ? _toggleButtons[_selectedIndex] : null;

        public int ButtonCount() => _toggleButtons.Length;
    }
}