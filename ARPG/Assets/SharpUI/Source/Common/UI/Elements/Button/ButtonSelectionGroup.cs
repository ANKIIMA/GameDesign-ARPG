using System.Collections.Generic;
using System.Linq;
using SharpUI.Source.Common.Util.Extensions;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.Button
{
    public class ButtonSelectionGroup : MonoBehaviour
    {
        [SerializeField] public bool selectButtonByIndex;
        [SerializeField] public int selectAtIndex;
        
        private List<BaseButton> _selectableButtons = new List<BaseButton>();

        public void Awake()
        {
            LoadButtons();
        }

        public void Start()
        {
            InitSelection();
            InitSelectionButtons();
            SelectIfRequested();
        }

        private void LoadButtons()
        {
            _selectableButtons = GetComponentsInChildren<BaseButton>().ToList();
        }

        private void InitSelection()
        {
            foreach (var button in _selectableButtons)
            {
                button.SetSelected(false);
                button.EnablePermanentSelection();
            }
        }
        
        private void InitSelectionButtons()
        {
            for (var i = 0; i < _selectableButtons.Count; i++)
            {
                var index = (ushort) i;
                var button = _selectableButtons[index];
                button.GetEventListener().ObserveOnSelected().SubscribeWith(this,
                    _ => OnButtonSelected(index));
            }
        }

        private void SelectIfRequested()
        {
            if (!selectButtonByIndex || _selectableButtons.ElementAt(selectAtIndex) == null)
                return;
            
            SelectAtIndex(selectAtIndex);
        }

        public void SelectAtIndex(int index)
        {
            if (_selectableButtons.ElementAtOrDefault(index) == null)
                return;

            _selectableButtons[index].SetSelected(true);
        }

        private void OnButtonSelected(ushort buttonIndex)
        {
            DeselectOthers(buttonIndex);
        }

        private void DeselectOthers(ushort selectedIndex)
        {
            for (var index = 0; index < _selectableButtons.Count; index++)
            {
                var button = _selectableButtons[index];
                if (button.GetState().IsSelected() && selectedIndex != index)
                    button.SetSelected(false);
            }
        }
    }
}