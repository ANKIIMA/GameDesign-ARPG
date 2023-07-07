using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.UI.Frost
{
    public class TimedProgressBar : MonoBehaviour
    {
        [Header("OBJECTS")]
        public Transform loadingBar;
        public Transform textPercent;

        [Header("VARIABLES (IN-GAME)")]
        public bool isOn;
        [Range(0, 100)] public float currentPercent;
        [Range(0.1f, 100)] public int speed;
        [Range(0, 100)] public float totalValue;

        void Update()
        {
            if (isOn == true && currentPercent >= 0)
            {
                currentPercent -= speed * Time.deltaTime;
            }

            loadingBar.GetComponent<Image>().fillAmount = currentPercent / totalValue;
            textPercent.GetComponent<TextMeshProUGUI>().text = ((int)currentPercent).ToString("F0");
        }

        public void SetTimer(int timer)
        {
            currentPercent = timer;
        }
    }
}