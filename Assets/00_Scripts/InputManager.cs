using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
    }

    private void OnApplicationQuit()
    {
        inputActions.Disable();
    }
}
