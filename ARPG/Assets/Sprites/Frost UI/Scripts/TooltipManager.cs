using UnityEngine;

namespace Michsky.UI.Frost
{
    public class TooltipManager : MonoBehaviour
    {
        [Header("RESOURCES")]
        [SerializeField]
        private Camera UICamera;
        public GameObject tooltipObject;
        public GameObject tooltipContent;
        public RectTransform tooltipHelper;

        [Header("SETTINGS")]
        [Range(0.05f, 0.5f)] public float tooltipSmoothness = 0.1f;
        // public Vector3 tooltipPosition = new Vector3(120, -60, 0);

        [Header("TOOLTIP BOUNDS")]
        public int vBorderTop = -115;
        public int vBorderBottom = 100;
        public int hBorderLeft = 230;
        public int hBorderRight = -210;

        Vector2 uiPos;
        Vector3 cursorPos;
        RectTransform tooltipRect;
        private Vector3 contentPos = new Vector3(0, 0, 0);
        Vector3 tooltipVelocity = Vector3.zero;

        void Start()
        {
            tooltipObject.SetActive(true);
            tooltipRect = tooltipObject.GetComponent<RectTransform>();
            // tooltipContent.transform.localPosition = tooltipPosition;
            contentPos = new Vector3(vBorderTop, hBorderLeft, 0);
        }

        void Update()
        {
            cursorPos = Input.mousePosition;
            cursorPos.z = tooltipHelper.position.z;
            tooltipRect.position = UICamera.ScreenToWorldPoint(cursorPos);
            uiPos = tooltipRect.anchoredPosition;
            tooltipContent.transform.localPosition = Vector3.SmoothDamp(tooltipContent.transform.localPosition, contentPos, ref tooltipVelocity, tooltipSmoothness);
            CheckForBounds();
        }

        public void CheckForBounds()
        {
            if (uiPos.x <= -300)
            {
                contentPos = new Vector3(hBorderLeft, contentPos.y, 0);
            }

            if (uiPos.x >= 300)
            {
                contentPos = new Vector3(hBorderRight, contentPos.y, 0);
            }

            if (uiPos.y <= -250)
            {
                contentPos = new Vector3(contentPos.x, vBorderBottom, 0);
            }

            if (uiPos.y >= 250)
            {
                contentPos = new Vector3(contentPos.x, vBorderTop, 0);
            }
        }
    }
}