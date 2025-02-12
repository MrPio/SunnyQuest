using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    public static void Reset() => _instance = new InputManager();

    private static InputManager _instance;

    private InputManager()
    {
    }

    public static InputManager Instance => _instance ??= new InputManager();

    public static bool IsMobile =>
        Application.platform is RuntimePlatform.Android or RuntimePlatform.IPhonePlayer;


    public bool HasJoystick() => Gamepad.current is not null;

    public Vector2 GetMovement
    {
        get
        {
            var in1 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            var in2 = new Vector2(Input.GetAxis("D-Pad Horizontal"), Input.GetAxis("D-Pad Vertical"));
            return in1.sqrMagnitude > 0.3f ? in1 : in2.sqrMagnitude > 0.3f ? in2 : Vector2.zero;
        }
    }

    public bool IsJumping => Gamepad.current.buttonSouth.isPressed || Input.GetKey(KeyCode.Space);

    public IEnumerator Vibrate(float duration = 0.5f)
    {
        if (HasJoystick())
        {
            Gamepad.current.SetMotorSpeeds(1f, 1f);
            yield return new WaitForSeconds(duration);
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
        else if (IsMobile)
        {
            // Handheld.Vibrate();
        }
    }
}