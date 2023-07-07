using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SharpUI.Source.Common.UI.Elements.Decorators;
using SharpUI.Source.Common.UI.Elements.Events;
using SharpUI.Source.Common.UI.Elements.State;
using UnityEngine;
using UnityEngine.UI;

namespace SharpUI.Source.Common.UI.Elements
{
    public abstract class BaseElement : MonoBehaviour
    {
        [SerializeField] public bool isEnabled = true;
        [SerializeField] public bool isClickable;
        [SerializeField] public bool isSelectable;
        [SerializeField] public bool isSelected;

        [CanBeNull] protected Selectable selectableElement;
        protected ElementEventPromoter eventPromoter;
        protected ElementEventDispatcher dispatcher;
        protected ElementState state;
        protected List<IDecorator> decorators;
        
        public virtual void Awake()
        {
            SetupElement();
            SetupSelectable();
        }

        public virtual void Start()
        {
            SetupUI();
            ObserveEvents();
        }

        protected virtual void SetupUI()
        {
            SetClickable(isClickable);
            SetSelectable(isSelectable);
            SetSelected(isSelected);
        }
        
        public void SetClickable(bool clickable)
        {
            isClickable = clickable;
            if (clickable)
                state.MakeClickable();
            else
                state.MakeNonClickable();
        }

        public void SetSelectable(bool selectable)
        {
            isSelectable = selectable;
            if (selectable)
                state.MakeSelectable();
            else
                state.MakeNonSelectable();
        }
        
        public void SetSelected(bool selected)
        {
            isSelected = selected;
            if (selected)
            {
                eventPromoter.ObservedSelected();

                if (!state.IsSelected()) return;
                if (selectableElement != null)
                    selectableElement.Select();
            }
            else
            {
                eventPromoter.ObservedDeselect();
            }
        }

        public IElementEventListener GetEventListener() => dispatcher;

        public IElementState GetState() => state;

        public List<IDecorator> GetDecorators() => decorators;

        public IElementEventDispatcher GetEventDispatcher() => dispatcher;

        protected virtual void SetupElement()
        {
            decorators = gameObject.GetComponentsInChildren<IDecorator>().ToList();
            dispatcher = new ElementEventDispatcher();
            state = new ElementState();
            eventPromoter = new ElementEventPromoter(dispatcher, state, decorators);
        }

        protected abstract void SetupSelectable();

        protected abstract void ObserveEvents();
    }
}