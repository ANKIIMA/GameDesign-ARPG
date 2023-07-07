using UnityEngine;

namespace SharpUI.Source.Common.UI.Util.Keyboard
{
    public class KeyInputState : IKeyInputState
    {
        public bool IsKeyPressed(KeyCode code) => Input.GetKeyDown(code);

        public bool IsKeyReleased(KeyCode code) => Input.GetKeyUp(code);
    }
}