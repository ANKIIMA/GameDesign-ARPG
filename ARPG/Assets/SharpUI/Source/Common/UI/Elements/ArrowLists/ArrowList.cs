using SharpUI.Source.Common.UI.Elements.ArrowLists.Adapter;
using SharpUI.Source.Common.UI.Elements.ArrowLists.Animation;
using SharpUI.Source.Common.UI.Elements.Button;
using SharpUI.Source.Common.Util.Extensions;
using TMPro;
using UniRx;
using UnityEngine;

namespace SharpUI.Source.Common.UI.Elements.ArrowLists
{
    public class ArrowList : MonoBehaviour
    {
        [SerializeField] public RectButton leftButton;
        [SerializeField] public RectButton rightButton;
        [SerializeField] public TMP_Text itemText;

        private CompositeDisposable _disposable = new CompositeDisposable();
        private IArrowListAnimator _textAnimator = new ArrowListAnimator();
        private IArrowListAdapter _adapter;

        public void Start()
        {
            ObserveButtonClicks();
            _textAnimator.BindTextComponent(itemText);
        }

        public void SetDisposable(CompositeDisposable disposable)
            => _disposable = disposable;

        public void SetArrowListAnimator(IArrowListAnimator textAnimator)
            => _textAnimator = textAnimator;

        private void ObserveButtonClicks()
        {
            leftButton.GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => LeftClicked());
            
            rightButton.GetEventListener()
                .ObserveOnClicked()
                .SubscribeWith(this, _ => RightClicked());
        }

        private void LeftClicked()
        {
            if (_textAnimator.IsAnimating())
                return;
            
            _textAnimator.CloneText();
            _adapter.SelectPrevious();
            _textAnimator.SlideRight();
        }
        
        private void RightClicked()
        {
            if (_textAnimator.IsAnimating())
                return;
            
            _textAnimator.CloneText();
            _adapter.SelectNext();
            _textAnimator.SlideLeft();
        }

        public void SetAdapter(IArrowListAdapter adapter)
        {
            _adapter = adapter;
            ObserveAdapterChanges();
        }

        private void ObserveAdapterChanges()
        {
            _disposable.Clear();
            _adapter.ObserveDataChange().SubscribeWith(_disposable, _ => Render());
            _adapter.ObserveSelectionChange().SubscribeWith(_disposable, _ => Render());
        }

        private void SetButtonInteractableStates()
        {
            SetLeftButtonInteractable();
            SetRightButtonInteractable();
        }

        private void SetLeftButtonInteractable()
        {
            if (_adapter.HasPreviousData())
                leftButton.EnableButton();
            else
                leftButton.DisableButton();
        }

        private void SetRightButtonInteractable()
        {
            if (_adapter.HasNextData())
                rightButton.EnableButton();
            else
                rightButton.DisableButton();
        }

        private void Render()
        {
            SetButtonInteractableStates();
            itemText.SetText(_adapter.CurrentItem() ?? "");
        }

        public void OnDestroy()
        {
            _textAnimator.Unbind();
        }
    }
}