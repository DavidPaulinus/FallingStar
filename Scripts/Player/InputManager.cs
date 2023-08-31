using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInput input;

    private void Awake()
    {
        input = new PlayerInput();
    }

    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }

    public float RetriveXValue()
    {
        return input.Move.Move.ReadValue<Vector2>().x;
    }
    public float RetriveYValue()
    {
        return input.Move.Move.ReadValue<Vector2>().y;
    }
    public bool RetriveJump()
    {
        return input.Move.Jump.WasPressedThisFrame();
    }
    public bool RetriveHoldingJump()
    {
        return input.Move.Jump.IsPressed();
    }

    public bool RetriveDash()
    {
        return input.Dash.Dash.WasPressedThisFrame();
    }
}
