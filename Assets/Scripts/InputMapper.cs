using UnityEngine;
using UnityEngine.InputSystem;
public enum ButtonInput {
    LIGHT,
    MEDIUM,
    HEAVY
}
public class InputMapper : MonoBehaviour
{

    public ButtonInput? GetPressedButton() {
        // Keyboard
        if (Keyboard.current.uKey.wasPressedThisFrame) return ButtonInput.LIGHT;
        if (Keyboard.current.iKey.wasPressedThisFrame) return ButtonInput.MEDIUM;
        if (Keyboard.current.oKey.wasPressedThisFrame) return ButtonInput.HEAVY;

        // Gamepad
        if (Gamepad.current != null) {
            if (Gamepad.current.buttonWest.wasPressedThisFrame) return ButtonInput.LIGHT;
            if (Gamepad.current.buttonNorth.wasPressedThisFrame) return ButtonInput.MEDIUM;
            if (Gamepad.current.buttonEast.wasPressedThisFrame) return ButtonInput.HEAVY;
        }

        return null;
    }


}
