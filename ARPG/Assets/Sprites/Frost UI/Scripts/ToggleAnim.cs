using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.Frost
{
    public class ToggleAnim : MonoBehaviour
    {
        [Header("TOGGLE")]
        public Toggle toggleObject;

        [Header("ANIMATORS")]
        public Animator toggleAnimator;

        [Header("ANIM NAMES")]
        public string toggleOn;
        public string toggleOff;

        void Start()
        {
            this.toggleObject.GetComponent<Toggle>();
            toggleObject.onValueChanged.AddListener(TaskOnClick);

            if (toggleObject.isOn)
            {
                toggleAnimator.Play(toggleOn);
            }

            else
            {
                toggleAnimator.Play(toggleOff);
            }
        }

        void TaskOnClick(bool value)
        {
            if (toggleObject.isOn)
            {
                toggleAnimator.Play(toggleOn);
            }

            else
            {
                toggleAnimator.Play(toggleOff);
            }
        }

        public void AnimateToggle()
        {
            if (toggleObject.isOn)
            {
                toggleAnimator.Play(toggleOff);
                toggleObject.isOn = false;
            }

            else
            {
                toggleAnimator.Play(toggleOn);
                toggleObject.isOn = true;
            }
        }
    }
}