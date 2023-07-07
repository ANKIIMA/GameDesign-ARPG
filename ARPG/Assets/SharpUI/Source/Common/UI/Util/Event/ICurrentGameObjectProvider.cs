using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.Event
{
    public interface ICurrentGameObjectProvider
    {
        GameObject GetCurrentSelectedGameObject();
    }
}