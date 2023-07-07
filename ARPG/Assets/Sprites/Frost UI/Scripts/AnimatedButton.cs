using UnityEngine;
using UnityEngine.Events;

namespace Michsky.UI.Frost
{
    public class AnimatedButton : MonoBehaviour
    {
        [Header("SETTINGS")]
        public bool isOn;

        public UnityEvent onEvents;
        public UnityEvent offEvents;

        private string onAnim = "On";
        private string offAnim = "Off";

        private Animator objAnimator;

        void Start()
        {
            objAnimator = gameObject.GetComponent<Animator>();
            if (isOn == true)
            {
                objAnimator.Play(onAnim);
                onEvents.Invoke();
            }

            else
            {
                objAnimator.Play(offAnim);
                offEvents.Invoke();
            }
        }

        public void Animate()
        {
            if (isOn == true)
            {
                objAnimator.Play(offAnim);
                offEvents.Invoke();
                isOn = false;
            }

            else
            {
                objAnimator.Play(onAnim);
                onEvents.Invoke();
                isOn = true;
            }
        }
    }
}