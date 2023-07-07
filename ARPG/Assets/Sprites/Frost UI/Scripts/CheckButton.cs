using UnityEngine;
using UnityEngine.EventSystems;

namespace Michsky.UI.Frost
{
    public class CheckButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isOn;
        private Animator buttonAnimator;
        bool isHovering;

        void Start()
        {
            buttonAnimator = this.GetComponent<Animator>();
            CheckStart();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;

            if (isOn == true)
            {
                buttonAnimator.Play("Pressed to Hover");
            }

            else
            {
                buttonAnimator.Play("Hover");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isHovering = false;

            if (isOn == true)
            {
                buttonAnimator.Play("Pt Hover to Pt");
            }

            else
            {
                buttonAnimator.Play("Hover to Normal");
            }
        }

        public void Animate()
        {
            if (isOn == true)
            {
                buttonAnimator.Play("Normal");
                isOn = false;
            }

            else if (isOn == false && isHovering == true)
            {
                buttonAnimator.Play("Hover to Pressed");
                isOn = true;
            }

            else if (isOn == false && isHovering == false)
            {
                buttonAnimator.Play("Normal to Pressed");
                isOn = true;
            }
        }

        public void CheckStart()
        {
            if (isOn == true)
            {
                buttonAnimator.Play("Normal to Pressed");
            }

            else
            {
                buttonAnimator.Play("Normal");
            }
        }
    }
}