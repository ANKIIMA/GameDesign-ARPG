using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.Keyboard
{
    public interface IKeyInputState
    {
        bool IsKeyPressed(KeyCode code);
        bool IsKeyReleased(KeyCode code);
    }
}