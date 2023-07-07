using UnityEngine;
using UnityEngine.EventSystems;

namespace SharpUI.Source.Common.UI.Util.Event
{
    public class CurrentGameObjectProvider : ICurrentGameObjectProvider
    {
        public GameObject GetCurrentSelectedGameObject() => EventSystem.current.currentSelectedGameObject;
    }
}