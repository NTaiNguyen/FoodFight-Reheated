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

    // For AI mode
    // Added by Cyler on 12/10/25
    private ButtonInput? forcedButton = null;
    public bool isAI = false;

    public ButtonInput? GetPressedButton()
    {
        // Basically the AI is using the same input as a player, just virtual
        // So the AI is forcing an input and then it registers and executes, then the input is cleared and another is done
        if(isAI && forcedButton != null)
        {
            ButtonInput? b = forcedButton;
            forcedButton = null;
            return b;
        }

        // Proceed with both characters controlable if not in AI mode
        if (!isAI)
        {
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
                if (Keyboard.current.numpad1Key.wasPressedThisFrame) return ButtonInput.LIGHT;
                if (Keyboard.current.numpad2Key.wasPressedThisFrame) return ButtonInput.MEDIUM;
                if (Keyboard.current.numpad3Key.wasPressedThisFrame) return ButtonInput.HEAVY;

                // Gamepad
                if (Gamepad.current != null) {
                    if (Gamepad.current.buttonWest.wasPressedThisFrame) return ButtonInput.LIGHT;
                    if (Gamepad.current.buttonNorth.wasPressedThisFrame) return ButtonInput.MEDIUM;
                    if (Gamepad.current.buttonEast.wasPressedThisFrame) return ButtonInput.HEAVY;
                }
            }
        }
        
        return null;
    }

    public void ForcedButtonPress(ButtonInput button)
    {
        forcedButton = button;
    }
}
