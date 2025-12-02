using UnityEngine;
using UnityEngine.InputSystem;
public enum ButtonInput {
    LIGHT,
    MEDIUM,
    HEAVY
}
public class InputMapper : MonoBehaviour
{

    // Added by Cyler on 12/1/25
    // Telling game which player is which

    public int playerID;

    public ButtonInput? GetPressedButton() {

        // Added by Cyler on 12/1/25
        if (playerID == 1)
        {
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
        // Added by Cyler on 12/1/25
        } else if (playerID == 2)
        {
            // Keyboard
            if (Keyboard.current.jKey.wasPressedThisFrame) return ButtonInput.LIGHT;
            if (Keyboard.current.kKey.wasPressedThisFrame) return ButtonInput.MEDIUM;
            if (Keyboard.current.lKey.wasPressedThisFrame) return ButtonInput.HEAVY;

            // Gamepad
            if (Gamepad.current != null) {
                if (Gamepad.current.buttonWest.wasPressedThisFrame) return ButtonInput.LIGHT;
                if (Gamepad.current.buttonNorth.wasPressedThisFrame) return ButtonInput.MEDIUM;
                if (Gamepad.current.buttonEast.wasPressedThisFrame) return ButtonInput.HEAVY;
            }
        }
        
        return null;
    }


}
