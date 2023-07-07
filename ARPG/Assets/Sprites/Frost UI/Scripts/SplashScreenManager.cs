using UnityEngine;

namespace Michsky.UI.Frost
{
    public class SplashScreenManager : MonoBehaviour
    {
        [Header("RESOURCES")]
        public GameObject splashScreen;
        public GameObject mainPanels;
        public Animator backgroundAnimator;
        public Animator startFadeIn;
        private Animator mainPanelsAnimator;
        private Animator splashScreenAnimator;

        [Header("SETTINGS")]
        public bool disableSplashScreen;
        public bool enableLoginScreen = true;

        void Start()
        {
            mainPanelsAnimator = mainPanels.GetComponent<Animator>();
            splashScreenAnimator = splashScreen.GetComponent<Animator>();

            if (disableSplashScreen == true)
            {
                splashScreen.SetActive(false);
                mainPanels.SetActive(true);
                mainPanelsAnimator.Play("Panel Start");
                backgroundAnimator.Play("Switch");
            }

            else
            {
                splashScreen.SetActive(true);
                mainPanelsAnimator.Play("Wait");
                startFadeIn.Play("Start with Splash");
            }

            if (enableLoginScreen == false && disableSplashScreen == false)
            {
                splashScreenAnimator.Play("Loading");
            }
        }
    }
}